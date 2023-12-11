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
    //public AudioClip Youarecryingbecauseyouarefeelinganxious;
    public AudioClip Youareupsetbecauseyouarefeelingaxious;
    public AudioClip Youarestandingclosetothedoorbecauseyouare;
    //public AudioClip Youaresayingyoudontwanttowalkintotheroom;
    public AudioClip Youdontwantmetoleavebecauseyouarefeelingscared;
    public AudioClip Iunderstandyouarefeelingscared;
    public AudioClip Igetthatitfeelsscarytoseparate;


    public AudioClip Enestemomentoparecesestartranquilo;
    //public AudioClip spanish_Youarecryingbecauseyouarefeelinganxious;
    public AudioClip spanish_Youareupsetbecauseyouarefeelingaxious;
    public AudioClip spanish_Youarestandingclosetothedoorbecauseyouare;
    //public AudioClip spanish_Youaresayingyoudontwanttowalkintotheroom;
    public AudioClip spanish_Youdontwantmetoleavebecauseyouarefeelingscared;
    public AudioClip spanish_Iunderstandyouarefeelingscared;
    public AudioClip spanish_Igetthatitfeelsscarytoseparate;
    public AudioClip StartVoice;
    public AudioClip StartVoiceSpanish;

    private ChildState childState;
    public TextMeshProUGUI tmpText;

    private float tantrumLevelInV2;
    private int tantrumLevel;
    private float amount;
    private bool atStart;
    private float introTimer;
    private float introLength;
    void Start()
    {
        tmpText = yourButton.GetComponentInChildren<TextMeshProUGUI>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();

        if (tmpText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the button's children.");
        }

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

        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);
        UpdateTextAndAudioClip(tantrumLevel, false); // Initial setup of text and audio clip

        yourButton.onClick.AddListener(OnButtonClick);
        introTimer = 0f;
        introLength = userPrefs.IsEnglishSpeaker() ? StartVoice.length : StartVoiceSpanish.length;
        atStart = true;
    }

    void Update()
    {
        if (atStart && tatrumchildbehavior.childIsTalking)
            introTimer += Time.deltaTime;
        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);

        if (atStart && introTimer < introLength || button1behavior.adultIsSpeaking || !tatrumchildbehavior.simluationOnGoing)  // if we at start the child is saying the greeting -> disable the button
        {                                                                        // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
            UpdateTextAndAudioClip(tantrumLevel, true);
            //tmpText.text = "";
        }
        else if (!button1behavior.adultIsSpeaking)
        { // let the parent finish talking first, otherwise their audio is cut off during tantrum level change

            UpdateTextAndAudioClip(tantrumLevel, false);
            yourButton.interactable = true;
            atStart = false;
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
            /*if (!tatrumchildbehavior.childIsTalking)
            {
                childState.ChangeTantrumLevel(amount);
            }*/
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned.");
        }
        
    }

}
