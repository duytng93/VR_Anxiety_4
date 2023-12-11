using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    GameObject child, playerObject;
    //private bool npcIsIdle, npcHappy;
    private UserPrefs userPrefs;
    private string move;
    public GameObject[] spots;
    private GameObject nextSpot;
    private int tantrumLevel;
    private bool atStart;
    //private bool isWalking;
    //private TextMeshProUGUI DebugLabel;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        if (userPrefs.GetChildAvatar() == Enums.ChildAvatars.Hispanic)
            child = GameObject.Find("TKGirlA");
        else if (userPrefs.GetChildAvatar() == Enums.ChildAvatars.Black)
            child = GameObject.Find("TKGirlB");
        else if (userPrefs.GetChildAvatar() == Enums.ChildAvatars.White)
            child = GameObject.Find("TKGirlC");
        else if (userPrefs.GetChildAvatar() == Enums.ChildAvatars.Asian)
            child = GameObject.Find("TKGirlD");
        playerObject = GameObject.Find("PlayerObject");
        transform.position = spots[0].transform.position;
        //npcIsIdle = true;
        //npcHappy = true;
        //isWalking = false;
        nextSpot = spots[0];
        atStart = true;
        //DebugLabel = GameObject.Find("DebugLabel").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        

        //npc look at child 
        if (tatrumchildbehavior.simluationOnGoing)
        {
            if (move != "walk")
            {
                Quaternion lookRotation2 = Quaternion.LookRotation((child.transform.position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation2, Time.deltaTime);
            }
            if(atStart)
                atStart = false;
        }
        else
        { //npc look at adult at start
            if (atStart)
            {
                Quaternion lookRotation2 = Quaternion.LookRotation((playerObject.transform.position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation2, Time.deltaTime);
            }
            // npc look at child at the end
            else {
                Quaternion lookRotation2 = Quaternion.LookRotation((child.transform.position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation2, Time.deltaTime);
            }
        }

        tantrumLevel = Mathf.CeilToInt(child.GetComponent<ChildState>().tantrumLevel / 20);
        //DebugLabel.text = " move is : " + move + " Anxiety level is: " + tantrumLevel;
        if (tantrumLevel >= 4) {
            if (transform.position != spots[0].transform.position && move != "walk") {
                nextSpot = spots[0];
                move = "walk";
            }
        }

        if (tatrumchildbehavior.isPlayingXylophone) {
            move = "happy";
        }
            
        
            
        if (move == "sad" && !animator.IsInTransition(0))
            animator.CrossFade("sad", 0.2f);
        else if (move == "idle2" && !animator.IsInTransition(0))
            animator.CrossFade("idle2", 0.2f);
        else if (move == "happy" && !animator.IsInTransition(0))
            animator.CrossFade("happy", 0.05f);
        else if (move == "idle" && !animator.IsInTransition(0))
            animator.CrossFade("idle", 0.2f);
        else if (move == "walk")
        {
            animator.Play("walk");
            walk();
        }
        else if (move == "idleThumbsUp" && !animator.IsInTransition(0)) {
            animator.CrossFade("idleThumbsUp", 0.4f);
        }

        



    }

    public void setMove(string m) {
        move = m;
    }


    /*public void setNPCIdle(bool state) { 
        npcIsIdle=state;
    }

    public void setNPCHappy(bool state) { 
        npcHappy=state;
    }

    public bool getNPCIdle() {
        return npcIsIdle;
    }

    public bool getNPCHappy() {
        return npcHappy;
    }*/

    public string getMove() {
        return move;
    }
    void walk() {
        Vector3 direction = nextSpot.transform.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (transform.position != nextSpot.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextSpot.transform.position, 0.6f * Time.deltaTime);
        }
        else { 
            move = "idle2"; 
        }
    }

    public void setNextSpot(int index) {
        nextSpot = spots[index];
    }
}
