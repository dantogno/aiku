using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used to switch between scenes.
/// This script should be attached to a root gameobject.
/// It works by disabling all object in a single scene and either
/// loading a new scene, or enabling all objects in a scene that has been
/// loaded before.
/// </summary>
public class SceneTransition : MonoBehaviour
{
    /// <summary>
    /// Invoked when the player start the scene transition.
    /// </summary>
    public static event Action SceneChangeStarted;
    /// <summary>
    /// Invoked when the player has finished switching scenes.
    /// </summary>
    public static event Action SceneChangeFinished;

    // List of scenes that have been loaded so far.
    private static List<Scene> loadedScenes;
    /// <summary>
    /// A list of scenes that have been loaded so far.
    /// </summary>
    public static List<Scene> LoadedScenes
    {
        get
        {
            return loadedScenes;
        }
    }

    private static List<string> loadedSceneNames;
    /// <summary>
    /// The names of the scenes that have been loaded.
    /// </summary>
    public static List<string> LoadedSceneNames
    {
        get
        {
            return loadedSceneNames;
        }
    }

    /// <summary>
    /// The number of scenes currently loaded.
    /// </summary>
    public static int NumberOfScenesLoaded
    {
        get
        {
            return loadedScenes.Count;
        }
    }

    private static string currentScene;
    /// <summary>
    /// The name of the scene currently loaded.
    /// </summary>
    public static string CurrentScene
    {
        get
        {
            return currentScene;
        }
        private set
        {
            currentScene = value;
        }
    }

    // Check to see if a level load is already in progress before attempting to load a new one.
    private static bool isLoading = false;
    /// <summary>
    /// Is a level load currently in progress?
    /// </summary>
    public static bool IsLoading
    {
        get
        {
            return isLoading;
        }
    }

    // Data structure to hold which object are enabled or disabled in a specific scene.
    private static Dictionary<string, Dictionary<GameObject, bool>> objectList;

    // Fields to keep track of whether the scenes have finished loading and unloading.
    private static bool disabledFinished = false;
    private static bool enabledFinished = false;
    // A field to keep track of which scene is currently being loaded.
    private static string sceneToLoad = "";

    // Needed to ensure a singleton.
    private static SceneTransition instance;

    private void Awake()
    {
        // Initialize what is needed
        InitializeComponents();
        // Add the first scene to the list.
        AddFirstScene();
        // Make the Scene Transition object a singleton.
        EnsureSingleton();
    }

    /// <summary>
    /// Initializes components that are necessary for this script.
    /// </summary>
    private void InitializeComponents()
    {
        loadedScenes = new List<Scene>();
        objectList = new Dictionary<string, Dictionary<GameObject, bool>>();
        loadedSceneNames = new List<string>();
    }

    /// <summary>
    /// Adds the first scene to the scene list.
    /// </summary>
    private void AddFirstScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        // Add the first scene to the scene list.
        loadedScenes.Add(currentScene);
        // Add the new scene to the name list.
        loadedSceneNames.Add(currentScene.name);
        // Add Information about the objects in the scene.
        AddToObjectList(currentScene);
        // Set this scene as the current scene.
        CurrentScene = currentScene.name;
    }

    /// <summary>
    /// Ensures that this object will be a singleton.
    /// </summary>
    private void EnsureSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        // Check to see if the scene switch currently in progress has finished.
        CheckIfSceneSwitchIsDone();
    }

    private void CheckIfSceneSwitchIsDone()
    {
        // Make sure that this class has finished disabling the old scene and
        // enabling the new one.
        if (disabledFinished && enabledFinished)
        {
            disabledFinished = false;
            enabledFinished = false;
            SendSceneLoadedMessage(sceneToLoad);
        }
    }

    /// <summary>
    /// Loads a new scene by index using the Scene Transition manager.
    /// Will not work if another load is currently in progress.
    /// </summary>
    /// <param name="buildIndex">Build index of the scene to load</param>
    public static void LoadScene(int buildIndex)
    {
        string sceneName = SceneManager.GetSceneByBuildIndex(buildIndex).name;
        LoadScene(sceneName);
    }

    /// <summary>
    /// Loads a new scene by name using the Scene Transition manager.
    /// Will not work if another load is currently in progress.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public static void LoadScene(string sceneName)
    {
        // Don't do anything if a load is already in progress.
        if(isLoading == false)
        {
            isLoading = true;
            // Set the name of the scene that is currently loading.
            sceneToLoad = sceneName;
            foreach (Scene scene in loadedScenes)
            {
                if (sceneName == scene.name)
                {
                    // If the scene has been loaded before, reload it.
                    LoadExistingScene(sceneName);
                    return;
                }
            }
            // If the scene hasn't been loaded before, load it now.
            LoadNewScene(sceneName);
        }
    }

    /// <summary>
    /// Add information about whether objects in a scene are enabled or disabled.
    /// </summary>
    /// <param name="scene">The scene whose object states to store</param>
    private static void AddToObjectList(Scene scene)
    {
        string sceneName = scene.name;
        // We only want to store information about the root game objects.
        GameObject[] objects = scene.GetRootGameObjects();
        // A data structure that holds a list of objects with information about whether objects
        // are enabled or disabled.
        Dictionary<GameObject, bool> newObjectList = new Dictionary<GameObject, bool>();
        foreach (GameObject gameObject in objects)
        {
            // Keeps track of whether the object is enabled or disabled.
            bool isActive = false;
            if (gameObject.activeSelf == true)
            {
                // If the object is enabled, make note of it.
                isActive = true;
            }
            // Add the object to the list along with its state.
            newObjectList.Add(gameObject, isActive);
        }
        // Add the collection of object information and relate it to the scene using the scene name.
        objectList.Add(sceneName, newObjectList);
    }

    /// <summary>
    /// Loads information about whether objects in a scene were enabled or disabled.
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    private static Dictionary<GameObject, bool> LoadFromObjectList(Scene scene)
    {
        string sceneName = scene.name;
        // Load the information from the list where it was stored before.
        Dictionary<GameObject, bool> newObjectList = objectList[sceneName];
        return newObjectList;
    }

    /// <summary>
    /// Refresh the information about the state of the objects in a scene.
    /// (Whether they are enabled or disabled).
    /// </summary>
    /// <param name="sceneToUpdate"></param>
    private static void UpdateObjectList(Scene sceneToUpdate)
    {
        string sceneName = sceneToUpdate.name;
        objectList.Remove(sceneName);
        AddToObjectList(sceneToUpdate);
    }

    /// <summary>
    /// Sends out a notification that the player has finished changing scenes.
    /// </summary>
    /// <param name="sceneName">The name of the scene that was loaded</param>
    private void SendSceneLoadedMessage(string sceneName)
    {
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        // Make sure to set it active for future transitions.
        SceneManager.SetActiveScene(newScene);
        // Let anyone who cares know that the scene transition is finished.
        if (SceneChangeFinished != null)
        {
            SceneChangeFinished.Invoke();
        }
        // Set the name of the current scene as the new scene that was loaded.
        CurrentScene = sceneToLoad;
        sceneToLoad = "";
        // Now that everything is done, another scene can be loaded again.
        isLoading = false;
    }

    /// <summary>
    /// Enables a scene if it has already been loaded, and disables the old one.
    /// </summary>
    /// <param name="sceneName">The name of the scene to enable</param>
    private static void LoadExistingScene(string sceneName)
    {
        // Let anyone that cares know that a scene change has started.
        if (SceneChangeStarted != null)
        {
            SceneChangeStarted.Invoke();
        }
        Scene currentScene = SceneManager.GetActiveScene();
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        //Disable the current scene and load the new one
        DisableAndEnable(currentScene, newScene);
    }

    /// <summary>
    /// Loads a scene if it has never been loaded before.
    /// </summary>
    /// <param name="sceneName">The name of the new scene to load</param>
    private static void LoadNewScene(string sceneName)
    {
        // Let anyone that cares know that a scene change has started.
        if (SceneChangeStarted != null)
        {
            SceneChangeStarted.Invoke();
        }
        // Load the scene asynchronously
        AsyncOperation loadNewScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        instance.StartCoroutine(WaitUntilSceneIsLoaded(sceneName, loadNewScene));
    }

    /// <summary>
    /// Disables one scene and enables another.
    /// </summary>
    /// <param name="sceneToDisable">The scene to disable</param>
    /// <param name="sceneToEnable">The scene to enable</param>
    private static void DisableAndEnable(Scene sceneToDisable, Scene sceneToEnable)
    {
        instance.StartCoroutine(EnableScene(sceneToEnable));
        instance.StartCoroutine(DisableScene(sceneToDisable));
    }

    /// <summary>
    /// A coroutine that waits until a new scene has been loaded before attempting
    /// to disable the old scene and enable the new scene
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    private static IEnumerator WaitUntilSceneIsLoaded(string sceneName, AsyncOperation operation)
    {
        // Wait until the new scene has been fully loaded before disabling the old scene and
        // enabling the new one.
        while (operation.isDone == false)
        {
            yield return null;
        }
        Scene currentScene = SceneManager.GetActiveScene();
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        // Add new scene to static field
        loadedScenes.Add(newScene);
        // Add the new scene's name to the name list.
        loadedSceneNames.Add(newScene.name);
        // Add information about the objects in the new scene.
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
    private static IEnumerator EnableScene(Scene sceneToEnable)
    {
        // Enable all root game object that were enabled before.
        Dictionary<GameObject, bool> objectsToEnable = LoadFromObjectList(sceneToEnable);
        foreach (KeyValuePair<GameObject, bool> entry in objectsToEnable)
        {
            if (entry.Key != null)
            {
                if (entry.Value == true)
                {
                    entry.Key.SetActive(true);
                }
                yield return null;
            }
        }
        // The enable portion of the scene transition is now finished.
        enabledFinished = true;
    }

    /// <summary>
    /// A coroutine that disables a scene.
    /// When it has finished running, it sets disabledFinished to true.
    /// </summary>
    /// <param name="sceneToDisable">The scene that should be disabled</param>
    /// <returns></returns>
    private static IEnumerator DisableScene(Scene sceneToDisable)
    {
        // Refresh the state of the objects in the scene.
        UpdateObjectList(sceneToDisable);
        // Disable all root game obejcts.
        GameObject[] objectsToDisable = sceneToDisable.GetRootGameObjects();
        foreach (GameObject thisObject in objectsToDisable)
        {
            thisObject.SetActive(false);
            yield return null;
        }
        // The disable portion of the scene transition is now finished.
        disabledFinished = true;
    }
}
