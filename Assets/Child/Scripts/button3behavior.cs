using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.InputManagerEntry;

public class button3behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip Iamgoingtostartwalkingintheroom;
    public AudioClip Imreadytopracticebeingbrave;
    public AudioClip Iseetheresafunactivityatthetable;
    public AudioClip Iamtakingdeepbreathtohelpmestaycalm;
    public AudioClip Iamhelpingmyselfstaycalmbycountingto10;
    public AudioClip Iamtakingadeepbreathandgoingtoname5things;


    public AudioClip spanish_Iamgoingtostartwalkingintheroom;
    public AudioClip spanish_Imreadytopracticebeingbrave;
    public AudioClip spanish_Iseetheresafunactivityatthetable;
    public AudioClip spanish_Iamtakingdeepbreathtohelpmestaycalm;
    public AudioClip spanish_Iamhelpingmyselfstaycalmbycountingto10;
    public AudioClip spanish_Iamtakingadeepbreathandgoingtoname5things;

    public GameObject[] Spots;

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

    private bool parentIsWalking;
    private GameObject nextSpot;
    private GameObject PlayerObject;
    private GameObject ChildObject;
    public NPCAnimationController nPCAnimationController;
    private bool spot1Visited,spot2Visited;
    void Start()
    {
        tmpText = yourButton.GetComponentInChildren<TextMeshProUGUI>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        PlayerObject = GameObject.Find("PlayerObject");


        if (tmpText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the button's children.");
        }

        switch (userPrefs.GetChildAvatar())
        {
            case Enums.ChildAvatars.Hispanic:
                ChildObject = GameObject.Find("TKGirlA");
                childState = ChildObject.GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.Black:
                //childState = GameObject.Find("TKGirlB").GetComponent<ChildController>().childState;
                ChildObject = GameObject.Find("TKGirlB");
                childState = ChildObject.GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.White:
                ChildObject = GameObject.Find("TKGirlC");
                childState = ChildObject.GetComponent<ChildController>().childState;
                //childState = GameObject.Find("TKGirlC").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.Asian:
                ChildObject = GameObject.Find("TKGirlD");
                childState = ChildObject.GetComponent<ChildController>().childState;
                //childState = GameObject.Find("TKGirlD").GetComponent<ChildController>().childState;
                break;
        }

        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);
        UpdateTextAndAudioClip(tantrumLevel, false); // Initial setup of text and audio clip

        yourButton.onClick.AddListener(OnButtonClick);
        introTimer = 0f;
        introLength = userPrefs.IsEnglishSpeaker() ? StartVoice.length : StartVoiceSpanish.length;
        atStart = true;
        parentIsWalking = false;
        spot1Visited = false;
        spot2Visited = false;
    }

    void Update()
    {
        if (parentIsWalking) {
            parentLookAtTheChild();
            parentWalk();
        }

        if (atStart && tatrumchildbehavior.childIsTalking)
            introTimer += Time.deltaTime;
        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);

        if (atStart && introTimer < introLength || button1behavior.adultIsSpeaking || !tatrumchildbehavior.simluationOnGoing) 
            //|| (PlayerObject.transform.position == Spots[1].transform.position && tantrumLevel == 2) 
           // || (PlayerObject.transform.position == Spots[2].transform.position && tantrumLevel == 1)
            //|| (PlayerObject.transform.position == Spots[2].transform.position && tantrumLevel == 2))  
        {                                                                        // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
            UpdateTextAndAudioClip(tantrumLevel, true);
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
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I am going to start walking in the room" : "Voy a empezar a entrar en la habitación";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iamgoingtostartwalkingintheroom : spanish_Iamgoingtostartwalkingintheroom;
                amount = -5.0f;
                break;
            case 1:
                if (!spot2Visited)
                {
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "I’m ready to practice being brave, I am going to walk to the table and would love for you to join me" : "Estoy lista para practicar ser valiente, voy a caminar a la mesa y me encantaría que vengas conmigo";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Imreadytopracticebeingbrave : spanish_Imreadytopracticebeingbrave;
                }
                else {
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "I am taking  deep breaths to help me stay calm" : "Estoy respirando profundamente para ayudarme a mantener la calma";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iamtakingdeepbreathtohelpmestaycalm : spanish_Iamtakingdeepbreathtohelpmestaycalm;
                }
                amount = -5.0f;
                break;
            case 2:
                if (!spot1Visited)
                {
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "I see there’s a fun activity at the table I am going to walk closer to get a better look" : "Veo que hay una actividad divertida en la mesa, voy a acercarme para verla mejor";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iseetheresafunactivityatthetable : spanish_Iseetheresafunactivityatthetable;
                }
                else {
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "I am taking  deep breaths to help me stay calm" : "Estoy respirando profundamente para ayudarme a mantener la calma";
                    if (!textOnly)
                        audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iamtakingdeepbreathtohelpmestaycalm : spanish_Iamtakingdeepbreathtohelpmestaycalm;
                }
                amount = -5.0f;
                break;
            case 3:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I am taking  deep breaths to help me stay calm" : "Estoy respirando profundamente para ayudarme a mantener la calma";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iamtakingdeepbreathtohelpmestaycalm : spanish_Iamtakingdeepbreathtohelpmestaycalm;
                amount = -5.0f;
                break;
            case 4:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I am helping myself stay calm by counting to 10 in my head" : "Me estoy ayudando a mantener la calma contando hasta 10 en mi cabeza";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iamhelpingmyselfstaycalmbycountingto10 : spanish_Iamhelpingmyselfstaycalmbycountingto10;
                amount = -5.0f;
                break; ;
            case 5:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I am taking a deep breath and going to name 5 things I see in this room to help me stay calm and present" : "Estoy respirando profundamente y voy a nombrar 5 cosas que veo en esta habitación para ayudarme a mantener la calma y estar presente";
                if (!textOnly)
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iamtakingadeepbreathandgoingtoname5things : spanish_Iamtakingadeepbreathandgoingtoname5things;
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
        if (tantrumLevel == 2 && !spot1Visited)
        {
            nextSpot = Spots[1];
            parentIsWalking = true;
            nPCAnimationController.setNextSpot(1);
            nPCAnimationController.setMove("walk");
            spot1Visited = true;
        }
        else if (tantrumLevel == 1 && !spot2Visited) {

            nextSpot = Spots[2];
            parentIsWalking = true;
            nPCAnimationController.setNextSpot(2);
            nPCAnimationController.setMove("walk");
            spot2Visited = true;
        }
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

    void parentLookAtTheChild() {
        Quaternion lookRotation2 = Quaternion.LookRotation((ChildObject.transform.position - PlayerObject.transform.position).normalized);
        PlayerObject.transform.rotation = Quaternion.Slerp(PlayerObject.transform.rotation, lookRotation2, Time.deltaTime);
    }

    void parentWalk() {
        if (PlayerObject.transform.position != nextSpot.transform.position)
        {
            PlayerObject.transform.position = Vector3.MoveTowards(PlayerObject.transform.position, nextSpot.transform.position, 0.8f * Time.deltaTime);
        }
        else
        {
            parentIsWalking = false;
        }
    }

}
