using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour, IInteractable
{
    public event Action<string> ChoseACrewmember;   // DW: Called after all levels have been completed and player chooses a survivor.

    public static List<Scene> LoadedScenes { get { return loadedScenes; } }     // DW: Accessed from GameManager.cs

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
    private static bool firstScene = false;
    private static bool loading = false;

    // Fields to keep track of whether the scenes have finished loading and unloading.
    private bool disabledFinished = false;
    private bool enabledFinished = false;
    private bool readyToChooseSurvivors = false;    // DW: Dictates what happens when player interacts with the screen.

    private void Awake()
    {
        if(loadedScenes == null)
        {
            objectList = new Dictionary<string, Dictionary<GameObject, bool>>();
            loadedScenes = new List<Scene>();
            firstScene = true;
        }
    }

    private void OnEnable()
    {
        EndingScreen.DoneWithLevels += SwitchInteractionModeToFinalChoice;
    }
    private void OnDisable()
    {
        EndingScreen.DoneWithLevels -= SwitchInteractionModeToFinalChoice;
    }

    // Make sure not to load the static List more than once.
    private void Start()
    {
        if (firstScene == true)
        {
            EndingScreen.levelsEntered++;

            // The initial scene needs to be added to the list.
            Scene currentScene = gameObject.scene;
            loadedScenes.Add(currentScene);
            AddToObjectList(currentScene);
            firstScene = false;
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
        loading = false;
        transform.root.gameObject.SetActive(false);
    }

    /// <summary>
    /// When the player interacts with this object, it should eiter Load a new scene
    /// or enable an existing one.
    /// OR, should be the final decision of who to save!    // DW
    /// </summary>
    /// <param name="agentInteracting">The player interacting with the scene changing object</param>
    public void Interact(GameObject agentInteracting)
    {
        // DW
        if (agentInteracting.GetComponent<PowerableObject>() != null)
        {
            agentInteracting.GetComponent<PowerableObject>().PowerOff();
        }

        // DW: Added conditional checking if the player has finished the levels and is ready to choose survivors.
        if(!loading && !readyToChooseSurvivors)
        {
            loading = true;
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
        else if (readyToChooseSurvivors)
        {
            ChooseCrewmember();
        }
    }

    /// <summary>
    /// Loads a scene if it has never been loaded before.
    /// </summary>
    /// <param name="sceneName">The name of the new scene to load</param>
    private void LoadNewScene(string sceneName)
    {
        EndingScreen.levelsEntered++;

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
        while (operation.isDone == false)
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
                    if(entry.Key.tag == "Player")
                    {
                        entry.Key.transform.position = new Vector3(entry.Key.transform.position.x, entry.Key.transform.position.y + 1, entry.Key.transform.position.z);
                    }
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
            if (thisObject != transform.root.gameObject)
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

    /// <summary>
    /// DW:
    /// After the player has played through all the levels,
    /// their final act is to pick the two crewmembers who will live.
    /// </summary>
    private void SwitchInteractionModeToFinalChoice()
    {
        readyToChooseSurvivors = true;

        if (GetComponentInParent<GlitchValueGenerator>() != null)
            GetComponentInParent<GlitchValueGenerator>().enabled = false;   // We don't want the vfx getting in the way of this important philosophical moment.
    }

    /// <summary>
    /// DW:
    /// The name of the scene tells GameManager.cs which crewmember the player has chosen.
    /// </summary>
    private void ChooseCrewmember()
    {
        switch (sceneToLoad)
        {
            case "DanielScene":
                ChoseACrewmember.Invoke("Norma");
                break;
            case "TrevorLevelGDC":
                ChoseACrewmember.Invoke("Trevor");
                break;
            case "RaySceneGDC":
                ChoseACrewmember.Invoke("Ray");
                break;
            default:
                break;
        }
    }
}
