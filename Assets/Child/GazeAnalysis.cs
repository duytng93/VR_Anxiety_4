using UnityEngine; 
using UnityEngine.XR;

public class GazeAnalysis : MonoBehaviour
{
    #region GazeTargets
    public GameObject avatarHair;
    public GameObject avatarHead;
    #endregion

    #region Colliders
    // Colliders for head of child
    private Collider _avatarHeadCollider;
    private Collider _avatarHairCollider;
    private Collider _avatarCloseIndicator;
    #endregion

   

    #region Timing
    // Time between tantrumCoefficient updates
    float sendTime;  // constant
    float sendTimer; // counter
    float countTime; // keeps track of time spent at rest or at max tantrum
    #endregion

    #region Levels
    int red;
    int yellow;
    int green;
    #endregion

    // It seems like bad practice to have these as static since the only reason is to be able to access the values from another script (i.e. ActionSystem.cs)
    public static bool placeFlag; // whether content has been placed into the scene (after instructions), only assigned in TantrumPlaceOnFloor.cs
    public static int tantrumCoefficientChange; // not specific to script, used for all scripts in child, possibly more suitable to define in ActionSystem.
    public static int endFlag; // I don't even know what to say about this. This needs to change

    EyeTrackingObjectFocuser eyeTracking;
    PlayerLookDetector playerLookDetector;

    bool reachHighLevel; // Child has reached tantrum level above 60

    private void Start()
    {
        // Get avatar colliders
        _avatarHeadCollider = avatarHead.GetComponent<MeshCollider>();
        _avatarHairCollider = avatarHair.GetComponent<SphereCollider>();

        sendTime = 2.0f;
        sendTimer = 0;

        red = 0;
        yellow = 0;
        green = 0;

        tantrumCoefficientChange = 0;

        endFlag = 0;
        countTime = 0f;

        reachHighLevel = false;
    }
    private void FixedUpdate()
    {
        // Update the head lock state 
        if (placeFlag && endFlag != 2 && endFlag != 6 && (!ChildBehavior.startPoint)) // this is removed until Action_System.cs is integrated
            CheckStates();
    }

    /// CheckStates 
    /// Switch headlock mode depending on the world mode 
    /// 
    private void CheckStates()
    {
        _avatarCloseIndicator.enabled = true;
        //if (_avatarHeadCollider.Raycast(eyeGazeRay, out hit, MaxDistance))
        if(eyeTracking.focusedObject.CompareTag("HeadCollider"))
        {
            //_indicatorMat.color = Color.red;
            red++;
        }
        //else if (_avatarHairCollider.Raycast(eyeGazeRay, out hit, MaxDistance))
        else if(eyeTracking.focusedObject.CompareTag("HairCollider"))
        {
            //_indicatorMat.color = Color.red;
            //if (Action_System.fall1_count > 0) // is this testing if child has fallen?
            //    yellow++;
            //else
            red++;
        }
        //else if (_avatarCloseIndicator.Raycast(eyeGazeRay, out hit, MaxDistance))
        //else if(raycastController.focusedObject.CompareTag("CloseIndicator"))
        else if(playerLookDetector.isLookingAtTarget)
        {
            //_indicatorMat.color = Color.yellow;
            //if (Action_System.fall1_count > 0)
            //    yellow++;
            //else
            yellow++;
        }
        else
        {
            //_indicatorMat.color = Color.green;
            //if (Action_System.fall1_count > 0)
            //    green++;
            //else
            green++;
        }

        // Perform analysis every {sendTime} seconds
        sendTimer -= Time.deltaTime;
        if (sendTimer <= 0)
        {
            sendTimer = sendTime; // reset timer

            // We want to calculate what percent of the time the user was looking in the red, yellow, and green areas
            int total = red + yellow + green;
            float epsilon = 0.6f;
            // to do list: adjust by difficult lev
            if (((float)yellow / (float)total) > epsilon)
                tantrumCoefficientChange += Random.Range(2, 5);
            else if (((float)red / (float)total) > epsilon)
                tantrumCoefficientChange += Random.Range(2, 5); // should red have higher consequences than yellow?
            else if (((float)green / (float)total) > epsilon)
                tantrumCoefficientChange -= Random.Range(4, 6);
            // no else, the above should cover every possible case

            // Reduce (but don't remove) impact of previous actions
            red /= 4;
            yellow /= 4;
            green /= 4;
        }

        /*
        What the remaining code is trying to convey:
        If the child is at max tantrum for over 5 seconds, the simulation ends

        if the child is at peace for over 1 second (after having tantrumed), the simulation ends

        If the simulation is paused, reset gaze history and timer.

        The only reason I can't rewrite this stuff yet is because it is used in Action_System.cs

        endFlag:
            0 - Child is at reduced tantrum
            1 - Child is at max tantrum
            2 - You have failed to calm child
            3 - Simulation paused
            4 - Simulation resumed (goes back to 0)
            5 - Child is at peace
            6 - You have succeeded at calming child
        */

        if ((ChildBehavior.tantrumCoefficient == 100) && (endFlag == 0))     
            endFlag = 1; // child is at max tantrum
        else if ((ChildBehavior.tantrumCoefficient < 100) && (endFlag == 1)) 
            endFlag = 0; // means child has reduced tantrum

        // Record continuous max tantrum time
        if (endFlag == 1)      
            countTime += Time.deltaTime;
        else if (endFlag == 0) 
            countTime = 0f;

        if (countTime > 5f && endFlag == 1) 
            endFlag = 2;                    // child has been at max tantrum for over 5 seconds straight

        if (endFlag == 3)                                                   // simulation paused
        {
            sendTimer = sendTime;
            red = 0;
            yellow = 0;
            green = 0;
        }

        if ((ChildBehavior.tantrumCoefficient == 0) && (endFlag == 0) && (reachHighLevel)) 
            endFlag = 5; // child has successfully stopped tantruming
        else if ((ChildBehavior.tantrumCoefficient > 0) && (endFlag == 5))                 
            endFlag = 0;

        if (endFlag == 5)
            countTime += Time.deltaTime;
        else if (endFlag == 0)
            countTime = 0f;

        if (countTime > 1f && endFlag == 5)
            endFlag = 6;

        if (!reachHighLevel && ChildBehavior.tantrumCoefficient > 60)
            reachHighLevel = true;
    }
}