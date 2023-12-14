using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.InputManagerEntry;

public class button4behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip Youseemcalmrightnow;
    public AudioClip Youareupsetbecauseyouarefeelingaxious;
    public AudioClip Youarestandingclosetothedoorbecauseyouare;
    public AudioClip Youdontwantmetoleavebecauseyouarefeelingscared;
    public AudioClip Iunderstandyouarefeelingscared;
    public AudioClip Igetthatitfeelsscarytoseparate;

    public AudioClip Enestemomentoparecesestartranquilo;
    public AudioClip spanish_Youareupsetbecauseyouarefeelingaxious;
    public AudioClip spanish_Youarestandingclosetothedoorbecauseyouare;
    public AudioClip spanish_Youdontwantmetoleavebecauseyouarefeelingscared;
    public AudioClip spanish_Iunderstandyouarefeelingscared;
    public AudioClip spanish_Igetthatitfeelsscarytoseparate;

    private ChildState childState;
    public TextMeshProUGUI tmpText;

    private int tantrumLevel;
    private float amount;
    void Start()
    {
        tmpText = yourButton.GetComponentInChildren<TextMeshProUGUI>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();

        switch (userPrefs.GetChildAvatar())
        {
            case Enums.ChildAvatars.Hispanic:
                childState = GameObject.Find("TKGirlA").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.Black:
                childState = GameObject.Find("TKGirlB").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.White:
                childState = GameObject.Find("TKGirlC").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.Asian:
                childState = GameObject.Find("TKGirlD").GetComponent<ChildController>().childState;
                break;
        }

        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);
        UpdateTextAndAudioClip(tantrumLevel, false); // Initial setup of text and audio clip

        yourButton.onClick.AddListener(OnButtonClick);
    }

    void Update()
    {
        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);

        if (button1behavior.adultIsSpeaking || !tatrumchildbehavior.simluationOnGoing)  // if player is talking or the simulation is not going yet. disable the button but still update the text
        {                                                                        
            yourButton.interactable = false;
            UpdateTextAndAudioClip(tantrumLevel, true);
        }
        else if (!button1behavior.adultIsSpeaking)
        { //if the player finish talking then enable the button
            UpdateTextAndAudioClip(tantrumLevel, false);
            yourButton.interactable = true;
        }
    }

    void UpdateTextAndAudioClip(int tantrumLevel, bool textOnly)
    {
        
        switch (tantrumLevel)
        {
            case 0:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You seem calm right now" : "En este momento pareces estar tranquilo";
                if(!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youseemcalmrightnow : Enestemomentoparecesestartranquilo;
                amount = -5.0f;
                break;
            case 1:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You are upset because you are feeling anxious about separating" : "Estás molesta porque te sientes ansioso por tener que separarte de mi";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youareupsetbecauseyouarefeelingaxious : spanish_Youareupsetbecauseyouarefeelingaxious;
                amount = -5.0f;
                break;
            case 2:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You are standing close to the door because you are feeling nervous" : "Estás parada cerca de la puerta porque te sientes nerviosa";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youarestandingclosetothedoorbecauseyouare : spanish_Youarestandingclosetothedoorbecauseyouare;
                amount = -5.0f;
                break;
            case 3:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You don’t want me to leave because you are feeling scared" : "No quieres que me vaya porque te sientes con miedo";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youdontwantmetoleavebecauseyouarefeelingscared : spanish_Youdontwantmetoleavebecauseyouarefeelingscared;
                amount = -5.0f;
                break;
            case 4:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I understand you are feeling scared and that’s why you don’t want to walk closer" : "Entiendo que te sientes con miedo y por eso no quieres caminar más cerca";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iunderstandyouarefeelingscared : spanish_Iunderstandyouarefeelingscared;
                amount = -5.0f;
                break; ;
            case 5:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I get that it feels scary to separate" : "Entiendo que tengas miedo de separarse";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Igetthatitfeelsscarytoseparate : spanish_Igetthatitfeelsscarytoseparate;
                amount = -5.0f;
                break;
            default:
                tmpText.text = "";
                audioSource.clip = null;
                amount = 0.0f;
                break;
        }
    }

    void OnButtonClick()
    {
        button1behavior.adultIsSpeaking = true;
        StartCoroutine(PlayAudioAndChangeTantrumLevel());
    }

    System.Collections.IEnumerator PlayAudioAndChangeTantrumLevel()
    {

        if (audioSource.clip != null)
        {
            if (!tatrumchildbehavior.childIsTalking)
                childState.ChangeTantrumLevel(amount);
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            button1behavior.adultIsSpeaking = false;
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned.");
        }
        
    }

}
