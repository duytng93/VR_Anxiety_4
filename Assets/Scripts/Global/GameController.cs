using UnityEngine;

/// <summary>
/// The GameController class is responsible for overall game management.
/// </summary>
public class GameController : MonoBehaviour
{
    private SceneController sceneController;
    private UserPrefs userPrefs;
    void Start()
    {
        // Find the SceneController and UserPrefs objects and get their scripts
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();

        // Load the necessary scenes
        sceneController.LoadScene(Enums.SceneNames.StartScene);
        sceneController.LoadScene(Enums.SceneNames.SchoolScene);

        // Set the defaults
        userPrefs.SetLanguage(Enums.Languages.English);
        userPrefs.SetScenario(Enums.Scenarios.School);
        userPrefs.SetChildAvatar(Enums.ChildAvatars.Hispanic);

    }
}
