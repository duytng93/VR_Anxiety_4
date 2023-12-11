using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class button5behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip StartVoice;
    public AudioClip StartVoiceSpanish;
    //public AudioClip Youknowyoucannotplayrightnow;
    //public AudioClip Iwishyouwoulduseyourbigkidvoice;
    //public AudioClip Ohyouaregoingtocrynow;
    //public AudioClip Ineedyoutoquietdown;
    public AudioClip Youneedtoquietdownrightnow;
    public AudioClip Youneedtocalmyourselfdownnow;
    public AudioClip itsreallytimetogonow;
    public AudioClip comeonitstimetoactlikeabigkid;
    public AudioClip youaremakingashow;
    public AudioClip youaremakingeveryonelate;

    //public AudioClip Sabesquenopuedesjugarahoramismo;
    //public AudioClip Ojaláusarastuvozdeniñograndej;
    //public AudioClip Vasallorarahora;
    //public AudioClip Necesitoquetecalmes;
    public AudioClip Tienesquecalmarteahoramismo;
    public AudioClip Necesitascalmarteahora;
    public AudioClip spanish_itsreallytimetogonow;
    public AudioClip spanish_comeonitstimetoactlikeabigkid;
    public AudioClip spanish_youaremakingashow;
    public AudioClip spanish_youaremakingeveryonelate;

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
        UpdateTextAndAudioClip(tantrumLevel,false); // Initial setup of text and audio clip

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
        
        if (atStart && introTimer < introLength || button1behavior.adultIsSpeaking || !tatrumchildbehavior.simluationOnGoing || tantrumLevel == 0)  // if we at start the child is saying the greeting -> disable the button
        {                                                                        // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
            UpdateTextAndAudioClip(tantrumLevel, true);
            //tmpText.text = "";
        }
        else if(!button1behavior.adultIsSpeaking)
        { // let the parent finish talking first, otherwise their audio is cut off during tantrum level change
            UpdateTextAndAudioClip(tantrumLevel,false);
            yourButton.interactable = true;
            atStart = false;
        }
    }

    void UpdateTextAndAudioClip(int tantrumLevel, bool textOnly)
    {
        switch (tantrumLevel)
        {
            case 0:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "It’s really time for you to go now" : "Es hora de que te vayas ahora mismo";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? itsreallytimetogonow : spanish_itsreallytimetogonow;
                amount = 5.0f;
                break;
            case 1:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "Come on, it’s time to act like a big kid, not a baby" : "Vamos, es hora de actuar como un niño grande, no como un bebé";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? comeonitstimetoactlikeabigkid : spanish_comeonitstimetoactlikeabigkid;
                amount = 5.0f;
                break;
            case 2:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You are making a show" : "Estás haciendo un espectáculo";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? youaremakingashow : spanish_youaremakingashow;
                amount = 5.0f;
                break;
            case 3:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You are making everyone late" : "Estás haciendo que todos perdamos el tiempo";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? youaremakingeveryonelate : spanish_youaremakingeveryonelate;
                amount = 5.0f;
                break;
            case 4:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You need to quiet down right now" : "Tienes que calmarte ahora mismo";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youneedtoquietdownrightnow : Tienesquecalmarteahoramismo;
                amount = 5.0f;
                break;
            case 5:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You need to calm yourself down now" : "Necesitas calmarte ahora";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youneedtocalmyourselfdownnow : Necesitascalmarteahora;
                amount = 5.0f;
                break;
            default:
                tmpText.text = "";
                if (!textOnly)
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
            childState.ChangeTantrumLevel(amount);
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            tatrumchildbehavior.negativeStatementSelected = true;
            button1behavior.adultIsSpeaking = false;
            
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned.");
        }

        
    }

    
}
