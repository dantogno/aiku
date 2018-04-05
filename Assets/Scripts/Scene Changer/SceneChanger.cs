using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script goes on an object that can be interacted with. It switches 
/// </summary>
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
    // Data structre for keeping track of whether objects are disabled or enabled in a certain scene.
    private static Dictionary<string, Dictionary<GameObject, bool>> objectList;
    // There are a few things that should only be initialized if the current scene is the first scene.
    private static bool firstScene = false;
    // Check to see if a level load is already in progress before attempting to load a new one.
    private static bool loading = false;

    // Fields to keep track of whether the scenes have finished loading and unloading.
    private bool disabledFinished = false;
    private bool enabledFinished = false;
    private bool readyToChooseSurvivors = false;    // DW: Dictates what happens when player interacts with the screen.

    private void Awake()
    {
        // Only initialize the static stuff once.
        if(loadedScenes == null)
        {
            objectList = new Dictionary<string, Dictionary<GameObject, bool>>();
            loadedScenes = new List<Scene>();
            firstScene = true;
        }
    }

    private void OnEnable()
    {
        // DW:
        EndingScreen.DoneWithLevels += SwitchInteractionModeToFinalChoice;
    }
    private void OnDisable()
    {
        // DW:
        EndingScreen.DoneWithLevels -= SwitchInteractionModeToFinalChoice;
    }

    private void Start()
    {
        // Make sure not to add the scene at start unless it's the first scene.
        if (firstScene == true)
        {
            // DW:
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

    /// <summary>
    /// Saves information about whether each gameobject was enabled or disabled in the scene
    /// before they are all disabled for the scene change.
    /// </summary>
    /// <param name="scene"></param>
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

    /// <summary>
    /// Load information about whether object were disabled or enabled when the
    /// scene was initially turned off (so that their state is preserved).
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
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
        loading = false;
        // Disable this object last.
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
        // DW:
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
        // DW:
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
        // Load the previous state of gameobjects from the object list.
        Dictionary<GameObject, bool> objectsToEnable = LoadFromObjectList(sceneToEnable);
        foreach (KeyValuePair<GameObject, bool> entry in objectsToEnable)
        {
            if(entry.Key != null)
            {
                if (entry.Value == true)
                {
                    if(entry.Key.tag == "Player")
                    {
                        // Placeholder fix for a bug where the player would sometimes fall through the floor.
                        entry.Key.transform.position = new Vector3(entry.Key.transform.position.x, entry.Key.transform.position.y + 1, entry.Key.transform.position.z);
                    }
                    entry.Key.SetActive(true);
                }
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
        // Update the state of objects in the scene before disabling them.
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

    /// <summary>
    /// Update the state of every game object (whether they are enabled
    /// or disabled) before disabling the scene.
    /// </summary>
    /// <param name="sceneToUpdate"></param>
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
