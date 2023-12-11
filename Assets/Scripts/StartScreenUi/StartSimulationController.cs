using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// The StartSimulationController class is responsible for managing the start simulation screen.
/// </summary>
public class StartSimulationController : MonoBehaviour
{
    private SceneController sceneController;
    private UserPrefs userPrefs;

    void Start()
    {
        // Find the SceneController and UserPrefs objects and get their scripts
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
    }

    public void StartSimulation() {
        //sceneController.LoadScene(Enums.SceneNames.ChildScene); load child scene after viewing tutorial video
        //SceneManager.LoadScene(Enums.SceneNames.TutorialScene.ToString(), LoadSceneMode.Additive);
        if(userPrefs.IsEnglishSpeaker())
            sceneController.LoadScene(Enums.SceneNames.TutorialScene);
        else
            sceneController.LoadScene(Enums.SceneNames.TutorialSceneSpanish);
        sceneController.UnloadScene(Enums.SceneNames.StartScene);
        
    }
}
