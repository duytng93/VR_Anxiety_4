using UnityEngine;

public class resetButtonAtEnd : MonoBehaviour
{
    private SceneController sceneController;
    private UserPrefs userPrefs;
    private GameObject PlayerObject;
    private GameObject PlayerOriginPosition;
    // Start is called before the first frame update
    void Start()
    {
        // Find the SceneController and UserPrefs objects and get their scripts
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        PlayerObject = GameObject.Find("PlayerObject");
        PlayerOriginPosition = GameObject.Find("PlayerOriginPosition");
    }

    public void resetSimulation()
    {
            // Load the necessary scenes
            sceneController.UnloadScene(Enums.SceneNames.ChildScene);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneWin);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneLose);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneLoseSpanish);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneWinSpanish);
            sceneController.LoadScene(Enums.SceneNames.StartScene);
            sceneController.ToggleBackgroundScene(0); // load School, unload others


            // Set the defaults
            userPrefs.SetScenario(Enums.Scenarios.School);
            userPrefs.SetChildAvatar(Enums.ChildAvatars.Hispanic);
            userPrefs.SetLanguage(Enums.Languages.English);

        //reset player position in case the player walked into the room
        PlayerObject.transform.position = PlayerOriginPosition.transform.position;
        PlayerObject.transform.rotation = PlayerOriginPosition.transform.rotation;
    }
}
