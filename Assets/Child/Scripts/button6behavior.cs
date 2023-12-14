using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class button6behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;

    public AudioClip Canwedothiswithoutyoucryingthistime;
    public AudioClip Pleasegivemesomepersonalspace;
    public AudioClip youareembarrassingmeinfrontofeveryone;
    public AudioClip youareactinglikeascaredcat;
    public AudioClip youarebeingbeyondridiculous;
    public AudioClip youarestartingtomakemeangry;

    public AudioClip spanish_Canwedothiswithoutyoucryingthistime;
    public AudioClip spanish_Pleasegivemesomepersonalspace;
    public AudioClip spanish_youareembarrassingmeinfrontofeveryone;
    public AudioClip spanish_youareactinglikeascaredcat;
    public AudioClip spanish_youarebeingbeyondridiculous;
    public AudioClip spanish_youarestartingtomakemeangry;


    private ChildState childState;
    public TextMeshProUGUI tmpText;

    private int tantrumLevel;
    private float amount;
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

        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);
        UpdateTextAndAudioClip(tantrumLevel,false); // Initial setup of text and audio clip

        yourButton.onClick.AddListener(OnButtonClick);
    }

    void Update()
    {
        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);
       
        if ( button1behavior.adultIsSpeaking || !tatrumchildbehavior.simluationOnGoing || tantrumLevel == 0) // if the child is saying the greeting -> not show the button
        {                                                       // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
            UpdateTextAndAudioClip(tantrumLevel, true);
        }
        else if(!button1behavior.adultIsSpeaking)
        {
            UpdateTextAndAudioClip(tantrumLevel,false);
            yourButton.interactable = true;
        }
    }

    void UpdateTextAndAudioClip(int tantrumLevel, bool textOnly)
    {
        switch (tantrumLevel)
        {
            case 0:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "Can we do this without you crying this time?" : "Podemos hacer esto sin que llores esta vez?";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Canwedothiswithoutyoucryingthistime : spanish_Canwedothiswithoutyoucryingthistime;
                amount = 10.0f;
                break;
            case 1:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "Please give me some personal space!" : "Por favor, dame un poco de espacio!";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Pleasegivemesomepersonalspace : spanish_Pleasegivemesomepersonalspace;
                amount = 10.0f;
                break;
            case 2:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You’re embarrassing me in front of everyone" : "Me estas haciendo pasar una pena delante de todo mundo";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? youareembarrassingmeinfrontofeveryone : spanish_youareembarrassingmeinfrontofeveryone;
                amount = 10.0f;
                break;
            case 3:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You’re acting like a scaredy cat!" : "Estás actuando como un bebé!";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? youareactinglikeascaredcat : spanish_youareactinglikeascaredcat;
                amount = 10.0f;
                break;
            case 4:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You are being beyond ridiculous!" : "Estás siendo ridícula!";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? youarebeingbeyondridiculous : spanish_youarebeingbeyondridiculous;
                amount = 10.0f;
                break;
            case 5:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You are starting to make me angry!" : "Me estoy enojando contigo!";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? youarestartingtomakemeangry : spanish_youarestartingtomakemeangry;
                amount = 10.0f;
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
