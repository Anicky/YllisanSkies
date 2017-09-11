using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using RaverSoft.YllisanSkies;

[InitializeOnLoad]
public class SceneAutoLoader : MonoBehaviour
{
    // Properties are remembered as editor preferences.
    private const string editorPrefIsMasterLoadedOnPlay = "SceneAutoLoader.LoadMasterOnPlay";
    private const string editorPrefMasterScene = "SceneAutoLoader.MasterScene";
    private const string editorPrefPreviousScene = "SceneAutoLoader.PreviousScene";

    private static bool isMasterLoadedOnPlay
    {
        get { return EditorPrefs.GetBool(editorPrefIsMasterLoadedOnPlay, false); }
        set { EditorPrefs.SetBool(editorPrefIsMasterLoadedOnPlay, value); }
    }

    private static string masterScene
    {
        get { return EditorPrefs.GetString(editorPrefMasterScene, "Master.unity"); }
        set { EditorPrefs.SetString(editorPrefMasterScene, value); }
    }

    private static string previousScene
    {
        get { return EditorPrefs.GetString(editorPrefPreviousScene, EditorSceneManager.GetActiveScene().path); }
        set { EditorPrefs.SetString(editorPrefPreviousScene, value); }
    }

    // Static constructor binds a playmode-changed callback.
    // [InitializeOnLoad] above makes sure this gets executed.
    static SceneAutoLoader()
    {
        EditorApplication.playmodeStateChanged += OnPlayModeChanged;
    }

    // Menu items to select the "master" scene and control whether or not to load it.
    [MenuItem("File/Scene Autoload/Select Master Scene...")]
    private static void SelectMasterScene()
    {
        string masterScene = EditorUtility.OpenFilePanel("Select Master Scene", Application.dataPath, "unity");
        if (!string.IsNullOrEmpty(masterScene))
        {
            SceneAutoLoader.masterScene = masterScene;
            isMasterLoadedOnPlay = true;
        }
    }

    [MenuItem("File/Scene Autoload/Load Master On Play", true)]
    private static bool ShowLoadMasterOnPlay()
    {
        return !isMasterLoadedOnPlay;
    }
    [MenuItem("File/Scene Autoload/Load Master On Play")]
    private static void EnableLoadMasterOnPlay()
    {
        isMasterLoadedOnPlay = true;
    }

    [MenuItem("File/Scene Autoload/Don't Load Master On Play", true)]
    private static bool ShowDontLoadMasterOnPlay()
    {
        return isMasterLoadedOnPlay;
    }
    [MenuItem("File/Scene Autoload/Don't Load Master On Play")]
    private static void DisableLoadMasterOnPlay()
    {
        isMasterLoadedOnPlay = false;
    }

    // Play mode change callback handles the scene load/reload.
    private static void OnPlayModeChanged()
    {
        if (!isMasterLoadedOnPlay)
        {
            return;
        }

        if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // User pressed play -- autoload master scene.
            previousScene = EditorSceneManager.GetActiveScene().path;
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                if (!EditorSceneManager.OpenScene(masterScene).IsValid())
                {
                    Debug.LogError(string.Format("error: scene not found: {0}", masterScene));
                    EditorApplication.isPlaying = false;
                }
            }
            else
            {
                // User cancelled the save operation -- cancel play as well.
                EditorApplication.isPlaying = false;
            }
        }

        // isPlaying check required because cannot OpenScene while playing
        if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // User pressed stop -- reload previous scene.
            if (!EditorSceneManager.OpenScene(previousScene).IsValid())
            {
                Debug.LogError(string.Format("error: scene not found: {0}", previousScene));
            }
        }

        if (EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            GameObject.Find("Game").GetComponent<Game>().setTestStartingMap(previousScene);
        }
    }
}