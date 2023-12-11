using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The SceneController class is responsible for managing the loading and unloading of scenes.
/// </summary>
public class ScenarioButtonController : MonoBehaviour
{
    private UserPrefs userPrefs;
    private TMPro.TMP_Dropdown scenarioDropdown;
    private TMPro.TMP_Text captionText;
    private string selection;
    private SceneController sceneController;
    void Start()
    {
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        scenarioDropdown = GameObject.Find("ScenarioDropdown").GetComponent<TMPro.TMP_Dropdown>();
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

    public void ToggleBackgroundScene() {
        string previousScene = "";
        captionText = scenarioDropdown.captionText;
        selection = captionText.text;

        if (!userPrefs.IsEnglishSpeaker())
        {
            if (captionText.text == "Casa")
            {
                selection = "Home";
            }
            else if (captionText.text == "Escuela")
            {
                selection = "School";
            }
            else if (captionText.text == "Oficina de médico")
            {
                selection = "Medical Office";
            }
        }
        /*else if (selection == "Medical Office")
        {
            selection = "Doctor";
        }*/


        /*if (SceneManager.GetSceneByName("HomeScene").isLoaded) {
            previousScene = "HomeScene";
        }
        else if (SceneManager.GetSceneByName("SchoolScene").isLoaded) {
            previousScene = "SchoolScene";
        }
        else if (SceneManager.GetSceneByName("DoctorScene").isLoaded) {
            previousScene = "DoctorScene";
        }*/

        //SceneManager.UnloadScene(previousScene);
        //SceneManager.LoadScene(selection + "Scene", LoadSceneMode.Additive);
        switch (selection) {
            case "Home":
                sceneController.ToggleBackgroundScene(1);
                break;
            case "School":
                sceneController.ToggleBackgroundScene(0);
                break;
            case "Medical Office":
                sceneController.ToggleBackgroundScene(2);
                break;
        }

    }
}
