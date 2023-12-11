using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Debug = UnityEngine.Debug;
#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;
#endif

public class Action_System : MonoBehaviour
{
	/*
	Animator anim;
	AudioSource aud;
	public AudioSource wraud;

	Rigidbody rigid;
	Transform trans;
	float jumpforce;
	float moveTimer;
	float moveTime;
	float getTimer;
	float getTime;
	int tan_coe;
	public static int tan_lev;
	public static int emo_lev;
	int change_n;
	int flag;
	bool gr;
	string[] move;
	float[] floormap;
	int tan_coe_get;
	float fall_flag;
	float jump_flag;
	float voice_flag;
	float distToGround;
	bool walkplayed;
	bool runplayed;
	bool pausedplayed;

	bool cry_flag;
	bool falldown_flag;
	bool angry_flag;

	float redotime = 0f;
	int redoprob = 0;
	string redoani = "";
	bool redoflag = false;
	bool resetwalk = false;
	bool startani = false;
	bool play_flag = false;

	public static bool startpoint;
	bool v1 = true;
	bool v2 = true;
	bool v3 = true;
	bool v4 = true;

	//public static float fall1_count = 0f;

	public Camera mainCamera;
	//Magic Leap Control
	//private MLInput.Controller _controller;

	// Tantrum Audio Clip
	public AudioClip lev2_Cry_1;
	public AudioClip lev2_Cry_2;
	public AudioClip lev2_Cry_3;
	public AudioClip lev3_Cry_1;
	public AudioClip lev3_Cry_2;
	public AudioClip lev3_Cry_3;
	public AudioClip lev4_Cry_1;
	public AudioClip lev4_Cry_2;
	public AudioClip lev4_Cry_3;
	public AudioClip lev5_Cry_1;
	public AudioClip lev5_Cry_2;
	public AudioClip lev5_Cry_3;
	public AudioClip bodyfalling;
	public AudioClip bodyfallingwithcrying;
	public AudioClip FallCry;
	public AudioClip lev4_Angry_1;
	public AudioClip lev4_Angry_2;
	public AudioClip lev5_Angry_1;
	public AudioClip lev5_Angry_2;
	public AudioClip mix_lev4_Angry_1;
	public AudioClip mix_lev4_Angry_2;
	public AudioClip mix_lev5_Angry_1;
	public AudioClip mix_lev5_Angry_2;
	public AudioClip lev4_Sad_1;
	public AudioClip lev4_Sad_2;
	public AudioClip lev5_Sad_1;
	public AudioClip lev5_Sad_2;
	public AudioClip walkaudio;
	public AudioClip runaudio;
	public AudioClip pausedaudio;

	public AudioClip caniplaywithyourphone;
	public AudioClip caniwatchpawpatrol;
	public AudioClip douwant2playwithme;
	public AudioClip givetomenow;
	public AudioClip iamgonnabiteu;
	public AudioClip iamgonnahateu;
	public AudioClip idontcare;
	public AudioClip ihateu;
	public AudioClip iamgettingmad;
	public AudioClip itsmyturn;
	public AudioClip notrightnowimusingit;
	public AudioClip notrightnowpleasewaituntilimdoneusingit;
	public AudioClip urtheworst;
	public AudioClip whatisthat;
	public AudioClip whatrudoing;
	public AudioClip ycantihaveit;

	// public static SoundController instance;

	// Start is called before the first frame update
	void Start()
	{
		anim = GetComponent<Animator>();
		aud = GetComponent<AudioSource>();
		rigid = GetComponent<Rigidbody>();
		trans = GetComponent<Transform>();
		jumpforce = 50f;
		// Initial Tantrum Coefficient and Level
		tan_lev = 0;
		tan_coe = 0;
		emo_lev = 0;
		// Movement Update Time
		moveTimer = 0;
		moveTime = 2.0f;
		// Tan_Coe Update Time
		getTimer = 0f; //Start Animation Time
		getTime = 0.5f; //Start Animation Time
		startpoint = true;
		flag = 0;
		// Initial Map
		floormap = new float[] { -3.0f, 3.0f, -3.0f, 3.0f };
		distToGround = GetComponent<Collider>().bounds.extents.y;

		fall_flag = 0f;
		jump_flag = 0f;
		voice_flag = 0f;

		walkplayed = false;
		runplayed = false;
		pausedplayed = false;

		//instance = this;
		//fall1_count = 0f;
		//Magic Leap Control
		//MLInput.Start();
		//MLInput.OnControllerButtonDown += OnButtonDown;
		//MLInput.OnControllerButtonUp += OnButtonUp;
		//_controller = MLInput.GetController(MLInput.Hand.Left);

		//flag
		cry_flag = false;
		falldown_flag = false;
		angry_flag = false;
	}

	// Tan_Coe

	public void Tan_Coe_Exchange()
	{
		tan_coe = GazeControl.tan_coe;
	}

	/*void onTriggerEnter(Collider collision)
    {
		if (collision.gameobject.name = "Original")
			UpdateMovement;
    }* /

	void UpdateMovement()
	{
		play_flag = false;
		//Deploy Animations for different Tantrum level
		//anim.Play("IDLES");
		anim.SetFloat("walk", 0f);
		anim.SetFloat("turn", 0f);
		anim.SetFloat("idle", 0f);
		anim.SetFloat("strafe", 0f);
		if (redoflag)
		{
			anim.enabled = false;
			anim.enabled = true;
			anim.Play("IDLES");
			/*
			if (anim.GetBool("cry") == true && flag == 0)
			{
				flag = 1;
				moveTime = 1.0f;
				if (Random.Range(0, 2) == 1)
					move = new string[] { "Idle" };
				else
					move = new string[] { "Idle" };
			}
			else if (anim.GetBool("fall") == true && flag == 0)
			{
				flag = 1;
				moveTime = 1.0f;
				if (Random.Range(0, 2) == 1)
					move = new string[] { "Idle" };
				else
					move = new string[] { "Idle" };
			}
			else if (anim.GetBool("ang2ide") == true && flag == 0)
			{
				flag = 1;
				moveTime = 1.0f;
				if (Random.Range(0, 2) == 1)
					move = new string[] { "Idle" };
				else
					move = new string[] { "Idle" };
			}
			else if (anim.GetBool("sad2ide") == true && flag == 0)
			{
				flag = 1;
				moveTime = 1.0f;
				if (Random.Range(0, 2) == 1)
					move = new string[] { "Idle" };
				else
					move = new string[] { "Idle" };
			}
			else* /
			{
				flag = 0;
				redoflag = false;
				anim.SetBool("cry", false);
				anim.SetBool("fall", false);
				anim.SetBool("ang2ide", false);
				anim.SetBool("sad2ide", false);
				if (redoani == "HighC")
				{
					move = new string[] { "HighC" };
					moveTime = redotime;
					//aud.time = moveTime;
					aud.volume = 0.6f;
					aud.Play();
					redoflag = false;
				}
				else if (redoani == "Sit_Cry")
				{
					move = new string[] { "Sit_Cry" };
					moveTime = redotime;
					//aud.time = moveTime;
					aud.volume = 0.6f;
					aud.Play();
					redoflag = false;
					anim.Play("Sit_Cry");
					anim.SetBool("cry", true);
				}
				else if (redoani == "Fall1")
				{
					move = new string[] { "Fall1" };
					aud.volume = 0.8f;
					moveTime = 4.0f;
					fall_flag = 15f + moveTime;
					aud.clip = bodyfalling;
					aud.Play();
					redoflag = false;
					//fall1_count = moveTime;
				}
				else if (redoani == "Fall2")
				{
					move = new string[] { "Fall2" };
					aud.volume = 0.9f;
					moveTime = 4.0f;
					fall_flag = 15f + moveTime;
					aud.clip = bodyfallingwithcrying;
					aud.Play();
					redoflag = false;
				}
				else if (redoani == "FallCry")
				{
					move = new string[] { "FallCry" };
					aud.volume = 1f;
					moveTime = 20.0f;
					fall_flag = 15f + moveTime;
					aud.clip = FallCry;
					aud.Play();
					redoflag = false;
					anim.Play("Fall_Cry");
					anim.SetBool("fall", true);
				}
				else if (redoani == "Angry2Idle")
				{
					move = new string[] { "Angry2Idle" };
					moveTime = redotime;
					aud.Play();
					redoflag = false;
				}
				else if (redoani == "Sad2Idle")
				{
					move = new string[] { "Sad2Idle" };
					moveTime = redotime;
					aud.Play();
					redoflag = false;
				}
				else if (redoani == "C")
				{
					move = new string[] { "C" };
					moveTime = redotime;
					aud.Play();
					redoflag = false;
				}
			}
		}
		else if (anim.GetBool("cry") == true && flag == 0)
		{
			flag = 1;
			moveTime = 1.0f;
			move = new string[] { "Idle" };
		}
		else if (anim.GetBool("fall") == true && flag == 0)
		{
			flag = 1;
			moveTime = 1.0f;
			move = new string[] { "Idle" };
		}
		else if (anim.GetBool("ang2ide") == true && flag == 0)
		{
			flag = 1;
			moveTime = 1.0f;
			move = new string[] { "Idle" };
		}
		else if (anim.GetBool("sad2ide") == true && flag == 0)
		{
			flag = 1;
			moveTime = 1.0f;
			move = new string[] { "Idle" };
		}
		else if (tan_lev == 0)
		{
			Debug.Log("Level 0");
			int movetype = Random.Range(0, 6);
			flag = 0;
			anim.SetBool("cry", false);
			anim.SetBool("fall", false);
			anim.SetBool("ang2ide", false);
			anim.SetBool("sad2ide", false);
			if (movetype == 0)
			{
				move = new string[] { "A", "W" };
				moveTime = 3.0f;
				resetwalk = false;
			}
			else if (movetype == 1)
			{
				move = new string[] { "W" };
				moveTime = 1.0f;
				resetwalk = false;
			}
			else if (movetype == 2)
			{
				move = new string[] { "W", "D" };
				moveTime = 3.0f;
				resetwalk = false;
			}
			else if (movetype == 3)
			{
				move = new string[] { "S" };
				moveTime = 0.5f;
				resetwalk = false;
			}
			else if (movetype == 4)
			{
				move = new string[] { "D" };
				moveTime = 1.0f;
				resetwalk = false;
			}
			else if (movetype == 5)
			{
				move = new string[] { "A" };
				moveTime = 1.0f;
				resetwalk = false;
			}
		}
		else if (tan_lev == 1)
		{
			Debug.Log("Level 1");
			int movetype = Random.Range(0, 6);
			flag = 0;
			anim.SetBool("cry", false);
			anim.SetBool("fall", false);
			anim.SetBool("ang2ide", false);
			anim.SetBool("sad2ide", false);
			if (movetype == 0)
			{
				move = new string[] { "A", "W" };
				moveTime = 2.0f;
				resetwalk = false;
			}
			else if (movetype == 1 && jump_flag == 0)
			{
				move = new string[] { "J" };
				moveTime = 0.3f;
				jump_flag = 10f;
				resetwalk = false;
			}
			else if (movetype == 2)
			{
				move = new string[] { "S" };
				moveTime = 0.5f;
				resetwalk = false;
			}
			else if (movetype == 3 && voice_flag == 0)
			{
				move = new string[] { "End" };
				aud.volume = 1f;
				moveTime = 1.5f;
				int voicetype = Random.Range(0, 3);
				if (voicetype == 0)
					aud.clip = ycantihaveit;
				else if (voicetype == 1)
					aud.clip = iamgettingmad;
				else if (voicetype == 2)
					aud.clip = givetomenow;
				aud.Play();
				voice_flag = 15f;
			}
			else if (movetype == 4)
			{
				move = new string[] { "W", "R" };
				moveTime = 0.5f;
				resetwalk = false;
			}
			else //if (movetype == 3)
			{
				move = new string[] { "W", "D" };
				moveTime = 2.0f;
				resetwalk = false;
			}
		}
		else if (tan_lev == 2)
		{
			Debug.Log("Level 2");
			int movetype = Random.Range(0, 5);
			{
				flag = 0;
				anim.SetBool("cry", false);
				anim.SetBool("fall", false);
				anim.SetBool("ang2ide", false);
				anim.SetBool("sad2ide", false);
				if (movetype == 0)
				{
					move = new string[] { "C" };
					moveTime = 4.5f;
					//aud.time = moveTime;
					int audiorandom = Random.Range(0, 3);
					aud.volume = 0.6f;
					if (audiorandom == 0)
					{
						aud.clip = lev2_Cry_1;
					}
					else if (audiorandom == 1)
					{
						aud.clip = lev2_Cry_2;
					}
					else
					{
						aud.clip = lev2_Cry_3;
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "C";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
						aud.Play();
				}
				else if (movetype == 1 && voice_flag == 0)
				{
					move = new string[] { "End" };
					moveTime = 1.5f;
					aud.volume = 1f;
					int voicetype = Random.Range(0, 3);
					if (voicetype == 0)
						aud.clip = givetomenow;
					else if (voicetype == 1)
						aud.clip = idontcare;
					else if (voicetype == 2)
						aud.clip = itsmyturn;
					aud.Play();
					voice_flag = 15f;
				}
				else if (movetype == 2)
				{
					move = new string[] { "A", "W" };
					moveTime = 3.0f;
					resetwalk = false;
				}
				else if (movetype == 3)
				{
					move = new string[] { "W", "D" };
					moveTime = 3.0f;
					resetwalk = false;
				}
				else //if (movetype == 2)
				{
					move = new string[] { "W" };
					moveTime = 0.5f;
					resetwalk = false;
				}
			}
		}
		else if (tan_lev == 3)
		{
			Debug.Log("Level 3");
			int movetype = Random.Range(0, 5);
			{
				flag = 0;
				anim.SetBool("cry", false);
				anim.SetBool("fall", false);
				anim.SetBool("ang2ide", false);
				anim.SetBool("sad2ide", false);
				if (movetype == 0)
				{
					move = new string[] { "C" };
					moveTime = 6.5f;
					//aud.time = moveTime;
					int audiorandom = Random.Range(0, 3);
					aud.volume = 0.7f;
					if (audiorandom == 0)
					{
						aud.clip = lev3_Cry_1;
					}
					else if (audiorandom == 1)
					{
						aud.clip = lev3_Cry_2;
					}
					else
					{
						aud.clip = lev3_Cry_3;
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "C";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
						aud.Play();
				}
				else if (movetype == 1 && voice_flag == 0)
				{
					move = new string[] { "End" };
					aud.volume = 1f;
					moveTime = 1.5f;
					int voicetype = Random.Range(0, 1);
					if (voicetype == 0)
						aud.clip = urtheworst;
					//else if (voicetype == 1)
					//	aud.clip = iamgettingmad;
					//else if (voicetype == 2)
					//	aud.clip = givetomenow;
					aud.Play();
					voice_flag = 20f;
				}
				else if (movetype == 2)
				{
					move = new string[] { "A", "W" };
					moveTime = 2.0f;
					resetwalk = false;
				}
				else if (movetype == 3)
				{
					move = new string[] { "W", "D" };
					moveTime = 2.0f;
					resetwalk = false;
				}
				else if (movetype == 4 && fall_flag == 0)
				{
					move = new string[] { "FallCry" };
					aud.volume = 1f;
					moveTime = 18.0f;
					fall_flag = 15f + moveTime;
					aud.clip = FallCry;
					//aud.clip = bodyfalling;
					//aud.clip = bodyfallingwithcrying;
					aud.Play();
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "FallCry";
						redotime = moveTime;
						//fall1_count = moveTime + 1f;
						moveTime = 1f;
						redoflag = true;
					}
					//else
					//	fall1_count = moveTime;
					else //if (movetype == 2)
					{
						move = new string[] { "W" };
						moveTime = 0.5f;
						resetwalk = false;
					}
				}
			}
		}
		else if (tan_lev == 4)
		{
			Debug.Log("Level 4");
			int movetype = Random.Range(0, 7);
			{
				flag = 0;
				anim.SetBool("cry", false);
				anim.SetBool("fall", false);
				anim.SetBool("ang2ide", false);
				anim.SetBool("sad2ide", false);
				if (movetype == 0)
				{
					move = new string[] { "C" };
					aud.clip = ihateu;
					aud.Play();
					moveTime = 10.5f;
					//aud.time = moveTime;
					int audiorandom = Random.Range(0, 3);
					aud.volume = 0.85f;
					if (audiorandom == 0)
					{
						aud.clip = lev4_Cry_1;
					}
					else if (audiorandom == 1)
					{
						aud.clip = lev4_Cry_2;
					}
					else
					{
						aud.clip = lev4_Cry_3;
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "C";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
						aud.Play();
				}
				else if (movetype == 1 && voice_flag == 0)
				{
					move = new string[] { "Angry2Idle" };
					moveTime = 6.0f;
					int audiorandom = Random.Range(0, 2);
					if (audiorandom == 0)
					{
						aud.clip = mix_lev4_Angry_1;
					}
					else
					{
						aud.clip = mix_lev4_Angry_2;
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "Angry2Idle";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
						aud.Play();
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
					aud.Play();
	* /
					voice_flag = 20f;
				}
				else if (movetype == 2)
				{
					move = new string[] { "A", "W" };
					moveTime = 2.0f;
					resetwalk = false;
				}
				else if (movetype == 3)
				{
					move = new string[] { "W", "D" };
					moveTime = 2.0f;
					resetwalk = false;
				}
				else if (movetype == 4 && fall_flag == 0)
				{
					move = new string[] { "FallCry" };
					aud.volume = 1f;
					moveTime = 18.0f;
					fall_flag = 15f + moveTime;
					aud.clip = FallCry;
					aud.Play();
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "FallCry";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
					{
						anim.Play("Fall_Cry");
						anim.SetBool("fall", true);
					}
				}
				else if (movetype == 5)
				{
					move = new string[] { "Angry2Idle" };
					aud.clip = ihateu;
					aud.Play();
					moveTime = 6.0f;
					int audiorandom = Random.Range(0, 2);
					if (audiorandom == 0)
					{
						aud.clip = lev4_Angry_1;
					}
					else
					{
						aud.clip = lev4_Angry_2;
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "Angry2Idle";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
						aud.Play();
				}
				else if (movetype == 6)
				{
					move = new string[] { "Sad2Idle" };
					moveTime = 8.0f;
					int audiorandom = Random.Range(0, 2);
					if (audiorandom == 0)
					{
						aud.clip = lev5_Sad_1;
						aud.Play();
					}
					else
					{
						aud.clip = lev5_Sad_2;
						aud.Play();
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "Sad2Idle";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
				}
				else 
				{
					move = new string[] { "W" };
					moveTime = 0.5f;
					resetwalk = false;
				}
			}
		}
		else if (tan_lev == 5)
		{
			Debug.Log("Level 5");
			int movetype = Random.Range(0, 10);
			{
				flag = 0;
				anim.SetBool("cry", false);
				anim.SetBool("fall", false);
				anim.SetBool("ang2ide", false);
				anim.SetBool("sad2ide", false);
				if (movetype == 0)
				{
					move = new string[] { "Sit_Cry" };
					anim.SetBool("cry", true);
					moveTime = 20.0f;
					//aud.time = moveTime;
					int audiorandom = Random.Range(0, 3);
					if (audiorandom == 0)
					{
						aud.volume = 0.8f;
						aud.clip = lev5_Cry_1;
						aud.Play();
					}
					else if (audiorandom == 1)
					{
						aud.volume = 1f;
						aud.clip = lev5_Cry_2;
						aud.Play();
					}
					else
					{
						aud.volume = 0.6f;
						aud.clip = lev5_Cry_3;
						aud.Play();
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "Sit_Cry";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
                    else
                    {
						anim.Play("Sit_Cry");
                    }
				}
				else if (movetype == 1 && voice_flag == 0)
				{
					move = new string[] { "Angry2Idle" };
					moveTime = 10.0f;
					int audiorandom = Random.Range(0, 2);
					if (audiorandom == 0)
					{
						aud.clip = mix_lev5_Angry_1;
					}
					else
					{
						aud.clip = mix_lev5_Angry_2;
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "Angry2Idle";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
						aud.Play();
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
					aud.Play();
	* /
					voice_flag = 25f;
				}
				else if (movetype == 2)
				{
					move = new string[] { "A", "W" };
					moveTime = 1.0f;
					resetwalk = false;
				}
				else if (movetype == 3)
				{
					move = new string[] { "W", "D" };
					moveTime = 1.0f;
					resetwalk = false;
				}
				else if ((movetype == 4 || movetype == 7 || movetype == 8) && fall_flag == 0)
				{
					move = new string[] { "FallCry" };
					aud.volume = 1f;
					moveTime = 18.0f;
					fall_flag = 15f + moveTime;
					aud.clip = FallCry;
					aud.Play();
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "FallCry";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
                    {
						anim.Play("Fall_Cry");
						anim.SetBool("fall", true);
					}
				}
				else if (movetype == 5 || movetype == 6)
				{
					move = new string[] { "Angry2Idle" };
					moveTime = 8.0f;
					int audiorandom = Random.Range(0, 2);
					if (audiorandom == 0)
					{
						aud.clip = lev5_Angry_1;
					}
					else
					{
						aud.clip = lev5_Angry_2;
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "Angry2Idle";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
						aud.Play();
				}
				else if (movetype == 9)// || movetype == 8)
				{
					move = new string[] { "Sad2Idle" };
					moveTime = 8.0f;
					int audiorandom = Random.Range(0, 2);
					if (audiorandom == 0)
					{
						aud.clip = lev5_Sad_1;
					}
					else
					{
						aud.clip = lev5_Sad_1;
					}
					redoprob = Random.Range(0, 3);
					if (!(redoprob == 0))
					{
						aud.Stop();
						move = new string[] { "ToMainCam" };
						redoani = "Sad2Idle";
						redotime = moveTime;
						moveTime = 1f;
						redoflag = true;
					}
					else
						aud.Play();
				}
				else //if (movetype == 2)
				{
					move = new string[] { "W" };
					moveTime = 0.5f;
					resetwalk = false;
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		//Simulate tan_coe change
		/*
		change_n = Random.Range(1, 3);
		if (Input.GetKey(KeyCode.O))
		{
			if (tan_coe + change_n <= 100)
			{
				tan_coe = tan_coe + change_n;
			}
			else
			{
				tan_coe = 100;
			}
		}
		if (Input.GetKey(KeyCode.P))
		{
			if (tan_coe - change_n >= 0)
			{
				tan_coe = tan_coe - change_n;
			}
			else
			{
				tan_coe = 0;
			}
		}* /

		// Fall_flag
		if (fall_flag > 0)
			fall_flag -= Time.deltaTime;
		if (fall_flag < 0)
			fall_flag = 0f;

		// Fall1 count
		//if (fall1_count > 0)
		//	fall1_count -= Time.deltaTime;
		//if (fall1_count < 0)
		//	fall1_count = 0f;

		// Jump_flag
		if (jump_flag > 0)
			jump_flag -= Time.deltaTime;
		if (jump_flag < 0)
			jump_flag = 0f;

		//Voice_flag
		if (voice_flag > 0)
			voice_flag -= Time.deltaTime;
		if (voice_flag < 0)
			voice_flag = 0f;

		// Get Tan_Coe
		if (getTimer > 0)
			getTimer -= Time.deltaTime;
		if (getTimer < 0)
			getTimer = 0;
		if (getTimer == 0)
		{
			Tan_Coe_Exchange();
			//resetwalk = false;
			getTimer = getTime;
			//flag = 0;
		}
		//tan_coe = 90;
		//Check tan_coe
		if (tan_coe == 0)
		{
			tan_lev = 0;
		}
		else if (tan_coe < 21)
		{
			tan_lev = 1;
		}
		else if (tan_coe < 41)
		{
			tan_lev = 2;
		}
		else if (tan_coe < 61)
		{
			tan_lev = 3;
		}
		else if (tan_coe < 81)
		{
			tan_lev = 4;
		}
		else if (tan_coe < 101)
		{
			tan_lev = 5;
		}
		else
		{
			Debug.Log("Wrong Coe");
		}

		Debug.Log(tan_coe);
		Debug.Log(tan_lev);
		//IDLES
		anim.SetInteger("idle", Random.Range(0, 1200));

		//CHECK GROUNDED
		//distToGround = collider.bounds.extents.y;
		if //(Physics.Raycast(trans.position, Vector3.down, distToGround + 0.1f)) 
		(Physics.Raycast(trans.position + new Vector3(0.1f, 0.05f, 0.1f), Vector3.down, 0.1f)
			|| Physics.Raycast(trans.position + new Vector3(0.075f, 0.05f, -0.02f), Vector3.down, 0.1f)
			|| Physics.Raycast(trans.position + new Vector3(-0.1f, 0.05f, 0.08f), Vector3.down, 0.1f)
			|| Physics.Raycast(trans.position + new Vector3(-0.1f, 0.05f, -0.05f), Vector3.down, 0.1f))
		{
			anim.SetBool("grounded", true);
			gr = true;

		}
		else
		{
			anim.SetBool("grounded", false);
			gr = false;
		}
		/*
		//Deploy Animations for different Tantrum level
		if (tan_lev == 0)
		{
			Debug.Log("Level 0");
			if (Random.Range(0, 10) == 0)
			{
				move = new string[] { "A", "W" };
			}
			else
			{
				move = new string[] { "D" };//, "S" };
			}
		} * /
		//Deploy Animations for different Tantrum level
		if (moveTimer > 0)
			moveTimer -= Time.deltaTime;
		if (moveTimer < 0)
			moveTimer = 0;
		if (moveTimer == 0 && !startpoint)
		{
			UpdateMovement();
			moveTimer = moveTime;
			//resetwalk = false;
		}
		//Invoke("UpdateMovement", 5f);

		//Start Animation
		
		if ((GazeControl.placeflag == 1) && (!startani))
        {
			startani = true;
			moveTimer = 16.0f;
        } 
		if (startpoint && startani)
        {
			move = new string[] { "End" };
			aud.volume = 1f;
			if (moveTimer < 3)
			{
				aud.clip = ycantihaveit;
				aud.Play();
				startpoint = false;
				voice_flag = 15f;
			}
			else if ((moveTimer < 7) && v4)
			{
				aud.clip = notrightnowpleasewaituntilimdoneusingit;
				aud.Play();
				v4 = false;
			}
			else if ((moveTimer < 9) && v3)
			{
				aud.clip = itsmyturn;
				aud.Play();
				v3 = false;
			}
			else if ((moveTimer < 12) && v2)
			{
				aud.clip = notrightnowimusingit;
				aud.Play();
				v2 = false;
			}
			else if ((moveTimer < 15) && v1)
			{
				aud.clip = caniplaywithyourphone;
				aud.Play();
				v1 = false;
			}		
		}
		//Check end_flag
		if (GazeControl.end_flag == 2 || GazeControl.end_flag == 6) // 2 and 6 means simulation stop, 3 means simulation paused
        {
			move = new string[] { "End" };
			aud.Stop();
			wraud.Stop();
		}
		if (GazeControl.end_flag == 3)
        {
			move = new string[] { "Paused" };
			aud.Stop();
			//wraud.Stop();
			walkplayed = true;
			runplayed = true;
		}
		if (GazeControl.end_flag == 4)
        {
			//Tan_Coe_Exchange();
			UpdateMovement();
			aud.Stop();
			wraud.Stop();
			getTimer = getTime;
			flag = 0;
			GazeControl.end_flag = 0;
			walkplayed = false;
			runplayed = false;
			pausedplayed = false;
		}

		//TRANSLATE

		//walking and running audio
		if (move.Contains("R") && gr == true)
        {
			if (!runplayed)
            {
				wraud.volume = 0.6f;
				wraud.clip = runaudio;
				wraud.Play();
				runplayed = true;
				walkplayed = false;
				aud.Stop();
			}
        }
		else if ((move.Contains("W") && gr == true) || (move.Contains("S") && gr == true))
        {
			if (!walkplayed)
			{
				wraud.volume = 0.3f;
				wraud.clip = walkaudio;
				wraud.Play();
				walkplayed = true;
				aud.Stop();
			}
		}
		else if (move.Contains("Paused") && gr == true)
        {
			if (!pausedplayed)
			{
				wraud.volume = 1f;
				wraud.clip = pausedaudio;
				wraud.Play();
				pausedplayed = true;
				aud.Stop();
			}
		}
		else
        {
			wraud.Stop();
			walkplayed = false;
			runplayed = false;
		}

		//anim.SetFloat("walk", 1);//Input.GetAxisRaw("Vertical"));
		if ((move.Contains("ToMainCam")) && gr == true)
		{
			Vector3 targetDirection = mainCamera.transform.position - transform.position;
			float singleStep = 2f * Time.deltaTime;
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
			//transform.rotation = Quaternion.LookRotation(newDirection);
			anim.SetFloat("turn", 1);
			//Vector3 targetDirection = mainCamera.transform.position - transform.position;
			//targetDirection.x = targetDirection.x - transform.position.x;
			//targetDirection.z = targetDirection.z - transform.position.z;
			//targetDirection.y = targetDirection.y - transform.position.y
			float angle = Quaternion.LookRotation(newDirection).y; // Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
			if (angle > 0f)
				move = new string[] { "D" }; //angle = 90f;
			else if (angle < 0f)
				move = new string[] { "A" }; //angle = -90f;
			//transform.Rotate(new Vector3(0f, angle, 0f) * Time.deltaTime);
		}
		//if (!((move.Contains("A")) || move.Contains("D") || move.Contains("AToMainCam")))
		//{
		//	anim.SetFloat("turn", 0);
		//}

		if (move.Contains("W") && gr == true && anim.GetBool("cry") == false && anim.GetBool("ang2ide") == false && anim.GetBool("cry2ide") == false && anim.GetBool("fall") == false)
		{
			anim.SetFloat("walk", 1);
			rigid.velocity = trans.forward * 0.625f;
			//anim.Play("TK_IP_walk1");
			if (anim.GetBool("run") == true)
				rigid.velocity = trans.forward * 2f;
		}
		if (move.Contains("S") && gr == true && anim.GetBool("cry") == false && anim.GetBool("ang2ide") == false && anim.GetBool("cry2ide") == false && anim.GetBool("fall") == false)
		{
			anim.SetFloat("walk", -1);
			rigid.velocity = trans.forward * -0.625f;
			anim.Play("TK_IP_walkbackwards");
			if (anim.GetBool("run") == true)
				rigid.velocity = trans.forward * -1.5f;
		}
		if (!((move.Contains("W")) || move.Contains("S")))
		{
			anim.SetFloat("walk", 0);
		}

		//ROTATE
		//anim.SetFloat("turn", Input.GetAxisRaw("Horizontal"));
		if (move.Contains("A"))
		{
			if (!((move.Contains("W")) || move.Contains("S")))
			{
				anim.SetFloat("turn", 1);
			}
			transform.Rotate(new Vector3(0f, -90f, 0f) * Time.deltaTime);
		}
		if (move.Contains("D"))
		{
			if (!((move.Contains("W")) || move.Contains("S")))
			{
				anim.SetFloat("turn", -1);
			}
			transform.Rotate(new Vector3(0f, 90f, 0f) * Time.deltaTime);
		}
		if (!(move.Contains("A") || move.Contains("D")) || (move.Contains("W") || move.Contains("S")))
		{
			anim.SetFloat("turn", 0);
		}

		//RUN
		if (move.Contains("R"))
			anim.SetBool("run", true);
		else
			anim.SetBool("run", false);

		//STRAFE
		if ((move.Contains("E")) || move.Contains("Q"))
		{
			if ((move.Contains("E")) && gr == true && anim.GetFloat("walk") == 0f)
			{
				anim.SetFloat("strafe", 1f);
				rigid.velocity = trans.right * 0.625f;
				if (anim.GetBool("run") == true)
				{
					anim.SetFloat("strafe", 2f);
					rigid.velocity = trans.right * 1.2f;
				}
			}
			if ((move.Contains("Q")) && gr == true && anim.GetFloat("walk") == 0f)
			{
				anim.SetFloat("strafe", -1f);
				rigid.velocity = trans.right * -0.625f;
				if (anim.GetBool("run") == true)
				{
					anim.SetFloat("strafe", -2f);
					rigid.velocity = trans.right * -1.2f;
				}
			}
		}
		else anim.SetFloat("strafe", 0f);

		//JUMP
		if ((move.Contains("J")) && gr == true)
		{
			if (anim.GetFloat("walk") <= 0f)
			{
				anim.Play("TK_IP_jump");
				//anim.Play("TK_cry");
				rigid.AddForce((Vector3.up) * jumpforce * 1.1f, ForceMode.Acceleration);
			}
			if (anim.GetFloat("walk") > 0f)
			{
				anim.Play("TK_IP_runjump");
				//anim.Play("TK_cry");
				if (anim.GetBool("run") == true)
					rigid.AddForce((trans.forward + new Vector3(0f, 2f, 0f)) * jumpforce, ForceMode.Acceleration);
				else
					rigid.AddForce((trans.forward + new Vector3(0f, 3f, 0f)) * jumpforce * 0.45f, ForceMode.Acceleration);
			}
		}

		//Cry
		if ((move.Contains("C")) && gr == true)
		{
			if (!play_flag)
            {
				anim.Play("TK_cry");
				play_flag = true;
			}
			if (moveTimer < 0.5)
            {
				anim.SetBool("cry", true);
				move = new string[] { };
			}
		}

		if ((move.Contains("HighC")) && gr == true)
		{
			if (!play_flag)
			{
				anim.Play("High_Cry");
				play_flag = true;
			}
			if (moveTimer < 0.5)
			{
				anim.SetBool("cry", true);
				move = new string[] { };
			}
		}

		//Fall
		if ((move.Contains("Fall1")) && gr == true)
		{
			anim.Play("Kid_Fall_1");
			anim.SetBool("fall", true);
		}
		if ((move.Contains("Fall2")) && gr == true)
		{
			anim.Play("Kid_Fall_2");
			anim.SetBool("fall", true);
		}
		/*if ((move.Contains("FallCry")) && gr == true)
		{
			anim.Play("Fall_Cry");
			anim.SetBool("fall", true);
		}* /

		//Angry2Idle
		if ((move.Contains("Angry2Idle")) && gr == true)
		{
			anim.Play("Angry");
			anim.SetBool("ang2ide", true);
			if (moveTimer < 1)
				move = new string[] {};
		}

		//Sad2Idle
		if ((move.Contains("Sad2Idle")) && gr == true)
		{
			if (!play_flag)
			{
				anim.Play("Sad");
				play_flag = true;
			}
			if (moveTimer < 1)
			{
				anim.SetBool("sad2ide", true);
				move = new string[] { };
			}
		}

		//Angry2Cry
		if ((move.Contains("Angry2Cry")) && gr == true)
		{
			if (!play_flag)
			{
				anim.Play("Angry");
				play_flag = true;
			}
			if (moveTimer < 0.5)
			{
				anim.SetBool("ang2ide", true);
				move = new string[] { };
			}
			//anim.SetBool("cry", true);
		}

		//Sad2Cry
		if ((move.Contains("Sad2Cry")) && gr == true)
		{
			anim.Play("TK_idlesad");
			anim.SetBool("ang2ide", true);
			//anim.SetBool("cry", true);
		}

		//IDLE
		if ((move.Contains("Idle")) && gr == true)
		{
			//anim.Play("IDLES", -1, 0f);
		}

		//End
		if ((move.Contains("End")) && gr == true)
		{
			anim.Play("IDLES");
		}

		//Paused
		if ((move.Contains("Paused")) && gr == true)
		{
			anim.Play("IDLES");
		}
	}
	*/
}