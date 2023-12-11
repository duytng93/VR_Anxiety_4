using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartingCoutnDownController : MonoBehaviour
{
    public TextMeshProUGUI startGameText;
    private UserPrefs userPrefs;
    public AudioClip StartVoiceEnglish;
    public AudioClip StartVoiceSpanish;
    private float introLength;
    private float timeLapse;
    private int countDown;
    private void Awake()
    {
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        startGameText.text = userPrefs.IsEnglishSpeaker() ? "Get Ready!!" : "¡¡Prepararse!!";
        introLength = userPrefs.IsEnglishSpeaker() ? StartVoiceEnglish.length : StartVoiceSpanish.length;
        countDown = (int)introLength;
        timeLapse = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (countDown != 0)
        {
            timeLapse += Time.deltaTime;
            if (timeLapse >= 1)
            {
                countDown = countDown - 1;
                timeLapse = 0;
            }
            if (countDown < 8)
            {
                startGameText.text = userPrefs.IsEnglishSpeaker() ? "Get Ready!! Simulation starts in: " + countDown : "¡¡Prepararse!! La simulación comienza en: " + countDown;
            }
        }
    }
}
