using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Video;

// <summary>
// Update all text in the scene to the current language.
// </summary>
public class StartSceneLanguageUpdater : MonoBehaviour {

    private TMPro.TextMeshProUGUI startUIHeaderText;
    private TMPro.TextMeshProUGUI languageDropdownLabel;
    private TMPro.TextMeshProUGUI childAvatarDropdownLabel;
    private TMPro.TextMeshProUGUI scenarioDropdownLabel;
    private TMPro.TextMeshProUGUI startButtonLabel;
    private TMPro.TMP_Dropdown childAvatarDropdown;
    private TMPro.TMP_Dropdown scenarioDropdown;

    //private VideoPlayer videoPlayer;
    public void Start() {
        startUIHeaderText = GameObject.Find("StartUiHeaderText").GetComponent<TMPro.TextMeshProUGUI>();
        languageDropdownLabel = GameObject.Find("LanguageDropdownLabel").GetComponent<TMPro.TextMeshProUGUI>();
        childAvatarDropdownLabel = GameObject.Find("ChildAvatarLabel").GetComponent<TMPro.TextMeshProUGUI>();
        scenarioDropdownLabel = GameObject.Find("ScenarioDropdownLabel").GetComponent<TMPro.TextMeshProUGUI>();
        startButtonLabel = GameObject.Find("StartButtonLabel").GetComponent<TMPro.TextMeshProUGUI>();
        childAvatarDropdown = GameObject.Find("ChildAvatarDropdown").GetComponent<TMPro.TMP_Dropdown>();
        scenarioDropdown = GameObject.Find("ScenarioDropdown").GetComponent<TMPro.TMP_Dropdown>();
        
    }

    public void UpdateStartScene(bool switchingToEnglish) {
        if (switchingToEnglish)
        {
            startUIHeaderText.text = "Settings";
            languageDropdownLabel.text = "Language";
            childAvatarDropdownLabel.text = "Child";
            scenarioDropdownLabel.text = "Scenario";
            startButtonLabel.text = "Start";
           
            UpdateScenarioDropdownOptions(new string[] { "School", "Medical Office" });
            UpdateChildAvatarDropdownOptions(new string[] {"Hispanic", "Black or African American", "White", "Asian" });
        }
        else
        {
            startUIHeaderText.text = "Configuración";
            languageDropdownLabel.text = "Idioma";
            childAvatarDropdownLabel.text = "Niño";
            scenarioDropdownLabel.text = "Situación";
            startButtonLabel.text = "Empezar";
            
            UpdateScenarioDropdownOptions(new string[] { "Escuela", "Oficina de médico" });
            UpdateChildAvatarDropdownOptions(new string[] { "Hispano", "Negro o Afroamericano", "Blanco", "Asiático" }); // all masculine
            
        }
    }

    private void UpdateScenarioDropdownOptions(string[] newOptions)
    {
        for (int i = 0; i < newOptions.Length; i++)
        {
            scenarioDropdown.options[i].text = newOptions[i];
        }
        
        // Refresh the dropdown to reflect the changes
        scenarioDropdown.RefreshShownValue();
    }

    private void UpdateChildAvatarDropdownOptions(string[] newOptions)
    {
        for (int i = 0; i < newOptions.Length; i++)
        {
            childAvatarDropdown.options[i].text = newOptions[i];
        }

        // Refresh the dropdown to reflect the changes
        childAvatarDropdown.RefreshShownValue();
    }
}
