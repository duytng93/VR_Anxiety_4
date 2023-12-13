
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    Animator animator;
    GameObject child, playerObject;
    private UserPrefs userPrefs;
    private string move;
    public GameObject[] spots;
    private GameObject nextSpot;
    private int tantrumLevel;
    private bool atStart;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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

        //initialize startup position
        transform.position = spots[0].transform.position;
        nextSpot = spots[0];
        atStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        

        //npc look at child when simulation is on going
        if (tatrumchildbehavior.simluationOnGoing)
        {
            // if the npc is not walking, look at the child
            if (move != "walk")
            {
                Quaternion lookRotation2 = Quaternion.LookRotation((child.transform.position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation2, Time.deltaTime);
            }
            if(atStart)
                atStart = false;
        }
        else
        { //npc look at adult at start (before simulation starts)
            if (atStart)
            {
                Quaternion lookRotation2 = Quaternion.LookRotation((playerObject.transform.position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation2, Time.deltaTime);
            }
            // npc look at child at the end when the child playing xylophone
            else {
                Quaternion lookRotation2 = Quaternion.LookRotation((child.transform.position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation2, Time.deltaTime);
            }
        }

        //convert anxiety coefficient to anxiety level
        tantrumLevel = Mathf.CeilToInt(child.GetComponent<ChildState>().tantrumLevel / 20);

        /*if (tantrumLevel >= 4) {
            //if the child's anxiety level is high, she will run to the door so the teacher has to go to the door if she is not currently at the door
            if (transform.position != spots[0].transform.position && move != "walk") {
                nextSpot = spots[0];
                move = "walk";
            }
        }*/

        //if the child is playing Xylophone, the teacher show happiness
        if (tatrumchildbehavior.isPlayingXylophone) {
            move = "happy";
        }
            
        //depend on the move variable, play correspoding animation 
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
            walk(); // change the position of the teacher over time
        }
        else if (move == "idleThumbsUp" && !animator.IsInTransition(0)) {
            animator.CrossFade("idleThumbsUp", 0.4f);
        }

        



    }

    public void setMove(string m) {
        move = m;
    }

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
