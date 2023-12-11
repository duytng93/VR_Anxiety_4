using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The SceneController class is responsible for managing the loading and unloading of scenes.
/// </summary>
public class SceneController : MonoBehaviour
{
    void Start()
    {}

    public void LoadScene(Enums.SceneNames scene)
    {
        if (!SceneManager.GetSceneByName(scene.ToString()).isLoaded)
        {
            Debug.Log("Loading scene: " + scene.ToString());
            SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Additive);
        }
        else
        {
            Debug.Log("Scene already loaded: " + scene.ToString());
        }
    }

    public void UnloadScene(Enums.SceneNames scene)
    {
        if (SceneManager.GetSceneByName(scene.ToString()).isLoaded)
        {
            Debug.Log("Unloading scene: " + scene.ToString());
            SceneManager.UnloadSceneAsync(scene.ToString());
        }
        else
        {
            Debug.Log("Scene already unloaded: " + scene.ToString());
        }
    }

    public void ToggleBackgroundScene(int index) {

        switch(index) {
            /*case 1: // Enums.Scenarios.Home:
                UnloadScene(Enums.SceneNames.DoctorScene);
                UnloadScene(Enums.SceneNames.SchoolScene);
                LoadScene(Enums.SceneNames.HomeScene);
                break;*/

            case 0: // Enums.Scenarios.School:
                UnloadScene(Enums.SceneNames.HomeScene);
                UnloadScene(Enums.SceneNames.DoctorScene);
                LoadScene(Enums.SceneNames.SchoolScene);
                break;

            case 1: // Enums.Scenarios.Doctor:
                UnloadScene(Enums.SceneNames.HomeScene);
                UnloadScene(Enums.SceneNames.SchoolScene);
                LoadScene(Enums.SceneNames.DoctorScene);
                break;

            default:
                Debug.Log("Invalid option selected");
                break;
        }
    }
}
