//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tatrumchildbehavior : MonoBehaviour
{
    public ChildState childState;
    private UserPrefs userPrefs;
    Animator animator;
    AudioSource audioSource;

    //need these to make player look at the teacher or the child look at the player some times during simulation
    private GameObject playerObject;
    private GameObject playerOriginalPosition;
    private bool playerNeedToLookAtTeacher;
    private bool playerNeedToLookAtChild;

    //need these to make the teacher react to the child behaviors
    private GameObject teacher;
    public NPCAnimationController teacherAnimController;

    private int tantrumLevel;  // simplified version of tantrumCoefficient only has 6 levels
    
    string move; // to hold the child behavior(cry, stamp, walk etc...) at any moment                     
    private float breakTimer; // timer for 9s calm between big emotion behaviors
    #region Audio Clips
    public AudioClip startVoice;
    public AudioClip startVoiceSpanish;
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
    #endregion

    #region static varibles
    public static bool childIsTalking = false; // used in other scripts to check if the child is showing bad behaviors
    public static bool simluationOnGoing; //used in other scripts to check if the simulation is ongoing
    public static bool negativeStatementSelected;  //used in other scripts. when a negative statement is selected, this flag is true and
                                                   // the child keep showing bad behaviors with out calm break
    public static bool isPlayingXylophone; //used in other scripts to check if the child is playing xylophone
    public static bool approachBehavior; //if the child just walk closer to the room. this flag is set to true and the praise for approach button showed
    #endregion

    #region variables related to walk and where to walk
    public List<GameObject> spots;
    public GameObject specialSpot;
    private GameObject nextSpot;
    private GameObject prevSpot;
    private bool isWalkingOrRunning;
    private bool walkedOrRan;
    private bool isIdle;
    #endregion

    public XylophoneManager xylophoneManager; // play the music

    //private TextMeshProUGUI DebugLabel;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        playerObject = GameObject.Find("PlayerObject");
        playerOriginalPosition = GameObject.Find("PlayerOriginPosition");
        teacher = GameObject.Find("TeacherDoctor");
        //DebugLabel = GameObject.Find("DebugLabel").GetComponent<TextMeshProUGUI>();

        tantrumLevel = 0;
        transform.position = spots[0].transform.position;
        nextSpot = spots[0];
        isWalkingOrRunning = false;
        walkedOrRan = false;
        simluationOnGoing = false;
        isIdle = true;
        playerNeedToLookAtChild = false;
        playerNeedToLookAtTeacher = false;
        isPlayingXylophone = false;
        approachBehavior = false;

        if (userPrefs.IsEnglishSpeaker())
            PlayAudioClip(startVoice);
        else
            PlayAudioClip(startVoiceSpanish);

        StartCoroutine(setMoveatStartVoice());
        StartCoroutine(setStartTantrum());
        
    }


    void Update()
    {

        //DebugLabel.text = "";

        if (playerNeedToLookAtChild)
            playerLookAtChild();
        else if (playerNeedToLookAtTeacher)
            playerLookAtTeacher();

        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);

        //if the child stop crying. start counting the time. find nextSpot and add walk to move if possible
        if (!audioSource.isPlaying) // && !negativeStatementSelected
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
                }
            }
            else if (tantrumLevel == 0 && !isWalkingOrRunning) {
                findNextSpot();
                StartCoroutine(addWalkOrRuntoMove("walk"));
            }
        }

        childIsTalking = audioSource.isPlaying; // this is a static variable to use in other script

        // if the time reach 9 seconds or if negative statments just got selected, the child got emotional again, teacher show sad behavior
        if (!audioSource.isPlaying && breakTimer > 9f && !isWalkingOrRunning || !audioSource.isPlaying && negativeStatementSelected && !isWalkingOrRunning)
        {
            UpdateMovement();
            walkedOrRan = false; // reset walkedOrRan
            breakTimer = 0f; //reset
            isIdle = false;
            teacherAnimController.setMove("sad");
            negativeStatementSelected = false; // reset
        }
        //if the child is calm then choose a specific calm behaviors. change the behavior of the teacher as well
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
                if (teacherAnimController.getMove() != "walk")
                    teacherAnimController.setMove("idleThumbsUp");
                else {
                    teacherAnimController.setMove("idleThumbsUp");
                    StartCoroutine(resumeTeacherWalking());
                }
                isIdle = true;
            }
        }

        //if the child is calm at 0 anxiety level and is at the table then play the xylophone
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

        //play animation depend on the move
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

            //unused
            /*else if (move == ("shorttalk") && !animator.IsInTransition(0))
            {
                //animator.Play("shorttalk");
                animator.CrossFade("shorttalk", 0.05f);
            }
            else if (move == ("firstTalk") && !animator.IsInTransition(0))
            {
                animator.CrossFade("firstTalk", 0.05f);
            }*/

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

    // assign a walking or running behavior to move, change the behavior of the teacher
    IEnumerator addWalkOrRuntoMove(string type)
    {

        isWalkingOrRunning = true;
        move = "SadIdle"; // switch tantrum animation to sad idle animation first because the kid will be idle for a second before he start to walk
        yield return new WaitForSeconds(1f);
        isIdle = false;

        if (type == "walk")
        {
            if (teacherAnimController.getMove() != "walk")
                teacherAnimController.setMove("happy");
            else
            {
                teacherAnimController.setMove("happy");
                StartCoroutine(resumeTeacherWalking()); // in case the kid start walking while the teacher is walking,
                                                    // the teacher will stop and show happy for a moment then continue finish walking
            }
            approachBehavior = true;
            move = "sadwalk";
        }
        else if (type == "run")
        {
            move = "sadrun";
            approachBehavior = false;
            teacherAnimController.setMove("sad");
        }
        else if (type == "walkBackward")
        {
            teacherAnimController.setMove("happy");
            approachBehavior = true;
            move = "walkBackward";
        }
       
            
    }

    //change the position of the child to appear like walking or running
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

    // find the nextSpot to walk or run to
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

    // update an emotional behavior depend on anxiety level
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
        playerNeedToLookAtChild = false;
        isIdle = false;
        teacherAnimController.setMove("sad");
    }

    void playerLookAtTeacher() {
        //Debug.Log("adult look at female NPC ###########");
        Quaternion lookRotation = Quaternion.LookRotation((teacher.transform.position - playerObject.transform.position).normalized);
        playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, lookRotation, Time.deltaTime);

    }

    void playerLookAtChild() {
        //Debug.Log("adult look at child ###########");
        Quaternion lookRotation = Quaternion.LookRotation((transform.position - playerObject.transform.position).normalized);
        playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, lookRotation, Time.deltaTime);
    }

    IEnumerator setMoveatStartVoice()
    {
        

        if (userPrefs.IsEnglishSpeaker())
        {
            teacherAnimController.setMove("idle2");
            playerNeedToLookAtChild = true;
            move = "SadIdle";  // I dont want you to leave me ...
            yield return new WaitForSeconds(8f); 
            move = "sad2";  // it is time for drop off ...
            yield return new WaitForSeconds(2f);
            playerNeedToLookAtChild = false;
            playerNeedToLookAtTeacher = true;
            teacherAnimController.setMove("idle");
            move = "SadIdle"; // we are practicing ...
            yield return new WaitForSeconds(11f);
            playerNeedToLookAtTeacher = false;
            playerNeedToLookAtChild = true;
            teacherAnimController.setMove("idle2");
            move ="afraidTalk"; // can you stay with me ... 
            yield return new WaitForSeconds(3.5f); 
            move =  "afraid";
        }
        else
        {
            teacherAnimController.setMove("idle2");
            playerNeedToLookAtChild = true;
            move = "SadIdle";  // I dont want you to leave me ...
            yield return new WaitForSeconds(3f);
            move = "sad2";  // it is time for drop off ...
            yield return new WaitForSeconds(4f);
            playerNeedToLookAtChild = false;
            playerNeedToLookAtTeacher = true;
            teacherAnimController.setMove("idle");
            move = "SadIdle"; // we are practicing ...
            yield return new WaitForSeconds(12.4f);
            playerNeedToLookAtTeacher = false;
            playerNeedToLookAtChild = true;
            teacherAnimController.setMove("idle2");
            move = "afraidTalk"; // can you stay with me ... 
            yield return new WaitForSeconds(2.3f);
            move = "afraid";
        }

    }

    IEnumerator cryAfter(float seconds, string nextMove) {
        yield return new WaitForSeconds(seconds);
        move = nextMove;
    }

    IEnumerator resumeTeacherWalking() {
        yield return new WaitForSeconds(1.5f);
        teacherAnimController.setMove("walk");
    }
}
