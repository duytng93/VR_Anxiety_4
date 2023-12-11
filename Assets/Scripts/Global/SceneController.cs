using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The SceneController class is responsible for managing the loading and unloading of scenes.
/// </summary>
public class SceneController : MonoBehaviour
{
    private UserPrefs userPrefs;

    void Start()
    {
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
    }

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
        // Get the current scene
        // Enums.Scenarios currentScenario = (Enums.Scenarios)index;
        // userPrefs.SetScenario(currentScenario);

        switch(index) {
            case 1: // Enums.Scenarios.Home:
                UnloadScene(Enums.SceneNames.DoctorScene);
                UnloadScene(Enums.SceneNames.SchoolScene);
                LoadScene(Enums.SceneNames.HomeScene);
                //SceneManager.UnloadSceneAsync(Enums.SceneNames.DoctorScene.ToString());
                //SceneManager.UnloadSceneAsync(Enums.SceneNames.SchoolScene.ToString());
                //SceneManager.LoadScene(Enums.SceneNames.HomeScene.ToString(), LoadSceneMode.Additive);
                break;

            case 0: // Enums.Scenarios.School:
                UnloadScene(Enums.SceneNames.HomeScene);
                UnloadScene(Enums.SceneNames.DoctorScene);
                LoadScene(Enums.SceneNames.SchoolScene);
                //SceneManager.UnloadSceneAsync(Enums.SceneNames.HomeScene.ToString());
                //SceneManager.UnloadSceneAsync(Enums.SceneNames.DoctorScene.ToString());
                //SceneManager.LoadScene(Enums.SceneNames.SchoolScene.ToString(), LoadSceneMode.Additive);
                break;

            case 2: // Enums.Scenarios.Doctor:
                UnloadScene(Enums.SceneNames.HomeScene);
                UnloadScene(Enums.SceneNames.SchoolScene);
                LoadScene(Enums.SceneNames.DoctorScene);
                //SceneManager.UnloadSceneAsync(Enums.SceneNames.HomeScene.ToString());
                // SceneManager.UnloadSceneAsync(Enums.SceneNames.SchoolScene.ToString());
                //SceneManager.LoadScene(Enums.SceneNames.DoctorScene.ToString(), LoadSceneMode.Additive);
                break;

            default:
                Debug.Log("Invalid option selected");
                break;
        }
    }
}
