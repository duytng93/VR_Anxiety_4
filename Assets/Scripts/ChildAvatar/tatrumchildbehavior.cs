//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class tatrumchildbehavior : MonoBehaviour
{
    public ChildState childState;
    //public float tantrumLevelInV2;
    private UserPrefs userPrefs;
    #region Objects
    Animator animator;
    AudioSource audioSource;            // main audio
    //Rigidbody rigidBody;
    private GameObject playerObject;
    private GameObject playerOriginalPosition;
    #endregion

    #region Tantrum
    //public static int tantrumCoefficient;// matches tantrum coefficient from GazeAnalysis
    public static int tantrumLevel;     // simplified version of tantrumCoefficient only has 6 levels
    
    #endregion

    #region Movement
    string move;                      
    #endregion


    #region Audio Clips
    public AudioClip startVoice;
    public AudioClip startVoiceSpanish;
    /*public AudioClip level2Cry1;
    public AudioClip level2Cry2;
    public AudioClip level2Cry3;
    public AudioClip level3Cry1;
    public AudioClip level3Cry2;
    public AudioClip level3Cry3;
    public AudioClip level4Cry1;
    public AudioClip level4Cry2;
    public AudioClip level4Cry3;*/
    public AudioClip level5Cry1;
    public AudioClip level5Cry2;
    public AudioClip level5Cry3;

    public AudioClip level4DontleavemeCry;
    public AudioClip level4IdontwannagoCry;
    public AudioClip level4ImreallyscaredCry;
    public AudioClip level4PleasedontmakemedothisCry;
    public AudioClip level4StaywithmeCry;

    public AudioClip spanish_level4DontleavemeCry;
    public AudioClip spanish_level4IdontwannagoCry;
    public AudioClip spanish_level4ImreallyscaredCry;
    public AudioClip spanish_level4PleasedontmakemedothisCry;
    public AudioClip spanish_level4StaywithmeCry;

    public AudioClip level3DontleavemeCry;
    public AudioClip level3IdontwannagoCry;
    public AudioClip level3ImreallyscaredCry;
    public AudioClip level3StaywithmeCry;

    public AudioClip spanish_level3DontleavemeCry;
    public AudioClip spanish_level3IdontwannagoCry;
    public AudioClip spanish_level3ImreallyscaredCry;
    public AudioClip spanish_level3StaywithmeCry;

    public AudioClip level2DontleavemeCry;
    public AudioClip level2IdontwannagoCry;
    public AudioClip level2ImreallyscaredCry;

    public AudioClip spanish_level2DontleavemeCry;
    public AudioClip spanish_level2IdontwannagoCry;
    public AudioClip spanish_level2ImreallyscaredCry;

    public AudioClip level1Dontleaveme;

    public AudioClip spanish_level1Dontleaveme;

    /*public AudioClip Butwhy;
    public AudioClip Ireallywanttoplay;
    public AudioClip AllIwanttodoisplaywiththetoys;
    public AudioClip Youaretheworst;
    public AudioClip Idontwanttobehereanymore;
    public AudioClip IfIcantplayIamleaving;
    public AudioClip Youbettergivemethetoysorelse;

    public AudioClip spanish_butwhy;
    public AudioClip spanish_ireallywanttoplay;
    public AudioClip spanish_allIwanttodoisplaywiththetoys;
    public AudioClip spanish_youaretheworst;
    public AudioClip spanish_idontwanttobehereanymore;
    public AudioClip spanish_ificantplayiamleaving;
    public AudioClip spanish_youbettergivethetoyorelse;*/

    #endregion

    private bool isWalkingOrRunning;
    public static bool childIsTalking = false;
    private float breakTimer;
    public static bool simluationOnGoing;
    public static bool negativeStatementSelected;
    public List<GameObject> spots;
    public GameObject specialSpot;
    private GameObject nextSpot;
    private GameObject prevSpot;
    private GameObject npc_female;
    private bool walkedOrRan;
    private bool isIdle;
    public static bool approachBehavior;
    private bool adultNeedToLookAtNPC;
    private bool adultNeedToLookAtChild;
    public XylophoneManager xylophoneManager;
    public static bool isPlayingXylophone;
    public NPCAnimationController npcAnimationController;

    //private TextMeshProUGUI DebugLabel;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //rigidBody = GetComponent<Rigidbody>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        playerObject = GameObject.Find("PlayerObject");
        playerOriginalPosition = GameObject.Find("PlayerOriginPosition");
        npc_female = GameObject.Find("NPC_Female");
        //npc_animator = npc_female.GetComponent<Animator>();
        //DebugLabel = GameObject.Find("DebugLabel").GetComponent<TextMeshProUGUI>();

        tantrumLevel = 0;
        transform.position = spots[0].transform.position;
        nextSpot = spots[0];
        isWalkingOrRunning = false;
        walkedOrRan = false;
        if (userPrefs.IsEnglishSpeaker())
            PlayAudioClip(startVoice);
        else
            PlayAudioClip(startVoiceSpanish);

        StartCoroutine(setMoveatStartVoice());
        StartCoroutine(setStartTantrum());
        simluationOnGoing = false;
        //gameLost = false;
        isIdle = true;
        adultNeedToLookAtChild = false;
        adultNeedToLookAtNPC = false;
        isPlayingXylophone = false;
        //npcIsIdle = true;
        //npcHappy = true;
    }

    private void Awake()
    {
        approachBehavior = false;
    }


    void Update()
    {

        //DebugLabel.text = " move is : " + move + " Anxiety level is: " + tantrumLevel;
        if (adultNeedToLookAtChild)
            adultLookAtChild();
        else if (adultNeedToLookAtNPC)
            adultLookAtFemaleNPC();

        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);

        //if the child stop saying. start counting the time
        if (!audioSource.isPlaying && !negativeStatementSelected)
        {
            breakTimer += Time.deltaTime;
            if (!isWalkingOrRunning && !walkedOrRan && tantrumLevel != 0)
            {
                findNextSpot();
                if (Vector3.Distance(nextSpot.transform.position, spots[0].transform.position) > Vector3.Distance(prevSpot.transform.position, spots[0].transform.position))
                {
                    if (nextSpot == spots[1] && playerObject.transform.position == playerOriginalPosition.transform.position)
                        StartCoroutine(addWalkOrRuntoMove("walkBackward")); // walk backward to spot 1 from spot 0
                    else
                        StartCoroutine(addWalkOrRuntoMove("walk")); // walk to the center of the room
                    
                }
                else if (Vector3.Distance(nextSpot.transform.position, spots[0].transform.position) < Vector3.Distance(prevSpot.transform.position, spots[0].transform.position))
                {
                    StartCoroutine(addWalkOrRuntoMove("run")); // run to the door
                    //approachBehavior = false;
                }
            }
            else if (tantrumLevel == 0 && !isWalkingOrRunning) {
                findNextSpot();
                StartCoroutine(addWalkOrRuntoMove("walk"));
            }
        }

        childIsTalking = audioSource.isPlaying; // this is a static variable to use in other script

        // if the time reach 9 seconds. the child talk again
        if (!audioSource.isPlaying && breakTimer > 9f && !isWalkingOrRunning || !audioSource.isPlaying && negativeStatementSelected && !isWalkingOrRunning)
        {
            
            UpdateMovement();
            walkedOrRan = false; // reset walkedOrRan
            breakTimer = 0f; //reset
            isIdle = false;
            npcAnimationController.setMove("sad");
            negativeStatementSelected = false; // reset
            //approachBehavior = false;
        }
        else if (!audioSource.isPlaying && !isWalkingOrRunning)
        {
            if (!isIdle)
            {
                switch (tantrumLevel) {
                    case 1:
                        move = "SadIdle";
                        break;
                    case 2:
                        move = "embarrased";
                        break;
                    case 3:
                        move = "afraid";
                        break;
                    case 4:
                        move = "idleDeepBreath";
                        break;
                    case 5:
                        move = "tired";
                        break;

                }
                if (npcAnimationController.getMove() != "walk")
                    npcAnimationController.setMove("idleThumbsUp");
                else {
                    npcAnimationController.setMove("idleThumbsUp");
                    StartCoroutine(resumeNPCWalking());
                }
                isIdle = true;
            }
        }

        if (!audioSource.isPlaying && childState.tantrumLevel == 0 && transform.position == spots[4].transform.position && !isPlayingXylophone)
        {
            // Direction from the child to the special spot
            Vector3 toSpecialSpot = (specialSpot.transform.position - transform.position).normalized;
            // Direction the child is facing
            Vector3 childDirection = transform.forward;

            if (Vector3.Angle(toSpecialSpot, childDirection) < 3) {
                move = "";
                transform.Find("Mallot 1").gameObject.SetActive(true);
                transform.Find("Mallot 2").gameObject.SetActive(true);
                StartCoroutine(xylophoneManager.PlayMaryHadALittleLamb());
                isPlayingXylophone = true;
            }
                
            else {
                move = "idle";
                Quaternion lookRotation = Quaternion.LookRotation((specialSpot.transform.position - transform.position).normalized);
                //over time
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
            }
        }

        if (move != "") {
            if (move == ("SadIdle") && !animator.IsInTransition(0))
            {
                animator.CrossFade("sad", 0.1f);
                //animator.Play("sad");

            }

            else if (move == ("embarrased") && !animator.IsInTransition(0))
            {
                animator.CrossFade("idleEmbarrased", 0.1f);
                //animator.Play("sad");

            }

            else if (move == ("afraid") && !animator.IsInTransition(0))
            {
                animator.CrossFade("idleAfraid", 0.1f);
                //animator.Play("sad");

            }

            else if (move == ("idleDeepBreath") && !animator.IsInTransition(0))
            {
                animator.CrossFade("idleDeepBreath", 0.1f);
                //animator.Play("sad");

            }

            else if (move == ("tired") && !animator.IsInTransition(0))
            {
                animator.CrossFade("Idletired", 0.1f);
                //animator.Play("sad");

            }

            //
            else if (move == ("sad2") && !animator.IsInTransition(0))
            {
                //animator.Play("sad2");
                animator.CrossFade("sad2", 0.1f);
            }

            //
            else if (move == ("idle") && !animator.IsInTransition(0))
            {
                //animator.Play("idle");
                animator.CrossFade("idle", 0.1f);
            }

            //
            else if (move == ("cry1") && !animator.IsInTransition(0))
            {
                //animator.Play("cry1");
                animator.CrossFade("cry1", 0.1f);

            }

            //
            else if (move == ("cry2") && !animator.IsInTransition(0))
            {
                //animator.Play("cry2");
                animator.CrossFade("cry2", 0.1f);
            }

            //
            else if (move == ("stamp") && !animator.IsInTransition(0))
            {
                animator.CrossFade("stamp", 0.1f);
                //animator.Play("stamp");
            }

            //
            else if (move == ("telloff") && !animator.IsInTransition(0))
            {
                //animator.Play("telloff");
                animator.CrossFade("telloff", 0.05f);
            }

            //
            else if (move == ("shorttalk") && !animator.IsInTransition(0))
            {
                //animator.Play("shorttalk");
                animator.CrossFade("shorttalk", 0.05f);
            }

            //
            else if (move == ("firstTalk") && !animator.IsInTransition(0))
            {
                animator.CrossFade("firstTalk", 0.05f);
            }
            //animator.Play("firstTalk");

            else if (move == "sadwalk")
            {
                animator.Play("sadWalk");
                walkOrRun("walk");
            }

            else if (move == "sadrun")
            {
                animator.Play("sadRun");
                walkOrRun("run");
            }

            else if (move == "sitdown" && !animator.IsInTransition(0))
            {
                animator.SetTrigger("sitdown");
            }

            else if (move == "afraidTalk" && !animator.IsInTransition(0))
            {
                animator.CrossFade("afraidTalk", 0.05f);
                Quaternion lookRotation3 = Quaternion.LookRotation((playerObject.transform.position - transform.position).normalized);
                //over time
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation3, 1);
            }

            else if (move == "walkBackward")
            {
                animator.Play("walkBackward");
                walkOrRun("walkBackward");
            }
        }
        
    }

    IEnumerator addWalkOrRuntoMove(string type)
    {

        isWalkingOrRunning = true;
        move = "SadIdle"; // switch tantrum animation to sad idle animation first because the kid will be idle for a second before he start to walk
        yield return new WaitForSeconds(1f);
        isIdle = false;

        if (type == "walk")
        {
            if (npcAnimationController.getMove() != "walk")
                npcAnimationController.setMove("happy");
            else
            {
                npcAnimationController.setMove("happy");
                StartCoroutine(resumeNPCWalking());
            }
            approachBehavior = true;
            move = "sadwalk";
        }
        else if (type == "run")
        {
            move = "sadrun";
            approachBehavior = false;
            npcAnimationController.setMove("sad");
        }
        else if (type == "walkBackward")
        {
            npcAnimationController.setMove("happy");
            approachBehavior = true;
            move = "walkBackward";
        }
       
            
    }

    void walkOrRun(string type)
    {
        Vector3 direction = nextSpot.transform.position - transform.position;

        if (direction != Vector3.zero && type != "walkBackward")
        {
           transform.rotation = Quaternion.LookRotation(direction);
        }


        if (transform.position != nextSpot.transform.position) {
            if(type == "walk" || type == "walkBackward")
                transform.position = Vector3.MoveTowards(transform.position, nextSpot.transform.position, 0.8f * Time.deltaTime);
            else transform.position = Vector3.MoveTowards(transform.position, nextSpot.transform.position, 1.3f * Time.deltaTime);
        }
            
        else
        {
            isWalkingOrRunning = false;
            walkedOrRan = true;
        }
    }

    void findNextSpot() {
        prevSpot = nextSpot;
        float currentTantrumLevel = childState.tantrumLevel;
        if (currentTantrumLevel > 60)
            nextSpot = spots[0];
        else if (currentTantrumLevel > 40)
            nextSpot = spots[1];
        else if (currentTantrumLevel > 20)
            nextSpot = spots[2];
        else if(currentTantrumLevel > 0)
            nextSpot = spots[3];
        else 
            nextSpot = spots[4];
    }


    void UpdateMovement()
    {
        //Debug.Log("################update Movement called");

        if (tantrumLevel == 0)
        {
            move = "idle";
        }
        else if (tantrumLevel == 1)
        {
            AudioClip clip;
            move = "afraid";
            clip = userPrefs.IsEnglishSpeaker() ? level1Dontleaveme : spanish_level1Dontleaveme;
            PlayAudioClip(clip);
        }
        else if (tantrumLevel == 2)
        {
            int moveType = Random.Range(0, 3);
            AudioClip clip;
            switch (moveType)
            {
                case 0:
                    move = "afraid";
                    clip = userPrefs.IsEnglishSpeaker() ? level2DontleavemeCry : spanish_level2DontleavemeCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.9f, "cry1"));
                    else StartCoroutine(cryAfter(1.5f, "cry1"));
                    break;
                case 1:
                    move = "afraid";
                    clip = userPrefs.IsEnglishSpeaker() ? level2IdontwannagoCry : spanish_level2IdontwannagoCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.8f, "cry1"));
                    else StartCoroutine(cryAfter(1.3f, "cry1"));
                    break;
                case 2:
                    move = "afraid";
                    clip = userPrefs.IsEnglishSpeaker() ? level2ImreallyscaredCry : spanish_level2ImreallyscaredCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.7f, "cry1"));
                    else StartCoroutine(cryAfter(1.3f, "cry1"));
                    break;
            }
        }
        else if (tantrumLevel == 3)
        {
            int moveType = Random.Range(0, 4);
            AudioClip clip;
            switch (moveType)
            {
                case 0:  
                    move = "afraidTalk";
                    clip = userPrefs.IsEnglishSpeaker() ? level3DontleavemeCry : spanish_level3DontleavemeCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.9f, "stamp"));
                    else StartCoroutine(cryAfter(1.5f, "stamp"));
                    break;
                case 1:
                    move = "afraidTalk";
                    clip = userPrefs.IsEnglishSpeaker() ? level3IdontwannagoCry : spanish_level3IdontwannagoCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.8f, "stamp"));
                    else StartCoroutine(cryAfter(1.3f, "stamp"));
                    break;
                case 2:
                    move = "afraidTalk";
                    clip = userPrefs.IsEnglishSpeaker() ? level3ImreallyscaredCry : spanish_level3ImreallyscaredCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.7f, "cry2"));
                    else StartCoroutine(cryAfter(1.3f, "cry2"));
                    break;
                case 3:
                    move = "afraidTalk";
                    clip = userPrefs.IsEnglishSpeaker() ? level3StaywithmeCry : spanish_level3StaywithmeCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.6f, "cry2"));
                    else StartCoroutine(cryAfter(1.1f, "cry2"));
                    break;

            }

        }
        else if (tantrumLevel == 4)
        {

            int moveType = Random.Range(0, 5);
            AudioClip clip;
            switch (moveType)
            {
                case 0:  //"WandC"
                    move = "afraidTalk";
                    clip = userPrefs.IsEnglishSpeaker() ? level4DontleavemeCry : spanish_level4DontleavemeCry;
                    PlayAudioClip(clip);
                    if(userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.9f, "cry2"));
                    else StartCoroutine(cryAfter(1.5f, "cry2"));
                    break;
                case 1:
                    move = "afraidTalk";
                    clip = userPrefs.IsEnglishSpeaker() ? level4IdontwannagoCry : spanish_level4IdontwannagoCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.8f, "cry2"));
                    else StartCoroutine(cryAfter(1.3f, "cry2"));
                    break;
                case 2:
                    move = "afraidTalk";
                    clip = userPrefs.IsEnglishSpeaker() ? level4ImreallyscaredCry : spanish_level4ImreallyscaredCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.7f, "stamp"));
                    else StartCoroutine(cryAfter(1.3f, "stamp"));
                    break;
                case 3:
                    move = "afraidTalk";
                    clip = userPrefs.IsEnglishSpeaker() ? level4PleasedontmakemedothisCry : spanish_level4PleasedontmakemedothisCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(2.2f, "stamp"));
                    else StartCoroutine(cryAfter(1.8f, "stamp"));
                    break;
                case 4:
                    move = "afraidTalk";
                    clip = userPrefs.IsEnglishSpeaker() ? level4StaywithmeCry : spanish_level4StaywithmeCry;
                    PlayAudioClip(clip);
                    if (userPrefs.IsEnglishSpeaker())
                        StartCoroutine(cryAfter(1.6f, "stamp"));
                    else StartCoroutine(cryAfter(1.1f, "stamp"));
                    break;
           
            }
        }
        else if (tantrumLevel == 5)
        {
            int moveType = Random.Range(0, 3);
            switch (moveType)
            {
                case 0:
                    move = "cry2";
                    PlayAudioClip(level5Cry3);
                    break;
                case 1:
                    move = "stamp";
                    PlayAudioClip(level5Cry2);
                    break;
                case 2:
                    move = "stamp";
                    PlayAudioClip(level5Cry1);
                    break;

            }

        }

    }



    void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


    IEnumerator setStartTantrum()
    {
        if (userPrefs.IsEnglishSpeaker())
            yield return new WaitForSeconds(startVoice.length);
        else
            yield return new WaitForSeconds(startVoiceSpanish.length);
        childState.setStartTantrumLevel(80);
        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);
        UpdateMovement(); // first tantrum
        simluationOnGoing = true;
        adultNeedToLookAtChild = false;
        isIdle = false;
        npcAnimationController.setMove("sad");
    }

    void adultLookAtFemaleNPC() {
        //Debug.Log("adult look at female NPC ###########");
        Quaternion lookRotation = Quaternion.LookRotation((npc_female.transform.position - playerObject.transform.position).normalized);
        playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, lookRotation, Time.deltaTime);

    }

    void adultLookAtChild() {
        //Debug.Log("adult look at child ###########");
        Quaternion lookRotation = Quaternion.LookRotation((transform.position - playerObject.transform.position).normalized);
        playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, lookRotation, Time.deltaTime);
    }

    IEnumerator setMoveatStartVoice()
    {
        

        if (userPrefs.IsEnglishSpeaker())
        {
            //npc_animator.Play("idle2");
            npcAnimationController.setMove("idle2");
            adultNeedToLookAtChild = true;
            move = "SadIdle";  // I dont want you to leave me ...
            yield return new WaitForSeconds(8f); 
            move = "sad2";  // it is time for drop off ...
            yield return new WaitForSeconds(2f);
            adultNeedToLookAtChild = false;
            adultNeedToLookAtNPC = true;
            //if (!npc_animator.IsInTransition(0))
            //    npc_animator.CrossFade("idle", 0.5f);
            npcAnimationController.setMove("idle");
            move = "SadIdle"; // we are practicing ...
            yield return new WaitForSeconds(11f);
            adultNeedToLookAtNPC = false;
            adultNeedToLookAtChild = true;
            //if (!npc_animator.IsInTransition(0))
            //    npc_animator.CrossFade("idle2", 0.5f);
            npcAnimationController.setMove("idle2");
            move ="afraidTalk"; // can you stay with me ... 
            yield return new WaitForSeconds(3.5f); 
            move =  "afraid";
        }
        else
        {
            //npc_animator.Play("idle2");
            npcAnimationController.setMove("idle2");
            adultNeedToLookAtChild = true;
            move = "SadIdle";  // I dont want you to leave me ...
            yield return new WaitForSeconds(3f);
            move = "sad2";  // it is time for drop off ...
            yield return new WaitForSeconds(4f);
            adultNeedToLookAtChild = false;
            adultNeedToLookAtNPC = true;
            //if (!npc_animator.IsInTransition(0))
            //    npc_animator.CrossFade("idle", 0.5f);
            npcAnimationController.setMove("idle");
            move = "SadIdle"; // we are practicing ...
            yield return new WaitForSeconds(12.4f);
            adultNeedToLookAtNPC = false;
            adultNeedToLookAtChild = true;
            //if (!npc_animator.IsInTransition(0))
            //    npc_animator.CrossFade("idle2", 0.5f);
            npcAnimationController.setMove("idle2");
            move = "afraidTalk"; // can you stay with me ... 
            yield return new WaitForSeconds(2.3f);
            move = "afraid";
        }

    }

    IEnumerator cryAfter(float seconds, string nextMove) {
        yield return new WaitForSeconds(seconds);
        move = nextMove;
    }

    IEnumerator resumeNPCWalking() {
        yield return new WaitForSeconds(1.5f);
        npcAnimationController.setMove("walk");
    }
}
