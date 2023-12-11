using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackupTantrumBehavior : MonoBehaviour
{
    public ChildState childState;
    public float tantrumLevelInV2;
    private UserPrefs userPrefs;
    #region Objects
    Animator animator;
    AudioSource audioSource;            // main audio
    public AudioSource walkRunAudioSource;  // walk/run audio
    Rigidbody rigidBody;
    public Camera mainCamera;
    #endregion

    #region Tantrum
    public static int tantrumCoefficient;// matches tantrum coefficient from GazeAnalysis
    public static int tantrumLevel;     // simplified version of tantrumCoefficient only has 6 levels
    float tantrumCoefficientUpdateTimer;// timing when to update tantrum coefficient
    float tantrumCoefficientUpdateTime; // what to reset tantrumCoefficientUpdateTimer to
    #endregion

    #region Movement
    float moveTimer;                    // timing how long the child needs to move around, used to make decisions
    float moveTime;                     // what to reset moveTimer to
    float fallTimer;                    // timing how long it takes to fall, avoid falling until timer is up
    float fallTime;                     // what to set fallTimer to
    float jumpTimer;                    // timing how long it takes to jump, avoid jumping until timer is up
    float jumpTime;                     // what to reset jumpTimer to
    bool isIdle;                        // whether the child is idle
    float jumpForce;                    // how hard child jumps during jump animation
    string[] move;                      /* Set of moves that need to be performed simultaneously:
											A - Turn left
											D - Turn right
											W - Walk forward
											S - Walk backward
											R - Run forward
											J - Jump
											C - Cry
											Idle - Stand still
											Paused - Game is paused
											End - Simulation is over
											Fall1
											Fall2
											FallCry
											Angry2Idle
											Sad2Idle
											Sit_Cry
											HighC
											ToMainCam
										*/
    #endregion

    #region Repeat
    float redoTime;                     // previous duration of moveTime
    int redoProbability;                // probability that last action will be repeated
    string redoAnimation;               // the last animation that was performed
    bool redoFlag;                      // whether character is going to redo their last action
    #endregion

    #region Positioning
    public static bool startPoint;      // whether the child is located at their starting position
    float[] floorMap;                   // unused
    float distToGround;                 // unused
    #endregion

    #region Animation
    bool resetWalk;                     // never tested, only assigned
    bool startAnimation;                // whether the child is doing their starting animation
    bool playFlag;                      // whether the animator is playing an animation
    #endregion

    #region Audio Control
    public bool isAudioPlaying = false;
    AudioClip currentWalkRunAudio;      // currently playing walk/run audio clip
    float voiceTimer;                   // timing how long it takes to speak, avoid running voiceline until timer is up
    bool v1;                            // false if "canIPlayWithYourPhone" voiceline has been used while the child is in their starting position and animation
    bool v2;                            // false if "notRightNowImUsingIt" voiceline has been used while the child is in their starting position and animation
    bool v3;                            // false if "itsMyTurn" voiceline has been used while the child is in their starting position and animation
    bool v4;                            // false if "notRightNowPleaseWaitUntilImDoneUsingIt" voiceline has been used while the child is in their starting position and animation
    #endregion

    #region Audio Clips
    public AudioClip startVoice;
    public AudioClip startVoiceSpanish;
    public AudioClip level2Cry1;
    public AudioClip level2Cry2;
    public AudioClip level2Cry3;
    public AudioClip level3Cry1;
    public AudioClip level3Cry2;
    public AudioClip level3Cry3;
    public AudioClip level4Cry1;
    public AudioClip level4Cry2;
    public AudioClip level4Cry3;
    public AudioClip level5Cry1;
    public AudioClip level5Cry2;
    public AudioClip level5Cry3;
    public AudioClip bodyFalling;
    public AudioClip bodyFallingWithCrying;
    public AudioClip fallCry;
    public AudioClip level4Angry1;
    public AudioClip level4Angry2;
    public AudioClip level5Angry1;
    public AudioClip level5Angry2;
    public AudioClip mixLevel4Angry1;
    public AudioClip mixLevel4Angry2;
    public AudioClip mixLevel5Angry1;
    public AudioClip mixLevel5Angry2;
    public AudioClip level4Sad1;
    public AudioClip level4Sad2;
    public AudioClip level5Sad1;
    public AudioClip level5Sad2;
    public AudioClip walkAudio;
    public AudioClip runAudio;
    public AudioClip pausedAudio;
    public AudioClip canIPlayWithYourPhone;
    public AudioClip canIWatchPawPatrol;
    public AudioClip doYouWantToPlayWithMe;
    public AudioClip giveToMeNow;
    public AudioClip iAmGoingToBiteYou;
    public AudioClip iAmGoingToHateYou;
    public AudioClip iDontCare;
    public AudioClip iHateYou;
    public AudioClip iAmGettingMad;
    public AudioClip itsMyTurn;
    public AudioClip notRightNowImUsingIt;
    public AudioClip notRightNowPleaseWaitUntilImDoneUsingIt;

    public AudioClip whatIsThat;
    public AudioClip whatAreYouDoing;
    public AudioClip whyCantIHaveIt;
    public AudioClip Butwhy;
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
    public AudioClip spanish_youbettergivethetoyorelse;

    #endregion

    public bool isWalkingAndCrying;
    public bool isWalking;
    public Coroutine audioCoroutine;
    public Coroutine audioCoroutine2;
    public Vector3 originalPosition;

    public float previousTantrumLevel;
    public Transform cameraTransform;

    public bool isCoroutineRunning = false;
    public static bool childIsTalking = false;
    //private float timeBetweenVerbalization = 0;
    private float breakTimer;
    public static bool simluationOnGoing;
    public GameObject[] Canvases;
    public static bool gameLost;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        jumpForce = 50f;

        //testflag = true;
        // Initial Tantrum Coefficient and Level
        //childState.setStartTantrumLevel(30);

        // Movement Update Time
        moveTimer = 0;
        moveTime = 2.0f;
        fallTimer = 0f;
        fallTime = 15f;
        jumpTimer = 0f;
        jumpTime = 10f;
        voiceTimer = 0f;

        // Tantrum Coefficient Update Time
        tantrumCoefficientUpdateTimer = 0f;
        tantrumCoefficientUpdateTime = 0.5f;
        tantrumLevel = 0;
        move = new string[] { };

        startPoint = true;
        isIdle = false;

        resetWalk = false;
        startAnimation = true;
        playFlag = false;

        // Initial Map
        floorMap = new float[] { -3.0f, 3.0f, -3.0f, 3.0f };
        distToGround = GetComponent<Collider>().bounds.extents.y;

        // Repetition
        redoTime = 0;
        redoProbability = 0;
        redoAnimation = "";
        redoFlag = false;

        v1 = true;
        v2 = true;
        v3 = true;
        v4 = true;

        Canvases = GameObject.FindGameObjectsWithTag("ChildTantrumAttentionCanvas");
        if (userPrefs.IsEnglishSpeaker())
            PlayAudioClip(startVoice);
        else
            PlayAudioClip(startVoiceSpanish);
        move = new string[] { "firstTalk" };
        StartCoroutine(setMoveatStartVoice());
        StartCoroutine(setStartTantrum());

        rigidBody.useGravity = false;
        rigidBody.isKinematic = false;
        simluationOnGoing = false;
        gameLost = false;
    }



    void Update()
    {
        if (gameLost)
            audioSource.volume = 0f; // mute this to play ending kid voice in simulationcontrol script

        //if the child stop saying. start counting the time
        if (!audioSource.isPlaying)
        {
            breakTimer += Time.deltaTime;
            //timeBetweenVerbalization = Random.Range(5f, 10f);
        }


        childIsTalking = audioSource.isPlaying;

        // if the time reach 5 to 10 seconds. the child talk again
        if (!audioSource.isPlaying && breakTimer > 7f)
        {
            Debug.Log("about to update movement");
            UpdateMovement();
            breakTimer = 0f; //reset
        }
        else if (!audioSource.isPlaying && childState.tantrumLevel > 0)
        {
            move = new string[] { "Sad2Idle" };
        }
        else if (!audioSource.isPlaying && childState.tantrumLevel == 0)
            move = new string[] { "idle" };

        previousTantrumLevel = tantrumLevel;
        tantrumCoefficient = (int)childState.tantrumLevel;
        if (tantrumCoefficient > 100)
            tantrumCoefficient = 100;
        else if (tantrumCoefficient < 0)
            tantrumCoefficient = 0;

        // Convert tantrum coefficient to tantrum level

        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);    // tantrum coefficient is 0-100 and we want 6 levels 0-5
                                                                  //tantrumLevel = 1;
                                                                  //if (tantrumLevel > 5) Debug.Log("Invalid Tantrum Coefficient");

        //Debug.Log("tantrumLevel is " + tantrumLevel);

        if (move.Contains("A"))
        {
            if (!(move.Contains("W") || move.Contains("S")))
                animator.SetInteger("turn", 1);
            base.transform.Rotate(new Vector3(0f, -90f, 0f) * Time.deltaTime);
        }
        else if (move.Contains("D"))
        {

            if (!(move.Contains("W") || move.Contains("S")))
                animator.SetInteger("turn", -1);
            //Debug.Log("in ds");
            base.transform.Rotate(new Vector3(0f, 90f, 0f) * Time.deltaTime);
            //animator.SetInteger("walk", 1);
            //rigidBody.velocity = transform.forward * 0.625f;
        }
        else if (move.Contains("W") || move.Contains("S"))
            animator.SetInteger("turn", 0);


        if (move.Contains("Angry2Idle"))
        {
            animator.Play("Angry");

        }

        if (move.Contains("Sad2Idle"))
        {

            animator.Play("sad");

        }

        if (move.Contains("sad2"))
        {
            animator.Play("sad2");
        }
        if (move.Contains("idle"))
        {
            animator.Play("idle");
        }
        if (move.Contains("idletired"))
        {
            animator.Play("idletired");
        }



        if (move.Contains("ToMainCam"))
        {
            Vector3 targetDirection = mainCamera.transform.position - base.transform.position;
            float singleStep = 2f * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(base.transform.forward, targetDirection, singleStep, 0.0f);
            animator.SetInteger("turn", 1);
            //Vector3 targetDirection = mainCamera.transform.position - transform.position;
            //targetDirection.x -= transform.position.x;
            //targetDirection.z -= transform.position.z;
            //targetDirection.y -= transform.position.y
            float angle = Quaternion.LookRotation(newDirection).y; // Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
            if (angle > 0f)
                move = new string[] { "D" }; //angle = 90f;
            else if (angle < 0f)
                move = new string[] { "A" }; //angle = -90f;
                                             //transform.Rotate(new Vector3(0f, angle, 0f) * Time.deltaTime);
        }

        if (move.Contains("cry1"))
        {
            animator.Play("cry1");
            //animator.SetBool("cry", true);
        }

        if (move.Contains("cry2"))
        {
            animator.Play("cry2");
            //animator.SetBool("cry", true);
        }

        if (move.Contains("stamp"))
        {
            //animator.Play("TK_cry");
            animator.Play("stamp");
        }

        if (move.Contains("telloff"))
        {
            //animator.Play("TK_cry");
            animator.Play("telloff");
        }
        if (move.Contains("idle1"))
        {
            //animator.Play("TK_cry");
            animator.Play("idle1tl0");
        }
        if (move.Contains("idle2"))
        {
            //animator.Play("TK_cry");
            animator.Play("idle2tl0");
        }


        if (move.Contains("WandC") && !isCoroutineRunning)
        {
            originalPosition = transform.position;
            animator.SetInteger("walk", 1);
            StartCoroutine(WalkAndCry());
        }
        else if (move.Contains("S"))
        {
            animator.SetInteger("walk", -1);
            rigidBody.velocity = transform.forward * -0.625f;
            animator.Play("TK_IP_walkbackwards");
            if (animator.GetBool("run"))
                rigidBody.velocity = transform.forward * -1.5f;
        }

        else if (move.Contains("shorttalk"))
        {
            //animator.SetBool("shorttalk", true);
            animator.Play("shorttalk");
        }

        if (move.Contains("firstTalk"))
            animator.Play("firstTalk");


        //move = new string[] { };

    }

    //IEnumerator WalkAndCry()
    //{
    //	isWalking = true;

    //	// Set the "walk" parameter to 1 to trigger the walking animation
    //	animator.SetInteger("walk", 1);

    //	// Set the direction to move to the right (adjust speed as needed)
    //	//Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0001f).normalized;
    //	Vector3 moveDirection = new Vector3((0.0001f * Random.Range(-1f, 1f)), 0, (0.0001f * Random.Range(-1f, 1f))).normalized;

    //	// Calculate the destination position for walking
    //	//Vector3 destination = transform.position + moveDirection * 1.0f; // Move right for 1 meter
    //	Vector3 destination = transform.position + moveDirection * (0.5f); // Move for 15 seconds at an extremely slow pace


    //	float startTime = Time.time;
    //	float journeyLength = Vector3.Distance(transform.position, destination);

    //	// While there's still distance to cover
    //	while (Vector3.Distance(transform.position, destination) > 0.01f)
    //	{
    //		// Calculate the distance covered in this frame
    //		float distanceCovered = (Time.time - startTime) * 0.1f; // Adjust speed as needed

    //		// Calculate the fraction of the journey completed
    //		float fractionOfJourney = distanceCovered / journeyLength;

    //		// Move towards the destination using Lerp for smooth movement
    //		transform.position = Vector3.Lerp(transform.position, destination, fractionOfJourney);
    //		animator.SetInteger("walk", 0);
    //		animator.SetBool("Cry", true);
    //		yield return null;
    //	}

    //	// Wait for a moment before triggering the crying animation
    //	//yield return new WaitForSeconds(1.0f);

    //	// Set the "walk" parameter to 0 to stop the walking animation
    //	animator.SetInteger("walk", 0);

    //	// Trigger the crying animation (assuming you have a "Cry" trigger parameter)
    //	animator.SetBool("Cry", true);


    //	isWalking = false;
    //	}

    IEnumerator WalkAndCry()
    {
        isCoroutineRunning = true;
        isWalking = true;

        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation; // Store the original rotation

        animator.SetInteger("walk", 1);

        // The child turns 90 degrees to his right to face the direction he is supposed to move

        Vector3 destination = transform.position + transform.right * 1.0f; // Use the local right vector of the child
        transform.Rotate(0, 90, 0);

        float step = 0.5f * Time.deltaTime; // You can adjust this value to make the child move faster or slower

        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
            yield return null;
        }

        animator.SetInteger("walk", 0);
        yield return new WaitForSeconds(1f);

        // The child should then turn 180 degrees from his current rotation to walk back to the original position
        transform.Rotate(0, 180, 0);
        animator.SetInteger("walk", 1);

        float returnStep = 1f * Time.deltaTime; // Adjust return speed here
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnStep);
            yield return null;
        }

        transform.rotation = originalRotation; // Reset to the original rotation
        animator.SetInteger("walk", 0);
        isWalking = false;
        animator.SetBool("Cry", true);
        isCoroutineRunning = false;
    }


    void UpdateMovement()
    {
        Debug.Log("################update Movement called");
        if (previousTantrumLevel > tantrumLevel || previousTantrumLevel < tantrumLevel)
        {
            //audioSource.Stop();
            //animator.StopPlayback();
            animator.SetBool("shorttalk", false);
        }

        if (tantrumLevel == 0)
        {

            //if (cameraTransform != null)
            //{
            //	// Calculate the direction from the kid to the camera
            //	Vector3 lookDirection = cameraTransform.position - transform.position;

            //	// Ensure that the kid object stays upright (aligned with the world's up vector)
            //	Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            //	// Apply the rotation to the kid object
            //	transform.rotation = rotation;
            //}


            //animator.StopPlayback();
            int moveType = Random.Range(0, 2);
            switch (moveType)
            {
                case 0:
                    move = new string[] { "idle" };
                    //PlayAudioClip(canIPlayWithYourPhone);
                    break;
                case 1:
                    move = new string[] { "idle" };
                    //PlayAudioClip(canIWatchPawPatrol);
                    break;
                    //case 2:
                    //	move = new string[] { "idle" };
                    //	break;
                    //case 3:
                    //	move = new string[] { "idle" };
                    //	break;
                    //case 4:
                    //	move = new string[] { "idle" };
                    //	break;
                    //case 5:
                    //	move = new string[] { "idle" };
                    //	break;
                    //case 6:
                    //	move = new string[] { "idle" };
                    //	break;
            }
        }
        else if (tantrumLevel == 1)
        {
            int moveType = Random.Range(0, 5);
            AudioClip clip;
            switch (moveType)
            {
                case 0:
                    move = new string[] { "cry1" };
                    PlayAudioClip(level2Cry3);
                    break;
                case 1:
                    move = new string[] { "shorttalk" };
                    clip = userPrefs.IsEnglishSpeaker() ? Ireallywanttoplay : spanish_ireallywanttoplay;
                    PlayAudioClip(clip);
                    break;
                case 2:
                    move = new string[] { "shorttalk" };
                    clip = userPrefs.IsEnglishSpeaker() ? Butwhy : spanish_butwhy;
                    PlayAudioClip(clip);
                    break;
                case 3:
                    move = new string[] { "shorttalk" };
                    clip = userPrefs.IsEnglishSpeaker() ? AllIwanttodoisplaywiththetoys : spanish_allIwanttodoisplaywiththetoys;
                    PlayAudioClip(clip);
                    break;
                case 4:
                    move = new string[] { "cry1" };
                    PlayAudioClip(level2Cry1);
                    break;
            }
        }
        else if (tantrumLevel == 2)
        {

            //if (cameraTransform != null)
            //{
            //	// Calculate the direction from the kid to the camera
            //	Vector3 lookDirection = cameraTransform.position - transform.position;

            //	// Ensure that the kid object stays upright (aligned with the world's up vector)
            //	Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            //	// Apply the rotation to the kid object
            //	transform.rotation = rotation;
            //}


            //move = new string[] { "Angry2Idle" };

            int moveType = Random.Range(0, 3);
            switch (moveType)
            {
                case 0:
                    move = new string[] { "cry1" };
                    PlayAudioClipwithnodelay(level2Cry1);
                    break;
                case 1:
                    move = new string[] { "cry2" };
                    PlayAudioClipwithnodelay(level2Cry2);
                    break;
                case 2:
                    move = new string[] { "cry1" };
                    PlayAudioClipwithnodelay(level2Cry3);
                    break;

            }
        }
        else if (tantrumLevel == 3)
        {

            //if (cameraTransform != null)
            //{
            //	// Calculate the direction from the kid to the camera
            //	Vector3 lookDirection = cameraTransform.position - transform.position;

            //	// Ensure that the kid object stays upright (aligned with the world's up vector)
            //	Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            //	// Apply the rotation to the kid object
            //	transform.rotation = rotation;
            //}

            int moveType = Random.Range(0, 4);
            switch (moveType)
            {
                case 0:
                    move = new string[] { "stamp" };
                    PlayAudioClipwithnodelay(level3Cry1);
                    //voiceTimer = 15f;
                    //audioSource.volume = 0.8f;
                    //audioSource.Play();
                    break;
                case 1:
                    move = new string[] { "stamp" };
                    PlayAudioClipwithnodelay(level3Cry2);
                    //voiceTimer = 15f;
                    //audioSource.volume = 0.8f;
                    //audioSource.Play();
                    break;
                case 2:
                    move = new string[] { "stamp" };
                    PlayAudioClipwithnodelay(level3Cry3);
                    break;
                case 3:
                    move = new string[] { "cry2" };
                    PlayAudioClipwithnodelay(level2Cry2);
                    break;

            }

        }
        else if (tantrumLevel == 4)
        {

            int moveType = Random.Range(0, 7);
            AudioClip clip;
            switch (moveType)
            {
                case 0:  //"WandC"
                    move = new string[] { "cry2" };
                    PlayAudioClipwithnodelay(level4Cry1);
                    break;
                case 1:
                    move = new string[] { "stamp" };
                    PlayAudioClipwithnodelay(level4Cry2);
                    break;
                case 2:
                    move = new string[] { "stamp" };
                    PlayAudioClipwithnodelay(level4Cry3);
                    break;
                case 3:
                    move = new string[] { "telloff" };
                    clip = userPrefs.IsEnglishSpeaker() ? Youaretheworst : spanish_youaretheworst;
                    PlayAudioClipwithnodelay(clip);
                    break;
                case 4:
                    move = new string[] { "telloff" };
                    clip = userPrefs.IsEnglishSpeaker() ? Idontwanttobehereanymore : spanish_idontwanttobehereanymore;
                    PlayAudioClipwithnodelay(clip);
                    break;
                case 5:
                    move = new string[] { "telloff" };
                    clip = userPrefs.IsEnglishSpeaker() ? IfIcantplayIamleaving : spanish_ificantplayiamleaving;
                    PlayAudioClipwithnodelay(clip);
                    break;
                case 6:
                    move = new string[] { "telloff" };
                    clip = userPrefs.IsEnglishSpeaker() ? Youbettergivemethetoysorelse : spanish_youbettergivethetoyorelse;
                    PlayAudioClipwithnodelay(clip);
                    break;
            }
        }
        else if (tantrumLevel == 5)
        {
            //if (cameraTransform != null)
            //{
            //	// Calculate the direction from the kid to the camera
            //	Vector3 lookDirection = cameraTransform.position - transform.position;

            //	// Ensure that the kid object stays upright (aligned with the world's up vector)
            //	Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            //	// Apply the rotation to the kid object
            //	transform.rotation = rotation;
            //}


            int moveType = Random.Range(0, 3);
            switch (moveType)
            {
                case 0:
                    move = new string[] { "cry2" };
                    PlayAudioClipwithnodelay(level5Cry3);
                    //voiceTimer = 15f;
                    //audioSource.volume = 0.8f;
                    //audioSource.Play();
                    break;
                case 1:
                    move = new string[] { "stamp" };
                    PlayAudioClipwithnodelay(level5Cry2);
                    //voiceTimer = 15f;
                    //audioSource.volume = 0.8f;
                    //audioSource.Play();
                    break;
                case 2:
                    move = new string[] { "stamp" };
                    PlayAudioClipwithnodelay(level5Cry1);
                    //voiceTimer = 15f;
                    //audioSource.volume = 0.8f;
                    //audioSource.Play();
                    break;

            }

        }

    }



    void PlayAudioClipwithnodelay(AudioClip clip)
    {

        if (clip != null && audioCoroutine2 == null)
        {

            //float randomDelay = Random.Range(2.0f, 3.0f);
            //yield return new WaitForSeconds(randomDelay);
            audioSource.clip = clip;
            audioSource.Play();

            //StartCoroutine(PlayWithDelay(clip, randomDelay));

            // Start the coroutine to check when the audio has finished playing
            audioCoroutine2 = StartCoroutine(CheckAudioFinished1());
        }
    }

    IEnumerator CheckAudioFinished1()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Audio has finished playing, reset the coroutine reference
        audioCoroutine2 = null;
    }


    void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        /*if (clip != null && audioCoroutine == null)
		{
			//float randomDelay = Random.Range(2.0f, 3.0f); // Generate a random delay between 2 and 3 seconds
			float randomDelay = 1f;
			// Use WaitForSeconds to pause for the random delay
			StartCoroutine(PlayWithDelay(clip, randomDelay));
		}*/
    }

    IEnumerator PlayWithDelay(AudioClip clip, float delay)
    {
        // Wait for the random delay
        yield return new WaitForSeconds(delay);

        audioSource.clip = clip;
        audioSource.Play();

        // Start the coroutine to check when the audio has finished playing
        audioCoroutine = StartCoroutine(CheckAudioFinished());

    }

    IEnumerator CheckAudioFinished()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Audio has finished playing, reset the coroutine reference
        audioCoroutine = null;
    }

    IEnumerator setStartTantrum()
    {
        if (userPrefs.IsEnglishSpeaker())
            yield return new WaitForSeconds(startVoice.length);
        else
            yield return new WaitForSeconds(startVoiceSpanish.length);
        childState.setStartTantrumLevel(30);
        tantrumLevel = Mathf.CeilToInt(childState.tantrumLevel / 20);
        UpdateMovement(); // first tantrum
        simluationOnGoing = true;
    }

    IEnumerator setMoveatStartVoice()
    {
        if (userPrefs.IsEnglishSpeaker())
        {
            yield return new WaitForSeconds(7.2f); // introduction talk
            move = new string[] { "shorttalk" };
            yield return new WaitForSeconds(2f); // Can I play with the toy
            move = new string[] { "Sad2Idle" };
            yield return new WaitForSeconds(5f); // adult talk
            move = new string[] { "telloff" };
            yield return new WaitForSeconds(1.8f); // I really want to play
            move = new string[] { "sad2" };
        }
        else
        {
            move = new string[] { "shorttalk" };
            yield return new WaitForSeconds(1.6f); // Can I play with the toy
            move = new string[] { "Sad2Idle" };
            yield return new WaitForSeconds(5.4f); // adult talk
            move = new string[] { "telloff" };
            yield return new WaitForSeconds(1.3f); // I really want to play
            move = new string[] { "sad2" };
        }

    }
}
