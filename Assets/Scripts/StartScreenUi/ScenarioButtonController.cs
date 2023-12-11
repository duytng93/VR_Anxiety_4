using UnityEngine;

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

    public void ToggleBackgroundScene() {
        captionText = scenarioDropdown.captionText;
        selection = captionText.text;

        if (!userPrefs.IsEnglishSpeaker())
        {
            if (captionText.text == "Escuela")
            {
                selection = "School";
            }
            else if (captionText.text == "Oficina de médico")
            {
                selection = "Medical Office";
            }
        }
        
        switch (selection) {
            case "School":
                sceneController.ToggleBackgroundScene(0);
                break;
            case "Medical Office":
                sceneController.ToggleBackgroundScene(1);
                break;
        }

    }
}
