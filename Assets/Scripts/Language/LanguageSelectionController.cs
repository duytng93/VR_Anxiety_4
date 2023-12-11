using UnityEngine;
using UnityEngine.UI;

// <summary>
// Controls the language selection UI.
// </summary>
public class LanguageSelectionController : MonoBehaviour
{
    private UserPrefs userPrefs;
    private StartSceneLanguageUpdater startSceneLanguageUpdater;
    private Enums.Languages newLanguage;

    public void Start()
    {
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        startSceneLanguageUpdater = GameObject.Find("StartSceneLanguageUpdater").GetComponent<StartSceneLanguageUpdater>();      
    }

    public void ToggleLanguage()
    {
        newLanguage = userPrefs.IsEnglishSpeaker() ? Enums.Languages.Spanish : Enums.Languages.English;
        userPrefs.SetLanguage(newLanguage);
        startSceneLanguageUpdater.UpdateStartScene(userPrefs.IsEnglishSpeaker());
    }
}

