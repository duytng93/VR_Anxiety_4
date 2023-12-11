using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Collections;

public class ChildBehavior : MonoBehaviour
{
	public ChildState childState = new ChildState();
	public float tantrumLevelInV2;

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

	void Start()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		rigidBody = GetComponent<Rigidbody>();
		jumpForce = 50f;

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

		//Play Start Voice
		audioSource.clip = startVoice;
		voiceTimer = 15f;
		audioSource.Play();
	}

	void Update()
	{
		//behavior();

		//Simulate tantrumCoefficient change

		//int change_n = Random.Range(1, 3);
		//if (Input.GetKey(KeyCode.O))
		//	tantrumCoefficient += change_n;
		//if (Input.GetKey(KeyCode.P))
		//	tantrumCoefficient -= change_n;

		#region Update Timers
		moveTimer -= Time.deltaTime;
		fallTimer -= Time.deltaTime;
		jumpTimer -= Time.deltaTime;
		voiceTimer -= Time.deltaTime;
		tantrumCoefficientUpdateTimer -= Time.deltaTime;
		#endregion
		Debug.Log("movetimer value is " + moveTimer);
		// Update movement
		if (moveTimer <= 0)
		{
			Debug.Log("inside movetimer if condition for UpdateMovement()");
			UpdateMovement();
			moveTimer = moveTime;
			//resetwalk = false;
		}

		#region Update Tantrum Coefficient
		if (tantrumCoefficientUpdateTimer <= 0)
		{
			tantrumCoefficient += GazeAnalysis.tantrumCoefficientChange;
			tantrumCoefficientUpdateTimer = tantrumCoefficientUpdateTime;
			//resetwalk = false;
			//flag = 0;
		}

		if (tantrumCoefficient > 100)
			tantrumCoefficient = 100;
		else if (tantrumCoefficient < 0)
			tantrumCoefficient = 0;

        // Convert tantrum coefficient to tantrum level

        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);    // tantrum coefficient is 0-100 and we want 6 levels 0-5
        //tantrumLevel = 1;
		if (tantrumLevel > 5) Debug.Log("Invalid Tantrum Coefficient");

		Debug.Log("tantrumLevelInV2 is " + tantrumLevelInV2);
		Debug.Log("tantrumLevel" + tantrumLevel);
		#endregion

		// Set idle, never used?
		//animator.SetInteger("idle", Random.Range(0, 1200));

		// Check if grounded
		// Send rays downwards from every side of the child's base
		animator.SetBool("grounded",
			Physics.Raycast(transform.position + new Vector3(0.1f, 0.05f, 0.1f), Vector3.down, 0.1f)
			|| Physics.Raycast(transform.position + new Vector3(0.075f, 0.05f, -0.02f), Vector3.down, 0.1f)
			|| Physics.Raycast(transform.position + new Vector3(-0.1f, 0.05f, 0.08f), Vector3.down, 0.1f)
			|| Physics.Raycast(transform.position + new Vector3(-0.1f, 0.05f, -0.05f), Vector3.down, 0.1f));

		//Start Animation
		if (!startAnimation) // if content has been placed and start animation hasn't run
		{
			Debug.Log("animations hasn't run");
			startAnimation = true;
			moveTimer = 16.0f;
		}

		//if (startAnimation)
		//{
		//	Debug.Log("Animations are running and movetimer value is "+moveTimer);
		//	//move = new string[] { "End" };
		//	audioSource.volume = 1f;
		//	if (moveTimer < 3)
		//	{
		//		//animator.SetInteger("walk", 1);
		//		//int walkValue = animator.GetInteger("walk");
		//		Debug.Log("in movetimer3 v1 is "+ v1+" v2 is "+ v2 + " v3 is "+ v3+ " v4 is "+v4);
		//		//PlayAudioClip(whyCantIHaveIt); 
		//		//PlayAudioClip
		//			//audioSource.clip = whyCantIHaveIt;
		//			//audioSource.Play();
		//			//startPoint = false;
		//			voiceTimer = 15f;
		//			//isAudioPlaying = true;
		//	}
		//	else if (v4 && (moveTimer < 7))
		//	{
		//		PlayAudioClip(notRightNowPleaseWaitUntilImDoneUsingIt);
		//		Debug.Log("in movetimer7");
		//		//audioSource.clip = notRightNowPleaseWaitUntilImDoneUsingIt;
		//		//audioSource.Play();
		//		v4 = false;
		//	}
		//	else if (v3 && (moveTimer < 9))
		//	{
		//		PlayAudioClip(itsMyTurn);
		//		Debug.Log("in movetimer9");
		//		//audioSource.clip = itsMyTurn;
		//		//audioSource.Play();
		//		v3 = false;
		//	}
		//	else if (v2 && (moveTimer < 12))
		//	{
		//		Debug.Log("in movetimer12");
		//		PlayAudioClip(notRightNowImUsingIt);
		//		//audioSource.clip = notRightNowImUsingIt;
		//		//audioSource.Play();
		//		v2 = false;
		//	}
		//	else if (v1 && (moveTimer < 15))
		//	{
		//		Debug.Log("in movetimer15");
		//		PlayAudioClip(canIPlayWithYourPhone);
		//		//audioSource.clip = canIPlayWithYourPhone;
		//		//audioSource.Play();
		//		v1 = false;
		//	}
		//}

		// END FLAG
		switch (GazeAnalysis.endFlag)
		{
			case 2:
			case 6: // simulation stopped
				move = new string[] { "End" };
				audioSource.Stop();
				walkRunAudioSource.Stop();
				break;
			case 3: // simulation paused
				move = new string[] { "Paused" };
				audioSource.Stop();
				//wraud.Stop();
				break;
			case 4:
				//UpdateTantrumCoefficient();
				Debug.Log("Inside gazeanalysis endflag block for updateMovement()");
				UpdateMovement();
				audioSource.Stop();
				walkRunAudioSource.Stop();
				tantrumCoefficientUpdateTimer = tantrumCoefficientUpdateTime;
				isIdle = false;
				GazeAnalysis.endFlag = 0;
				break;
		}

        #region Rotate
       // animator.SetInteger("turn", Input.GetAxisRaw("Horizontal"));
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
            base.transform.Rotate(new Vector3(0f, 90f, 0f) * Time.deltaTime);
        }
        else if (move.Contains("W") || move.Contains("S"))
            animator.SetInteger("turn", 0);
        #endregion
        int i = 0;
		foreach (string moves in move)
		{
			i++;
			Debug.Log("move array of strings at this time is "+moves +" and i is "+i + " move array length is "+move.Length);
		}

		//Debug.Log("move array of strings at this time is "+ move);
		// STOP WALK
		if (!(move.Contains("W") || move.Contains("S")))
			animator.SetInteger("walk", 0);

		// RUN
		//animator.SetBool("run", move.Contains("R"));

		// CHECK GROUNDED
		if (!animator.GetBool("grounded"))
		{
			walkRunAudioSource.Stop();
			return;
		}

        /*
		EVERYTHING AFTER THIS POINT REQUIRES BEING GROUNDED
		*/

        // AUDIO
        //if (animator.GetBool("run") && currentWalkRunAudio != runAudio)
        //{
        //	walkRunAudioSource.volume = 0.6f;
        //	currentWalkRunAudio = runAudio;
        //	walkRunAudioSource.clip = currentWalkRunAudio;
        //	walkRunAudioSource.Play();
        //	audioSource.Stop();
        //}
        //else if ((move.Contains("W") || move.Contains("S")) && currentWalkRunAudio != walkAudio)
        //{
        //	walkRunAudioSource.volume = 0.3f;
        //	currentWalkRunAudio = walkAudio;
        //	walkRunAudioSource.clip = currentWalkRunAudio;
        //	walkRunAudioSource.Play();
        //	audioSource.Stop();
        //}
        //else if (move.Contains("Paused") && currentWalkRunAudio != pausedAudio)
        //{
        //	walkRunAudioSource.volume = 1f;
        //	currentWalkRunAudio = pausedAudio;
        //	walkRunAudioSource.clip = currentWalkRunAudio;
        //	walkRunAudioSource.Play();
        //	audioSource.Stop();
        //}
        //else walkRunAudioSource.Stop();

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

        // WALK
        if (!animator.GetBool("cry") && !animator.GetBool("ang2ide") && !animator.GetBool("cry2ide") && !animator.GetBool("fall"))
		{
			Debug.Log("inside the walk animtor evaluation");
			if (move.Contains("W"))
			{

				animator.SetInteger("walk", 1);
				rigidBody.velocity = transform.forward * 0.625f;
				//animator.Play("TK_IP_walk1");
				if (animator.GetBool("run"))
					rigidBody.velocity = transform.forward * 2f;
			}
			else if (move.Contains("S"))
			{
				animator.SetInteger("walk", -1);
				rigidBody.velocity = transform.forward * -0.625f;
				animator.Play("TK_IP_walkbackwards");
				if (animator.GetBool("run"))
					rigidBody.velocity = transform.forward * -1.5f;
			}
		}

		//STRAFE
		if (move.Contains("E") && animator.GetInteger("walk") == 0)
		{
			animator.SetInteger("strafe", 1);
			rigidBody.velocity = transform.right * 0.625f;
			if (animator.GetBool("run"))
			{
				animator.SetInteger("strafe", 2);
				rigidBody.velocity = transform.right * 1.2f;
			}
		}
		else if (move.Contains("Q") && animator.GetInteger("walk") == 0)
		{
			animator.SetInteger("strafe", -1);
			rigidBody.velocity = transform.right * -0.625f;
			if (animator.GetBool("run"))
			{
				animator.SetInteger("strafe", -2);
				rigidBody.velocity = transform.right * -1.2f;
			}
		}
		else animator.SetInteger("strafe", 0);

		//JUMP
		if (move.Contains("J"))
		{
			if (animator.GetInteger("walk") <= 0)
			{
				animator.Play("TK_IP_jump");
				//animator.Play("TK_cry");
				rigidBody.AddForce((Vector3.up) * jumpForce * 1.1f, ForceMode.Acceleration);
			}
			else
			{
				animator.Play("TK_IP_runjump");
				//animator.Play("TK_cry");
				if (animator.GetBool("run"))
					rigidBody.AddForce((transform.forward + new Vector3(0f, 2f, 0f)) * jumpForce, ForceMode.Acceleration);
				else
					rigidBody.AddForce((transform.forward + new Vector3(0f, 3f, 0f)) * jumpForce * 0.45f, ForceMode.Acceleration);
			}
		}

		// CRY
		if (move.Contains("C"))
		{
			if (!playFlag)
			{
				animator.Play("TK_cry");
				playFlag = true;
			}
			if (moveTimer < 0.5)
			{
				animator.SetBool("cry", true);
				move = new string[] { };
			}
		}
		else if (move.Contains("HighC"))
		{
			if (!playFlag)
			{
				animator.Play("High_Cry");
				playFlag = true;
			}
			if (moveTimer < 0.5)
			{
				animator.SetBool("cry", true);
				move = new string[] { };
			}
		}

		// FALL
		if (move.Contains("Fall1"))
		{
			animator.Play("Kid_Fall_1");
			animator.SetBool("fall", true);
		}
		else if (move.Contains("Fall2"))
		{
			animator.Play("Kid_Fall_2");
			animator.SetBool("fall", true);
		}
		/*if ((move.Contains("FallCry")) && animator.GetBool("grounded"))
		{
			animator.Play("Fall_Cry");
			animator.SetBool("fall", true);
		}*/

		// EMOTION
		if (move.Contains("Angry2Idle"))
		{
			animator.Play("Angry");
			animator.SetBool("ang2ide", true);
			if (moveTimer < 1)
				move = new string[] { };
		}
		else if (move.Contains("Sad2Idle"))
		{
			if (!playFlag)
			{
				animator.Play("Sad");
				playFlag = true;
			}
			if (moveTimer < 1)
			{
				animator.SetBool("sad2ide", true);
				move = new string[] { };
			}
		}
		else if (move.Contains("Angry2Cry"))
		{
			if (!playFlag)
			{
				animator.Play("Angry");
				playFlag = true;
			}
			if (moveTimer < 0.5)
			{
				animator.SetBool("ang2ide", true);
				move = new string[] { };
			}
			//animator.SetBool("cry", true);
		}
		else if (move.Contains("Sad2Cry"))
		{
			animator.Play("TK_idlesad");
			animator.SetBool("ang2ide", true);
			//animator.SetBool("cry", true);
		}

		// IDLE
		if (move.Contains("Paused") || move.Contains("End") || move.Contains("Idle"))
			animator.Play("TK_idle1");
	}

	/* Unused
	public void UpdateTantrumCoefficient()
	{
		tantrumCoefficient = GazeAnalysis.tantrumCoefficient;
	}
	*/

	/* Unused
	void onTriggerEnter(Collider collision)
	{
		if (collision.gameobject.name = "Original")
			UpdateMovement;
	}*/

	void UpdateMovement()
	{
		playFlag = false;
		//Deploy Animations for different Tantrum level
		//animator.Play("IDLES");
		animator.SetInteger("walk", 0);
		animator.SetInteger("turn", 0);
		animator.SetInteger("idle", 0);
		animator.SetInteger("strafe", 0);

		move = new string[] { };

		if (redoFlag) RedoAction();
		else if (!isIdle && (animator.GetBool("cry") || animator.GetBool("fall") || animator.GetBool("ang2ide") || animator.GetBool("sad2ide")))
		{
			isIdle = true;
			moveTime = 1.0f;
			move = new string[] { "Idle" };
		}
		else
		{
			isIdle = false;
			animator.SetBool("cry", false);
			animator.SetBool("fall", false);
			animator.SetBool("ang2ide", false);
			animator.SetBool("sad2ide", false);

			if (tantrumLevel == 0)
			{
				Debug.Log("Level 0");

				int moveType = Random.Range(0, 6);
				Debug.Log("moveType in level0 is "+ moveType);
				switch (moveType)
				{
					case 0:
						move = new string[] { "A", "W" };
						moveTime = 3.0f;
						foreach(string moves in move){ Debug.Log("case 0 move array in level 0 is " + moves + " move array length is " + move.Length); }
						break;
					case 1:
						move = new string[] { "W" };
						//moveTime = 1.0f;
						moveTime = 7.0f;
						foreach (string moves in move) { Debug.Log("case 1 move array in level 0 is " + moves + " move array length is " + move.Length); }
						break;
					case 2:
						move = new string[] { "W", "D" };
						//moveTime = 3.0f;
						moveTime = 9.0f;
						foreach (string moves in move) { Debug.Log("case 2 move array in level 0 is " + moves + " move array length is " + move.Length); }
						break;
					case 3:
						move = new string[] { "S" };
						//moveTime = 0.5f;
						moveTime = 12.0f;
						foreach (string moves in move) { Debug.Log("case 3 move array in level 0 is " + moves + " move array length is " + move.Length); }
						break;
					case 4:
						move = new string[] { "D" };
						//moveTime = 1.0f;
						moveTime = 15.0f;
						foreach (string moves in move) { Debug.Log("case 4 move array in level 0 is " + moves + " move array length is " + move.Length); }
						break;
					case 5:
						move = new string[] { "A" };
						//moveTime = 1.0f;
						moveTime = 3.0f;
						foreach (string moves in move) { Debug.Log("case 5 move array in level 0 is " + moves + " move array length is " + move.Length); }
						break;
				}
				resetWalk = false;
			}
			else if (tantrumLevel == 1)
			{
				Debug.Log("Level 1");

				animator.SetBool("run", false);

				int moveType = Random.Range(0, 6);
				switch (3)
				{
					case 0:
						move = new string[] { "A", "W" };
						moveTime = 2.0f;
						resetWalk = false;
						break;
					case 1:
						if (jumpTimer > 0) break;
						move = new string[] { "J" };
						moveTime = 0.3f;
						jumpTimer = jumpTime;
						resetWalk = false;
						break;
					case 2:
						move = new string[] { "S" };
						moveTime = 0.5f;
						resetWalk = false;
						break;
					case 3:
						if (jumpTimer > 0) break;
						move = new string[] { "End" };
						audioSource.volume = 1f;
						moveTime = 1.5f;

						audioSource.clip = new AudioClip[] { iAmGettingMad, giveToMeNow }[Random.Range(0, 2)];
						audioSource.Play();
						voiceTimer = 15f;
						break;
					case 4:
						move = new string[] { "W" };
						animator.SetBool("run", true);
						moveTime = 0.5f;
						resetWalk = false;
						break;
					case 5:
						move = new string[] { "W", "D" };
						moveTime = 2.0f;
						resetWalk = false;
						break;
				}
			}
			else if (tantrumLevel == 2)
			{
				Debug.Log("Level 2");

				int moveType = Random.Range(0, 5);
				switch (0)
				{
					case 0:
						move = new string[] { "C" };
						moveTime = 4.5f;
						//aud.time = moveTime;

						audioSource.volume = 0.6f;
						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { level2Cry1, level2Cry2, level2Cry3 }[Random.Range(0, 3)];

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
							move = new string[] { "ToMainCam" };
							redoAnimation = "C";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
							audioSource.Play();
						break;
					case 1:
						if (voiceTimer > 0) break;
						move = new string[] { "End" };
						moveTime = 1.5f;
						audioSource.volume = 1f;

						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { giveToMeNow, iDontCare, itsMyTurn }[Random.Range(0, 3)];
						audioSource.Play();
						voiceTimer = 15f;
						break;
					case 2:
						//move = new string[] { "A", "W" };
						moveTime = 3.0f;
						resetWalk = false;
						break;
					case 3:
						//move = new string[] { "W", "D" };
						moveTime = 3.0f;
						resetWalk = false;
						break;
					case 4:
						//move = new string[] { "W" };
						moveTime = 0.5f;
						resetWalk = false;
						break;
				}
			}
			else if (tantrumLevel == 3)
			{
				Debug.Log("Level 3");

				int moveType = Random.Range(0, 5);
				switch (0)
				{
					case 0:
						move = new string[] { "C" };
						moveTime = 6.5f;
						//aud.time = moveTime;

						audioSource.volume = 0.7f;
						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { level3Cry1, level3Cry2, level3Cry3 }[Random.Range(0, 3)];

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
							//move = new string[] { "ToMainCam" };
							redoAnimation = "C";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
							audioSource.Play();
						break;
					case 1:
						if (voiceTimer > 0) break;
						move = new string[] { "End" };
						audioSource.volume = 1f;
						moveTime = 1.5f;

						audioSource.clip = youAreTheWorst;
						audioSource.Play();
						voiceTimer = 20f;
						break;
					case 2:
						//move = new string[] { "A", "W" };
						moveTime = 2.0f;
						resetWalk = false;
						break;
					case 3:
						//move = new string[] { "W", "D" };
						moveTime = 2.0f;
						resetWalk = false;
						break;
					case 4:
						if (fallTimer > 0) break;
						move = new string[] { "FallCry" };
						audioSource.volume = 1f;
						moveTime = 18.0f;
						fallTimer = fallTime + moveTime;
						audioSource.clip = fallCry;
						//aud.clip = bodyfalling;
						//aud.clip = bodyfallingwithcrying;
						audioSource.Play();
						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
						//	move = new string[] { "ToMainCam" };
							redoAnimation = "FallCry";
							redoTime = moveTime;
							//fall1_count = moveTime + 1f;
							moveTime = 1f;
							redoFlag = true;
						}
						else
						{
						//	move = new string[] { "W" };
							moveTime = 0.5f;
							resetWalk = false;
						}
						break;
				}
			}
			else if (tantrumLevel == 4)
			{
				Debug.Log("Level 4");

				int moveType = Random.Range(0, 7);
				switch (0)
				{
					case 0:
						move = new string[] { "C" };
						audioSource.clip = iHateYou;
						audioSource.Play();
						moveTime = 10.5f;
						//aud.time = moveTime;

						audioSource.volume = 0.85f;
						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { level4Cry1, level4Cry2, level4Cry3 }[Random.Range(0, 3)];

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
							move = new string[] { "ToMainCam" };
							redoAnimation = "C";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
							audioSource.Play();
						break;
					case 1:
						if (voiceTimer > 0) break;

						move = new string[] { "Angry2Idle" };
						moveTime = 6.0f;

						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { mixLevel4Angry1, mixLevel4Angry2 }[Random.Range(0, 2)];

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
							move = new string[] { "ToMainCam" };
							redoAnimation = "Angry2Idle";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
							audioSource.Play();
						/*
						move = new string[] { "End" };
						moveTime = 1.5f;
						aud.volume = 1f;
						int voicetype = Random.Range(0, 1);
						if (voicetype == 0)
							aud.clip = ihateu;
						//else if (voicetype == 1)
						//	aud.clip = iamgettingmad;
						//else if (voicetype == 2)
						//	aud.clip = givetomenow;
						aud.Play();*/
						voiceTimer = 20f;
						break;
					case 2:
					//	move = new string[] { "A", "W" };
						moveTime = 2.0f;
						resetWalk = false;
						break;
					case 3:
					//	move = new string[] { "W", "D" };
						moveTime = 2.0f;
						resetWalk = false;
						break;
					case 4:
						move = new string[] { "FallCry" };
						audioSource.volume = 1f;
						moveTime = 18.0f;
						fallTimer = fallTime + moveTime;
						audioSource.clip = fallCry;
						audioSource.Play();

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
						//	move = new string[] { "ToMainCam" };
							redoAnimation = "FallCry";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
						{
							animator.Play("Fall_Cry");
							animator.SetBool("fall", true);
						}
						break;
					case 5:
						move = new string[] { "Angry2Idle" };
						audioSource.clip = iHateYou;
						audioSource.Play();
						moveTime = 6.0f;

						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { level4Angry1, level4Angry2 }[Random.Range(0, 2)];

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
						//	move = new string[] { "ToMainCam" };
							redoAnimation = "Angry2Idle";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
							audioSource.Play();
						break;
					case 6:
						move = new string[] { "Sad2Idle" };
						moveTime = 8.0f;

						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { level4Sad1, level4Sad2 }[Random.Range(0, 2)];
						audioSource.Play();

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
						//	move = new string[] { "ToMainCam" };
							redoAnimation = "Sad2Idle";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						break;
					case 7:
					//	move = new string[] { "W" };
						moveTime = 0.5f;
						resetWalk = false;
						break;
				}
			}
			else if (tantrumLevel == 5)
			{
				Debug.Log("Level 5");

				int moveType = Random.Range(0, 10);
				switch (0)//switch (moveType)
				{
					case 0:
						move = new string[] { "Sit_Cry" };
						animator.SetBool("cry", true);
						moveTime = 20.0f;

						int audioRandomChoice = Random.Range(0, 3);
						switch (audioRandomChoice)
						{
							case 0:
								audioSource.volume = 0.8f;
								audioSource.clip = level5Cry1;
								break;
							case 1:
								audioSource.volume = 1f;
								audioSource.clip = level5Cry2;
								break;
							case 2:
								audioSource.volume = 0.6f;
								audioSource.clip = level5Cry3;
								break;
						}
						audioSource.Play();

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
							move = new string[] { "ToMainCam" };
							redoAnimation = "Sit_Cry";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
							animator.Play("Sit_Cry");
						break;
					case 1:
						if (voiceTimer > 0) break;
						move = new string[] { "Angry2Idle" };
						moveTime = 10.0f;

						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { mixLevel5Angry1, mixLevel5Angry2 }[Random.Range(0, 2)];

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
							move = new string[] { "ToMainCam" };
							redoAnimation = "Angry2Idle";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
							audioSource.Play();
						/*
						aud.volume = 1f;
						moveTime = 1.5f;
						int voicetype = Random.Range(0, 2);
						if (voicetype == 0)
							aud.clip = iamgonnabiteu;
						else if (voicetype == 1)
							aud.clip = iamgonnahateu;
						//else if (voicetype == 2)
						//	aud.clip = givetomenow;
						aud.Play();*/
						voiceTimer = 25f;
						break;
					case 2:
				//		move = new string[] { "A", "W" };
						moveTime = 1.0f;
						resetWalk = false;
						break;
					case 3:
				//		move = new string[] { "W", "D" };
						moveTime = 1.0f;
						resetWalk = false;
						break;
					case 4:
					case 5:
					case 6:
						if (fallTimer > 0) break;
						move = new string[] { "FallCry" };
						audioSource.volume = 1f;
						moveTime = 18.0f;
						fallTimer = fallTime + moveTime;
						audioSource.clip = fallCry;
						audioSource.Play();
						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
							move = new string[] { "ToMainCam" };
							redoAnimation = "FallCry";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
						{
							animator.Play("Fall_Cry");
							animator.SetBool("fall", true);
						}
						break;
					case 7:
					case 8:
						move = new string[] { "Angry2Idle" };
						moveTime = 8.0f;

						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { level5Angry1, level5Angry2 }[Random.Range(0, 2)];

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
							move = new string[] { "ToMainCam" };
							redoAnimation = "Angry2Idle";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
							audioSource.Play();
						break;
					case 9:
						move = new string[] { "Sad2Idle" };
						moveTime = 8.0f;

						// Select a random audio clip from the following
						audioSource.clip = new AudioClip[] { level5Sad1, level5Sad2 }[Random.Range(0, 2)];

						redoProbability = Random.Range(0, 3);
						if (redoProbability != 0)
						{
							audioSource.Stop();
							move = new string[] { "ToMainCam" };
							redoAnimation = "Sad2Idle";
							redoTime = moveTime;
							moveTime = 1f;
							redoFlag = true;
						}
						else
							audioSource.Play();
						break;
				}
			}
		}
		// Default case if no move is chosen
		if (move.Length == 0)
		{
			move = new string[] { "W" };
			moveTime = 0.5f;
			resetWalk = false;
		}
	}

	void PlayAudioClip(AudioClip clip)
	{
		if (clip != null && audioCoroutine == null)
		{
			Debug.Log("The name of the clip being played is " + clip);
			// Play the provided audio clip
			audioSource.clip = clip;
			audioSource.Play();

			// Start the coroutine to check when the audio has finished playing
			audioCoroutine = StartCoroutine(CheckAudioFinished());
		}
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

	void RedoAction()
	{
		// Reset animator
		animator.enabled = false;
		animator.enabled = true;
		animator.Play("TK_idle1");
		isIdle = false;
		redoFlag = false;

		animator.SetBool("cry", false);
		animator.SetBool("fall", false);
		animator.SetBool("ang2ide", false);
		animator.SetBool("sad2ide", false);

		move = new string[] { redoAnimation };
		switch (redoAnimation)
		{
			case "HighC":
				moveTime = redoTime;
				//aud.time = moveTime;
				audioSource.volume = 0.6f;
				break;
			case "Sit_Cry":
				Debug.Log("in the redo action");
				moveTime = redoTime;
				//aud.time = moveTime;
				audioSource.volume = 0.6f;
				animator.Play("Sit_Cry");
				animator.SetBool("cry", true);
				break;
			case "Fall1":
				audioSource.volume = 0.8f;
				moveTime = 4.0f;
				fallTimer = fallTime + moveTime;
				audioSource.clip = bodyFalling;
				//fall1_count = moveTime;
				break;
			case "Fall2":
				audioSource.volume = 0.9f;
				moveTime = 4.0f;
				fallTimer = fallTime + moveTime;
				audioSource.clip = bodyFallingWithCrying;
				break;
			case "FallCry":
				audioSource.volume = 1f;
				moveTime = 20.0f;
				fallTimer = fallTime + moveTime;
				audioSource.clip = fallCry;
				animator.Play("Fall_Cry");
				animator.SetBool("fall", true);
				break;
			case "Angry2Idle":
			case "Sad2Idle":
			case "C":
				moveTime = redoTime;
				break;
		}
		audioSource.Play();
	}


	void behavior()
	{
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
					if (audioCoroutine == null)
					{
						audioCoroutine = StartCoroutine(Playtheclip(iDontCare));
					}

					//StartCoroutine(Playtheclip(whatIsThat));
					break;
				case 1:
					//StartCoroutine(Playtheclip(whatIsThat));
					if (audioCoroutine == null)
					{
						audioCoroutine = StartCoroutine(Playtheclip(canIPlayWithYourPhone));
					}
					break;
				case 2:
					//StartCoroutine(Playtheclip(whatIsThat));
					if (audioCoroutine == null)
					{
						audioCoroutine = StartCoroutine(Playtheclip(iAmGettingMad));
					}
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
					StartCoroutine(Playtheclip(level2Cry1));
					break;
				case 1:
					StartCoroutine(Playtheclip(level2Cry2));
					break;
				case 2:
					StartCoroutine(Playtheclip(level2Cry3));
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
					StartCoroutine(Playtheclip(level3Cry1));
					break;
				case 1:
					StartCoroutine(Playtheclip(level3Cry2));
					break;
				case 2:
					StartCoroutine(Playtheclip(level3Cry3));
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
					StartCoroutine(Playtheclip(level4Cry1));
					break;
				case 1:
					StartCoroutine(Playtheclip(level4Cry2));
					break;
				case 2:
					StartCoroutine(Playtheclip(level4Cry3));
					break;
			}
			animator.SetBool("tantrumlevel0", false);
			animator.SetBool("tantrumlevel4", true);
			animator.SetBool("tantrumlevel1", false);
			animator.SetBool("tantrumlevel2", false);
			animator.SetBool("tantrumlevel3", false);
			animator.SetBool("tantrumlevel5", false);
		}
		else if (tantrumLevel == 5)
		{

			int taudio4 = Random.Range(0, 3);
			switch (taudio4)//switch (moveType)
			{
				case 0:
					StartCoroutine(Playtheclip(level5Cry1));
					break;
				case 1:
					StartCoroutine(Playtheclip(level5Cry2));
					break;
				case 2:
					StartCoroutine(Playtheclip(level5Cry3));
					break;
			}
			animator.SetBool("tantrumlevel0", false);
			animator.SetBool("tantrumlevel5", true);
			animator.SetBool("tantrumlevel1", false);
			animator.SetBool("tantrumlevel2", false);
			animator.SetBool("tantrumlevel3", false);
			animator.SetBool("tantrumlevel4", false);
		}
	}
	IEnumerator Playtheclip(AudioClip clip)
	{
		while (true)
		{
			PlayAudioClip(clip);
			yield return new WaitForSeconds(clip.length + 2f); // Add a 2-second delay before playing again
		}
	}



}
