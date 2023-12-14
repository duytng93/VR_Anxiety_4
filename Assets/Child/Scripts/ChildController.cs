
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChildController : MonoBehaviour
{

    public ChildState childState;
    public SimulationController simulationController;
    public FaceCheck faceCheck;

    #region unused variables
    //have a reference from ChildAnimationController.cs but this script is not currently used
    public ChildMovementController movementController;
    public EyeTrackingObjectFocuser eyeTrackingObjectFocuser;
    public ChildAnimationController childAnimationController;
    #endregion

    //Refs for GUI
    public FloatingBar attentionBar, tantrumBar;
    public List<AttentionLevelRegion> attentionRegions;
    public List<TantrumRegion> tantrumRegions;

    //boxed breathing UI
    public GameObject boxedbreather;
    private float boxedbreatherTimer;
    public RectTransform panelRectTransform;

    //Number of times these values are updated per second
    private float updateFrequencyPerSecond;
    private GameObject[] focusStatusTextMesh;
    private UserPrefs userPrefs;
    private void Start()
    {
        focusStatusTextMesh = GameObject.FindGameObjectsWithTag("FocusStatus");
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        simulationController = GameObject.Find("SimulationController").GetComponent<SimulationController>();
        boxedbreatherTimer = 0;
        updateFrequencyPerSecond = Time.deltaTime;
    }
    
    void Update()
    {
        // check if the player is facing the child, if so change the focusStatus accordingly
        if(faceCheck.IsFacingPlayer())
        {
            childState.receivingAttention = true;
            updateFocusStatus(userPrefs.IsEnglishSpeaker() ? "You're focusing on the child" : "Estás prestando atención al niño");
        }
        else
        {
            childState.receivingAttention = false;
            updateFocusStatus(userPrefs.IsEnglishSpeaker() ? "You're ignoring the child" : "Estás ignorando al niño");
        }

        //if tantrunlevel >= 4, display boxed breather Ui
        if (childState.tantrumLevel > 60)
        {
            boxedbreather.SetActive(true);
            panelRectTransform.offsetMin = new Vector2(360, 0); // expand back ground panel
        }

        //if boxedbreather is showing, count the time
        if (boxedbreather.activeSelf)
            boxedbreatherTimer += Time.deltaTime;

        //if boxedbreather is showing more than 16s and the anxiety level is low, hide the boxedbreather
        if (boxedbreatherTimer > 16f && childState.tantrumLevel < 60)
        {
            boxedbreather.SetActive(false);
            panelRectTransform.offsetMin = new Vector2(360, 190); // collapse background panel
            boxedbreatherTimer = 0; //reset timer
        }


        //Integrate attention level into tantrum level
        foreach (var region in attentionRegions)
        {
            //If we are not in this region's bounds, skip it
            if (!isWithinRange(region.activatePercentageLoBound, region.activatePercentageHiBound, childState.attentionLevel))
                continue;

            //Update attention level
            float attentionLevelChangePerSecond = 0;
            if (tatrumchildbehavior.simluationOnGoing) {
                if (childState.receivingAttention)
                {
                    attentionLevelChangePerSecond = region.attentionLevelAddedPerSecond;
                }
                else
                {
                    attentionLevelChangePerSecond = -region.attentionLevelLostPerSecond;
                }
            }
            
            if (isWithinRange(0, 100, childState.attentionLevel + (attentionLevelChangePerSecond * updateFrequencyPerSecond)))
            {
                childState.attentionLevel += attentionLevelChangePerSecond * updateFrequencyPerSecond;
            }
            else
            {
                childState.attentionLevel = (childState.attentionLevel > 50) ? 100 : 0;
            }

            //If the attention level is too low, increase anxiety level
            if (childState.tantrumLevel < 25 && childState.attentionLevel <= 30)
            {
                childState.tantrumLevel += 3 * updateFrequencyPerSecond;
                if (childState.tantrumLevel > 25)
                    childState.tantrumLevel = 25;
            }
            
            //update anxiety level (the rate of increase or decrease is controlled by tantrum regions
            if (childState.receivingAttention)
            {   //if the child receives attention while showing negative behaviors => anxiety level increased
                if (childState.tantrumLevel > 0 && tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel += region.tantrumLevelIncreasePerSecond * updateFrequencyPerSecond;
                //if the child receives attention while showing calm behaviors => anxiety level decreased
                else if (childState.tantrumLevel > 0 && !tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel -= region.tantrumLevelDecreasePerSecond * updateFrequencyPerSecond;
            }
            else
            {
                //if the child receives NO attention while showing negative behaviors => anxiety level decreased unless the player just chose a negative statement
                if (childState.tantrumLevel > 0 && tatrumchildbehavior.childIsTalking && !tatrumchildbehavior.negativeStatementSelected)
                    childState.tantrumLevel -= region.tantrumLevelDecreasePerSecond * updateFrequencyPerSecond;
                //if the child receives NO attention while showing calm behavior => anxiety level increased
                else if (childState.tantrumLevel > 0 && !tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel += 1 * updateFrequencyPerSecond;
            }

            
            // limit the anxiety level between 0 and 100
            if (childState.tantrumLevel > 100)
                childState.tantrumLevel = 100;
            else if (childState.tantrumLevel < 0)
                childState.tantrumLevel = 0;

            //Update AttentionBar color
            attentionBar.barImage.color = region.barColor;

            //Do not check other regions
            break;
        }

        //Respond to tantrum level
        foreach (var region in tantrumRegions)
        {
            //If we are not in this region's bounds, skip it
            if (!isWithinRange(region.activatePercentageLoBound, region.activatePercentageHiBound, childState.tantrumLevel))
                continue;

            tantrumBar.barImage.color = region.barColor;

            //Do not check other regions
            break;
        }

        //Update the GUI
        if (attentionBar != null)
            attentionBar.UpdateBar(childState.attentionLevel, 100);
        if (tantrumBar != null)
            tantrumBar.UpdateBar(childState.tantrumLevel, 100);

        //count the time anxiety level at zero to announce win
        if (childState.tantrumLevel == 0 && tatrumchildbehavior.simluationOnGoing)
        {
            simulationController.incrementTantrumTimeAtZero();
        }
        //count the time anxiety level at zero to determin lose
        else if (childState.tantrumLevel > 80 && tatrumchildbehavior.simluationOnGoing)
        {
            simulationController.incrementTantrumTimeAboveEighty();
        }

        
    }

    private bool isWithinRange(float lo, float hi, float newValue)
    {
        return (newValue >= lo && newValue <= hi);
    }

    #region unused function
    //have a reference from ChildAnimationController.cs but this script is not currently used
    public List<ChildBehaviorState> GetValidBehaviors()
    {
        List<ChildBehaviorState> result = new();

        foreach (var region in tantrumRegions)
        {
            //If we are not in this region's bounds, skip it
            if (!isWithinRange(region.activatePercentageLoBound, region.activatePercentageHiBound, childState.tantrumLevel))
                continue;

            foreach (ChildBehaviorState behavior in region.validBehaviors)
                result.Add(behavior);
        }

        return result;
    }
    #endregion

    public void updateFocusStatus(string message) {
        foreach (GameObject focusStatusLabel in focusStatusTextMesh)
        {
            focusStatusLabel.GetComponent<TextMeshProUGUI>().text = message;
        }
    }
}
