using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class button1behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip Thankyouforstayingcalm;
    public AudioClip Thankyoufortryingtoquietdown;
    public AudioClip Thankyoufortakingdeepbreath;
    public AudioClip Thankyoufortryingtocalmdown;
    public AudioClip Thankyouforunderstandingthatitistimetowalkintotheroom;

    public AudioClip Graciasporquedartecalmado;
    public AudioClip Graciasporintentarcalmarse;
    public AudioClip Permanezcatranquilo;
    public AudioClip Graciaspormantenertusbrazosypiesatuslados;
    public AudioClip Graciasporintentarrespiraprofundo;
    public AudioClip Spanish_Thankyouforunderstandingthatitistimetowalkintotheroom;


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
    public static bool adultIsSpeaking;
    void Start()
    {
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        tmpText = yourButton.GetComponentInChildren<TextMeshProUGUI>();
        
        if (tmpText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the button's children.");
        }

        

        switch (userPrefs.GetChildAvatar()) {
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

        if (atStart && introTimer < introLength || adultIsSpeaking || !tatrumchildbehavior.simluationOnGoing)  // if we at start the child is saying the greeting -> disable the button
        {                                                                        // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
            UpdateTextAndAudioClip(tantrumLevel, true);
            //tmpText.text = "";
        }
        else if (!adultIsSpeaking)
        { // let the parent finish talking first, otherwise their audio is cut off during tantrum level change
            UpdateTextAndAudioClip(tantrumLevel,false);
            yourButton.interactable = true;
            atStart = false;
        }

    }

    void UpdateTextAndAudioClip(int tantrumLevel, bool textOnly)
    {
        {
            switch (tantrumLevel)
            {
                case 0:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for staying calm" : "Gracias por quedarte calmado";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyouforstayingcalm : Graciasporquedartecalmado;
                    amount = -10.0f;
                    break;
                case 1:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for trying to quiet down" : "Gracias por intentar calmarse";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyoufortryingtoquietdown : Graciasporintentarcalmarse;
                    amount = -10.0f;
                    break;
                case 2:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for trying to quiet down" : "Gracias por intentar calmarse";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyoufortryingtoquietdown : Graciasporintentarcalmarse;
                    amount = -10.0f;
                    break;
                case 3:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for trying to take deep breath" : "Gracias por intentar respira profundo";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyoufortakingdeepbreath : Graciasporintentarrespiraprofundo;
                    amount = -10.0f;
                    break;
                case 4:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for trying to calm down" : "Gracias por intentar calmarse";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyoufortryingtocalmdown : Graciasporintentarcalmarse;
                    amount = -10.0f;
                    break;
                case 5:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for understanding that it is time to walk into the room" : "Gracias por entender que es hora de entrar en la habitación";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyouforunderstandingthatitistimetowalkintotheroom : Spanish_Thankyouforunderstandingthatitistimetowalkintotheroom;
                    amount = -10.0f;
                    break;
                default:
                    tmpText.text = "";
                    if (!textOnly)
                        audioSource.clip = null;
                    amount = 0.0f;
                    break;
            }
        }
    }

    void OnButtonClick()
    {

        adultIsSpeaking = true;
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
            adultIsSpeaking = false;
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
