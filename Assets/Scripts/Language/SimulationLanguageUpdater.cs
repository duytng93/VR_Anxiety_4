
using TMPro;
using UnityEngine;

// <summary>
// Update simulation elements to user's language.
// </summary>
public class SimulationLanguageUpdater : MonoBehaviour
{
    private UserPrefs userPrefs;
    private TMPro.TextMeshProUGUI speakButtonsHeaderText;
    private GameObject[] attentionLevelLabels, tantrumLevelLabels;
    public TMPro.TextMeshProUGUI warningMessage;
    public TMPro.TextMeshProUGUI resetButtonText;
    public TextMeshProUGUI[] breathBoxDescriptions;
    public GameObject[] breathImages;
    public GameObject[] breathImagesSpanish;
    public void Start()
    {
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        attentionLevelLabels = GameObject.FindGameObjectsWithTag("AttentionLevelLabelTag");
        tantrumLevelLabels = GameObject.FindGameObjectsWithTag("TantrumLevelLabelTag");
        speakButtonsHeaderText = GameObject.Find("SpeakButtonsHeaderText").GetComponent<TMPro.TextMeshProUGUI>();
        
    }

    public void UpdateSimulationText()
    {
        speakButtonsHeaderText.SetText(userPrefs.IsEnglishSpeaker() ? "Speak to Child" : "Hablar con el Niño");
        foreach (GameObject label in attentionLevelLabels)
        {
            label.GetComponent<TextMeshProUGUI>().SetText(userPrefs.IsEnglishSpeaker() ? "Attention Level" : "Nivel de Atención");
        }
        foreach (GameObject label in tantrumLevelLabels)
        {
            label.GetComponent<TextMeshProUGUI>().SetText(userPrefs.IsEnglishSpeaker() ? "Big Emotion Level" : "Gran Nivel de Emoción");
        }
        warningMessage.SetText(userPrefs.IsEnglishSpeaker() ? "Click again to reset!" : "Haga clic nuevamente para restablecer");
        resetButtonText.SetText(userPrefs.IsEnglishSpeaker() ? "Reset Simulation!" : "Restablecer Simulación");

        foreach (TextMeshProUGUI breathBoxText in breathBoxDescriptions)
        {
            breathBoxText.SetText(userPrefs.IsEnglishSpeaker() ? "Taking a deep breath and not focusing on negative behaviors could help reduce the child's Big Emotion Level." 
                : "Respirar profundamente y no enfocarse en los comportamientos negativos podría ayudar a reducir el nivel de gran emoción del niño.");

        }
        if (userPrefs.IsEnglishSpeaker())
        {
            foreach (GameObject breathImageSpanish in breathImagesSpanish)
            {
                breathImageSpanish.SetActive(false);
            }
            foreach (GameObject breathImage in breathImages)
            {
                breathImage.SetActive(true);
            }
        }
        else {
            foreach (GameObject breathImageSpanish in breathImagesSpanish)
            {
                breathImageSpanish.SetActive(true);
            }
            foreach (GameObject breathImage in breathImages)
            {
                breathImage.SetActive(false);
            }
        }
        
    }

    

}