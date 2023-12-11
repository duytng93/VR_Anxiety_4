using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


// <summary>
// Update all text in the scene to the current language.
// </summary>
public class TutorialSceneLanguageUpdater : MonoBehaviour
{

    private TMPro.TextMeshProUGUI tutorialHeader;
    private TMPro.TextMeshProUGUI continueButton;
    private UserPrefs userPrefs;
    //private VideoPlayer videoPlayer;
    
    public void Start()
    {
        tutorialHeader = GameObject.Find("Header").GetComponent<TMPro.TextMeshProUGUI>();
        continueButton = GameObject.Find("ContinueButton").GetComponent<TMPro.TextMeshProUGUI>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        //videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        updateLanguage();
    }

    public void Update()
    {
        
    }

    public void updateLanguage() {
        if (userPrefs.IsEnglishSpeaker())
        {
            tutorialHeader.text = "Please watch this tutorial to the end and click Continue";
            continueButton.text = "Continue";
            //videoPlayer.clip = Resources.Load<VideoClip>("tutorial_video_english");
        }
        else
        {
            tutorialHeader.text = "Mire este tutorial hasta el final y haga clic en Continuar.";
            continueButton.text = "Continuar";
           // videoPlayer.clip = Resources.Load<VideoClip>("tutorial_video_english");
        }
    }

}
