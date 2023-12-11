using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Collections;

public class test : MonoBehaviour
{
	public ChildState childState = new ChildState();
	public float tantrumLevelInV2;
	public float previousTantrumLevel;
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
	public AudioClip youAreTheWorst;
	public AudioClip whatIsThat;
	public AudioClip whatAreYouDoing;
	public AudioClip whyCantIHaveIt;
	#endregion

	public Coroutine audioCoroutine;
	public Coroutine animationCoroutine;
	public bool firstintro=true;

	//public Animator animator; // Reference to the Animator Controller
	public float walkSpeed = 1.0f; // Adjust the walking speed as needed
	public float walkDistance = 5.0f; // Adjust the walking distance
	public float cryDuration = 2.0f; // Duration of crying
	public Transform originalPosition; // Store the original position of the character

	private Vector3 targetPosition; // The position to walk to
	private bool isCrying = false;


	void Start()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		rigidBody = GetComponent<Rigidbody>();
		jumpForce = 50f;

		targetPosition = transform.position + transform.right * walkDistance;


		StartAnimationAndAudio("intro", startVoice);
		// Initial Tantrum Coefficient and Level
		tantrumLevel = 0;
		tantrumCoefficient = 60;

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
		IEnumerator PlayIntroAndAudio()
		{
			// Wait for the animation to finish playing
			yield return new WaitForSeconds(startVoice.length); // Replace "animationLength" with the actual duration of your "intro" animation

			// Play the audio here if needed
			// ...

			// Set "intro" to false after the animation finishes
			animator.SetBool("intro", false);
		}


	}

	void StopAudioClip()
	{
		if (audioSource.isPlaying)
		{

			audioSource.Stop();
		}
	}
	// Update is called once per frame
	void Update()
    {
		previousTantrumLevel = tantrumLevel;
		animator.SetBool("intro", false);
		tantrumLevelInV2 = childState.tantrumLevel;
		tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);    // tantrum coefficient is 0-100 and we want 6 levels 0-5
		//tantrumLevel = 4;
		behavior();
	}

    void PlayAudioClip(AudioClip clip)
    {

        if (clip != null && audioCoroutine == null)
        {
            Debug.Log("The name of the clip being played is " + clip);
            // Play the provided audio clip


            //float randomDelay = Random.Range(2.0f, 3.0f);
            //new WaitForSeconds(randomDelay);
            audioSource.clip = clip;
            audioSource.Play();

            //StartCoroutine(PlayWithDelay(clip, randomDelay));

            // Start the coroutine to check when the audio has finished playing
            audioCoroutine = StartCoroutine(CheckAudioFinished());
        }
    }
    //IEnumerator PlayWithDelay(AudioClip clip, float delay)
    //{
    //    // Wait for the random delay
    //    yield return new WaitForSeconds(delay);

    //    // Play the provided audio clip
    //    audioSource.clip = clip;
    //    audioSource.Play();

    //    // Start the coroutine to check when the audio has finished playing
    //    audioCoroutine = StartCoroutine(CheckAudioFinished());
    //}

    IEnumerator CheckAudioFinished()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Audio has finished playing, reset the coroutine reference
        audioCoroutine = null;
    }

    IEnumerator PlayAudioWithRandomDelay(AudioClip clip)
	{
		if (clip != null)
		{
			Debug.Log("The name of the clip being played is " + clip);

			// Generate a random delay between 2 and 3 seconds
			float randomDelay = Random.Range(2.0f, 3.0f);

			// Wait for the random delay
			yield return new WaitForSeconds(randomDelay);

			// Play the provided audio clip
			audioSource.clip = clip;
			audioSource.Play();

			// Wait for the audio to finish playing
			while (audioSource.isPlaying)
			{
				yield return null;
			}
		}
	}


	void behavior()
	{
		Debug.Log("outside the if condition previousTantrumLevel is " + previousTantrumLevel + " tantrumLevel " + tantrumLevel);

		if (previousTantrumLevel >  tantrumLevel || previousTantrumLevel < tantrumLevel)
		{
			Debug.Log("inside if previousTantrumLevel is "+ previousTantrumLevel + " tantrumLevel "+ tantrumLevel);
			// Tantrum level has gone down to 0, stop any playing audio clip
			StopAudioClip();

		}


		if (tantrumLevel == 0)
		{
			
			animator.SetBool("tantrumlevel0", true);
			animator.SetBool("tantrumlevel1", false);
			animator.SetBool("tantrumlevel2", false);
			animator.SetBool("tantrumlevel3", false);
			animator.SetBool("tantrumlevel4", false);
			animator.SetBool("tantrumlevel5", false);

		}
		else if (tantrumLevel == 1)
		{
			

			int taudio1 = Random.Range(0, 3);
			switch (taudio1)//switch (moveType)
			{
				case 0:
					//if (audioCoroutine == null)
					//{
					//	audioCoroutine = StartCoroutine(Playtheclip(iDontCare));
					//}
					//PlayAudioClip(iDontCare);
					//StartCoroutine(Playtheclip(whatIsThat));
					break;
				case 1:
					//StartCoroutine(Playtheclip(whatIsThat));
					//if (audioCoroutine == null)
					//{
					//	audioCoroutine = StartCoroutine(Playtheclip(canIPlayWithYourPhone));
					//}
					//PlayAudioClip(canIPlayWithYourPhone);
					break;
				case 2:
					//StartCoroutine(Playtheclip(whatIsThat));
					//if (audioCoroutine == null)
					//{
					//	audioCoroutine = StartCoroutine(Playtheclip(iAmGettingMad));
					//}
					//PlayAudioClip(iAmGettingMad);
					break;

			}
			animator.SetBool("tantrumlevel0", false);
			animator.SetBool("tantrumlevel1", true);
			animator.SetBool("tantrumlevel2", false);
			animator.SetBool("tantrumlevel3", false);
			animator.SetBool("tantrumlevel4", false);
			animator.SetBool("tantrumlevel5", false);
			
		}
		else if (tantrumLevel == 2)
		{


			int taudio2 = Random.Range(0, 3);
			switch (taudio2)//switch (moveType)
			{
				case 0:
					if (jumpTimer > 0) break;
					move = new string[] { "tantrumlevel2" };
					audioSource.volume = 1f;
					moveTime = 1.5f;

					audioSource.clip = new AudioClip[] { whatAreYouDoing, giveToMeNow }[Random.Range(0, 2)];
					audioSource.Play();
					voiceTimer = 15f;
					break;
				case 1:
					if (jumpTimer > 0) break;
					move = new string[] { "tantrumlevel2" };
					audioSource.volume = 1f;
					moveTime = 1.5f;

					audioSource.clip = new AudioClip[] { whyCantIHaveIt, whatAreYouDoing }[Random.Range(0, 2)];
					audioSource.Play();
					voiceTimer = 15f;
					//StartCoroutine(Playtheclip(level4Sad2));
					//if (audioCoroutine == null)
					//{
					//	audioCoroutine = StartCoroutine(Playtheclip(level4Sad2));
					//}
					//PlayAudioClip(level2Cry2);
					break;
				case 2:
					if (jumpTimer > 0) break;
					move = new string[] { "tantrumlevel2" };
					audioSource.volume = 1f;
					moveTime = 1.5f;

					audioSource.clip = new AudioClip[] { iHateYou, iAmGoingToHateYou }[Random.Range(0, 2)];
					audioSource.Play();
					voiceTimer = 15f;
					//StartCoroutine(Playtheclip(level4Sad1));
					//if (audioCoroutine == null)
					//{
					//	audioCoroutine = StartCoroutine(Playtheclip(level4Sad1));
					//}
					//PlayAudioClip(level2Cry3);
					break;

			}
			animator.SetBool("tantrumlevel0", false);
			animator.SetBool("tantrumlevel2", true);
			animator.SetBool("tantrumlevel1", false);
			animator.SetBool("tantrumlevel3", false);
			animator.SetBool("tantrumlevel4", false);
			animator.SetBool("tantrumlevel5", false);
	
		}
		else if (tantrumLevel == 3)
		{

			int taudio3 = Random.Range(0, 3);
			switch (taudio3)//switch (moveType)
			{
				case 0:
					//StartCoroutine(Playtheclip(level3Cry1));
					PlayAudioClip(level3Cry1);
					break;
				case 1:
					//StartCoroutine(Playtheclip(level3Cry2));
					PlayAudioClip(level3Cry2);
					break;
				case 2:
					//StartCoroutine(Playtheclip(level3Cry3));
					PlayAudioClip(level3Cry3);
					break;
			}
			animator.SetBool("tantrumlevel0", false);
			animator.SetBool("tantrumlevel3", true);
			animator.SetBool("tantrumlevel1", false);
			animator.SetBool("tantrumlevel2", false);
			animator.SetBool("tantrumlevel4", false);
			animator.SetBool("tantrumlevel5", false);
		
		}
		else if (tantrumLevel == 4)
		{

			int taudio4 = Random.Range(0, 3);
			switch (taudio4)//switch (moveType)
			{
				case 0:
					//StartCoroutine(Playtheclip(level4Cry1));
					PlayAudioClip(level4Cry1);
					break;
				case 1:
					//StartCoroutine(Playtheclip(level4Cry2));
					PlayAudioClip(level4Cry2);
					break;
				case 2:
					//StartCoroutine(Playtheclip(level4Cry3));
					PlayAudioClip(level4Cry3);
					break;
			}


			animator.SetBool("tantrumlevel0", false);
			animator.SetBool("tantrumlevel4", true);
			animator.SetBool("tantrumlevel1", false);
			animator.SetBool("tantrumlevel2", false);
			animator.SetBool("tantrumlevel3", false);
			animator.SetBool("tantrumlevel5", false);

			//if (!isCrying)
			//{
			//	MoveToTarget();
			//}

		}
		else if (tantrumLevel == 5)
		{

			int taudio4 = Random.Range(0, 4);
			switch (taudio4)//switch (moveType)
			{
				case 0:
					//StartCoroutine(Playtheclip(level5Cry1));
					PlayAudioClip(level5Cry1);
					break;
				case 1:
					//StartCoroutine(Playtheclip(level5Cry2));
					PlayAudioClip(level5Cry2);
					break;
				case 2:
					PlayAudioClip(level5Cry3);
					//StartCoroutine(Playtheclip(level5Cry3));
					break;
				case 3:
					PlayAudioClip(iHateYou);
					break;
			}



			//animator.SetBool("IsWalking", false);
			animator.SetBool("tantrumlevel0", false);
			animator.SetBool("tantrumlevel5", true);
			animator.SetBool("tantrumlevel1", false);
			animator.SetBool("tantrumlevel2", false);
			animator.SetBool("tantrumlevel3", false);
			animator.SetBool("tantrumlevel4", false);

		}
	}


	private void MoveToTarget()
	{
		float step = walkSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

		if (transform.position == targetPosition)
		{
			// If the character has reached the target, trigger crying animation
			animator.SetBool("IsWalking", false);
			StartCoroutine(CryAndReturn());
		}
		else
		{
			// If the character is walking, play the walking animation
			animator.SetBool("IsWalking", true);

			// Rotate the character to face the direction of movement
			Vector3 direction = (targetPosition - transform.position).normalized;
			if (direction != Vector3.zero)
			{
				Quaternion lookRotation = Quaternion.LookRotation(direction);
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, step * 5f);
			}
		}

	}
	private IEnumerator CryAndReturn()
	{
		isCrying = true;
		yield return new WaitForSeconds(cryDuration); // Cry for a specified duration

		// Play crying animation (replace with your animation trigger)
		animator.SetTrigger("Cry");

		yield return new WaitForSeconds(cryDuration); // Wait for the crying animation to finish

		// Walk back to the original position
		targetPosition = originalPosition.position;
		isCrying = false;
	}


	IEnumerator Playtheclip(AudioClip clip)
	{
		while (true)
		{
			PlayAudioClip(clip);
			yield return new WaitForSeconds(clip.length + 2f); // Add a 2-second delay before playing again
		}
	}
    void StartAnimationAndAudio(string animtrigger, AudioClip aclip)
    {
        animator.SetBool(animtrigger, true);
        float desiredDuration = 7.3f;
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        float animationSpeed = animationLength / desiredDuration;

        // Set the animation speed
        animator.speed = animationSpeed;
        // Play the audio clip
        audioSource.clip = aclip;
        audioSource.Play();

        // If there is an animation coroutine already running, stop it
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        // Use a coroutine to stop the animation and audio after the audio clip length
        animationCoroutine = StartCoroutine(StopAnimationAndAudioAfterAudioLength(animtrigger, aclip));

    }
    public void StopAnimationAndAudio(string animtrigger, AudioClip aclip)
    {
        // Stop the animation
        animator.SetBool(animtrigger, false);
        animator.speed = 1;
        // Stop the audio playback
        audioSource.Stop();

        // Stop the coroutine if it's running
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
    }

    private IEnumerator StopAnimationAndAudioAfterAudioLength(string animtrigger, AudioClip aclip)
    {
        // Wait for the audio clip to finish playing
        //yield return new WaitForSeconds(aclip.length);
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(aclip.length);



        // Stop both animation and audio
        StopAnimationAndAudio(animtrigger, aclip);
    }

}
