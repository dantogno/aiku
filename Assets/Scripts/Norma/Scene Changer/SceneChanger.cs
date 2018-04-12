using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Invoked when the player start the scene transition.
    /// </summary>
    public static event Action SceneChangeStarted;
    /// <summary>
    /// Invoked when the player has finished switching scenes.
    /// </summary>
    public static event Action SceneChangeFinished;

    [Tooltip("The name of the scene this object loads when Interacted with")]
    [SerializeField]
    private string sceneToLoad;

    // List of scenes that have been loaded so far.
    private static List<Scene> loadedScenes;
    private static Dictionary<string, Dictionary<GameObject, bool>> objectList;

    // Fields to keep track of whether the scenes have finished loading and unloading.
    private bool disabledFinished = false;
    private bool enabledFinished = false;

    // Make sure not to load the static List more than once.
    private void Start()
    {
        if(loadedScenes == null)
        {
            objectList = new Dictionary<string, Dictionary<GameObject, bool>>();
            loadedScenes = new List<Scene>();
            // The initial scene needs to be added to the list.
            Scene currentScene = gameObject.scene;
            loadedScenes.Add(currentScene);
            AddToObjectList(currentScene);
        }
    }

    private void Update()
    {
        // Only called after the player has initiated a scene switch.
        if (disabledFinished && enabledFinished)
        {
            disabledFinished = false;
            enabledFinished = false;
            SendSceneLoadedMessage(sceneToLoad);
        }
    }

    private void AddToObjectList(Scene scene)
    {
        string sceneName = scene.name;
        GameObject[] objects = scene.GetRootGameObjects();
        Dictionary<GameObject, bool> newObjectList = new Dictionary<GameObject, bool>();
        foreach(GameObject gameObject in objects)
        {
            bool isActive = false;
            if(gameObject.activeSelf == true)
            {
                isActive = true;
            }
            newObjectList.Add(gameObject, isActive);
        }
        objectList.Add(sceneName, newObjectList);
    }

    private Dictionary<GameObject, bool> LoadFromObjectList(Scene scene)
    {
        string sceneName = scene.name;
        Dictionary<GameObject, bool> newObjectList = objectList[sceneName];
        return newObjectList;
    }

    /// <summary>
    /// Sends out a notification that the player has finished changing scenes.
    /// </summary>
    /// <param name="sceneName">The name of the scene that was loaded</param>
    private void SendSceneLoadedMessage(string sceneName)
    {
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(newScene);
        if(SceneChangeFinished != null)
        {
            SceneChangeFinished.Invoke();
        }
        // Disable this object last.
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// When the player interacts with this object, it should eiter Load a new scene
    /// or enable an existing one.
    /// </summary>
    /// <param name="agentInteracting">The player interacting with the scene changing object</param>
    public void Interact(GameObject agentInteracting)
    {
        foreach (Scene scene in loadedScenes)
        {
            if (sceneToLoad == scene.name)
            {
                // If the scene has been loaded before, reload it.
                LoadExistingScene(sceneToLoad);
                return;
            }
        }
        // If the scene hasn't been loaded before, load it now.
        LoadNewScene(sceneToLoad);
    }

    /// <summary>
    /// Loads a scene if it has never been loaded before.
    /// </summary>
    /// <param name="sceneName">The name of the new scene to load</param>
    private void LoadNewScene(string sceneName)
    {
        // Start masking scene transition
        if (SceneChangeStarted != null)
        {
            SceneChangeStarted.Invoke();
        }
        AsyncOperation loadNewScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        StartCoroutine(WaitUntilSceneIsLoaded(sceneName, loadNewScene));
    }

    /// <summary>
    /// Enables a scene if it has already been loaded, and disables the old one.
    /// </summary>
    /// <param name="sceneName">The name of the scene to enable</param>
    private void LoadExistingScene(string sceneName)
    {
        // Start masking scene transition
        if (SceneChangeStarted != null)
        {
            SceneChangeStarted.Invoke();
        }
        Scene currentScene = gameObject.scene;
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        //Disable the current scene and load the new one
        DisableAndEnable(currentScene, newScene);
    }

    /// <summary>
    /// Disables one scene and enables another.
    /// </summary>
    /// <param name="sceneToDisable">The scene to disable</param>
    /// <param name="sceneToEnable">The scene to enable</param>
    private void DisableAndEnable(Scene sceneToDisable, Scene sceneToEnable)
    {
        StartCoroutine(EnableScene(sceneToEnable));
        StartCoroutine(DisableScene(sceneToDisable));
    }

    /// <summary>
    /// A coroutine that waits until a new scene has been loaded before attempting
    /// to disable the old scene and enable the new scene
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    private IEnumerator WaitUntilSceneIsLoaded(string sceneName, AsyncOperation operation)
    {
        if (operation.isDone == false)
        {
            yield return null;
        }
        Scene currentScene = gameObject.scene;
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        // Add new scene to static field
        loadedScenes.Add(newScene);
        AddToObjectList(newScene);
        // Disable the current scene and load the new one
        DisableAndEnable(currentScene, newScene);
    }

    /// <summary>
    /// A coroutine that enables a scene.
    /// When it has finished running, it sets enableFinished to true.
    /// </summary>
    /// <param name="sceneToEnable">The scene that should be enabled</param>
    /// <returns></returns>
    private IEnumerator EnableScene(Scene sceneToEnable)
    {
        Dictionary<GameObject, bool> objectsToEnable = LoadFromObjectList(sceneToEnable);
        //GameObject[] objectsToEnable = sceneToEnable.GetRootGameObjects();
        foreach (KeyValuePair<GameObject, bool> entry in objectsToEnable)
        {
            if(entry.Key != null)
            {
                if (entry.Value == true)
                {
                    entry.Key.SetActive(true);
                }
                //thisObject.SetActive(true);
                yield return null;
            }
        }
        enabledFinished = true;
    }

    /// <summary>
    /// A coroutine that disables a scene.
    /// When it has finished running, it sets disabledFinished to true.
    /// </summary>
    /// <param name="sceneToDisable">The scene that should be disabled</param>
    /// <returns></returns>
    private IEnumerator DisableScene(Scene sceneToDisable)
    {
        UpdateObjectList(sceneToDisable);
        GameObject[] objectsToDisable = sceneToDisable.GetRootGameObjects();
        foreach (GameObject thisObject in objectsToDisable)
        {
            if (thisObject != this.gameObject)
            {
                thisObject.SetActive(false);
                yield return null;
            }
        }
        disabledFinished = true;
    }

    private void UpdateObjectList(Scene sceneToUpdate)
    {
        string sceneName = sceneToUpdate.name;
        objectList.Remove(sceneName);
        AddToObjectList(sceneToUpdate);
    }
}
