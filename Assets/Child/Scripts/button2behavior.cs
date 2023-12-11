using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class button2behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip Amazingjobtakingastepcloser;
    public AudioClip Ilovehowyouarebeingbraveandwalkingaway;
    public AudioClip Goodjobcalmingdownandwalkingtowardsthetable;
    public AudioClip Greatjobpracticingbeingbravebycalmingdown;
    public AudioClip Ilikehowyouarecalmlywalkingcloser;
    public AudioClip Ilovehowbraveyouarebeingbywalkingmoreintotheroom;

    public AudioClip spanish_Amazingjobtakingastepcloser;
    public AudioClip spanish_Ilovehowyouarebeingbraveandwalkingaway;
    public AudioClip spanish_Goodjobcalmingdownandwalkingtowardsthetable;
    public AudioClip spanish_Greatjobpracticingbeingbravebycalmingdown;
    public AudioClip spanish_Ilikehowyouarecalmlywalkingcloser;
    public AudioClip spanish_Ilovehowbraveyouarebeingbywalkingmoreintotheroom;


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
    private float showingTimer;
    //private TextMeshProUGUI DebugLabel;
    public Button button3, button1, button4, button5, button6;
    void Start()
    {
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        tmpText = yourButton.GetComponentInChildren<TextMeshProUGUI>();
        //DebugLabel = GameObject.Find("DebugLabel").GetComponent<TextMeshProUGUI>();
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
        UpdateTextAndAudioClip(tantrumLevel); // Initial setup of text and audio clip
        yourButton.onClick.AddListener(OnButtonClick);
        introTimer = 0f;
        introLength = userPrefs.IsEnglishSpeaker() ? StartVoice.length : StartVoiceSpanish.length;
        atStart = true;
    }

    void Update()
    {
        //DebugLabel.text = "button 3 y: " + button3.transform.localPosition.y;
        if (atStart && tatrumchildbehavior.childIsTalking)
            introTimer += Time.deltaTime;

        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);

        if (!tatrumchildbehavior.approachBehavior)
            showingTimer = 0f;
        else showingTimer += Time.deltaTime;
        
        

        if (atStart && introTimer < introLength || button1behavior.adultIsSpeaking || !tatrumchildbehavior.simluationOnGoing)  // if we at start the child is saying the greeting -> disable the button
        {                                                                        // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
            tmpText.text = "";

            moveButtonsUp();
        }
        else if (!button1behavior.adultIsSpeaking)
        { // let the parent finish talking first, otherwise their audio is cut off during tantrum level change
            UpdateTextAndAudioClip(tantrumLevel);
            if (tatrumchildbehavior.approachBehavior && showingTimer < 10f)
            {
                yourButton.interactable = true;
                moveButtonsDown();
            }
            else
            {
                yourButton.interactable = false;
                tmpText.text = "";
                moveButtonsUp();
                tatrumchildbehavior.approachBehavior = false;
            }
                
            atStart = false;
        }

    }


    void UpdateTextAndAudioClip(int tantrumLevel)
    {
        {
            switch (tantrumLevel)
            {
                case 0:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "I love how brave you are being by walking more into the room" : "Me encanta lo valiente que estás siendo con entrando en la habitación";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Ilovehowbraveyouarebeingbywalkingmoreintotheroom : spanish_Ilovehowbraveyouarebeingbywalkingmoreintotheroom;
                    amount = -10.0f;
                    break;
                case 1:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "I like how you are calmly walking closer" : "Me gusta cómo te acercas con calma";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Ilikehowyouarecalmlywalkingcloser : spanish_Ilikehowyouarecalmlywalkingcloser;
                    amount = -10.0f;
                    break;
                case 2:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Great job practicing being brave by calming down" : "Gran trabajo practicando ser valiente con calmándose";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Greatjobpracticingbeingbravebycalmingdown : spanish_Greatjobpracticingbeingbravebycalmingdown;
                    amount = -10.0f;
                    break;
                case 3:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Good job calming down and walking towards the table" : "Buen trabajo calmándose y caminando hacia la mesa!";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Goodjobcalmingdownandwalkingtowardsthetable : spanish_Goodjobcalmingdownandwalkingtowardsthetable;
                    amount = -10.0f;
                    break;
                case 4:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "I love how you are being brave and walking away" : "Me encanta cómo estás siendo valiente y alejándote";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Ilovehowyouarebeingbraveandwalkingaway : spanish_Ilovehowyouarebeingbraveandwalkingaway;
                    amount = -10.0f;
                    break;
                case 5:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Amazing job taking a step closer!" : "Muy bien acercándose un paso más a la mesa!";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Amazingjobtakingastepcloser : spanish_Amazingjobtakingastepcloser;
                    amount = -10.0f;
                    break;
                default:
                    tmpText.text = "";
                    audioSource.clip = null;
                    amount = 0.0f;
                    break;
            }
        }
    }

    void OnButtonClick()
    {
        button1behavior.adultIsSpeaking = true;
        yourButton.interactable = false;
        tatrumchildbehavior.approachBehavior = false;
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
    IEnumerator enableButtonAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        yourButton.interactable = true;
    }

    void moveButtonsUp() {
        if (button3.transform.localPosition.y <= 138)
            button3.transform.localPosition += new Vector3(0, 1, 0) * Time.deltaTime * 100;
        if(button6.transform.localPosition.y <= 68)
            button6.transform.localPosition += new Vector3(0, 1, 0) * Time.deltaTime * 100;
        if (button5.transform.localPosition.y <= 68)
            button5.transform.localPosition += new Vector3(0, 1, 0) * Time.deltaTime * 100;
        if (button4.transform.localPosition.y <= 68)
            button4.transform.localPosition += new Vector3(0, 1, 0) * Time.deltaTime * 100;
        if (button1.transform.localPosition.y <= -66)
            button1.transform.localPosition += new Vector3(0, 1, 0) * Time.deltaTime * 100;
    }

    void moveButtonsDown() {
        if (button3.transform.localPosition.y >=74)
            button3.transform.localPosition += new Vector3(0, -1, 0) * Time.deltaTime * 100;
        if (button6.transform.localPosition.y >=5)
            button6.transform.localPosition += new Vector3(0, -1, 0) * Time.deltaTime * 100;
        if (button5.transform.localPosition.y >=5)
            button5.transform.localPosition += new Vector3(0, -1, 0) * Time.deltaTime * 100;
        if (button4.transform.localPosition.y >=5)
            button4.transform.localPosition += new Vector3(0, -1, 0) * Time.deltaTime * 100;
        if (button1.transform.localPosition.y >=-130)
            button1.transform.localPosition += new Vector3(0, -1, 0) * Time.deltaTime * 100;
    }
}
