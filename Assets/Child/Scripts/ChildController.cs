using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChildController : MonoBehaviour
{
    public ChildState childState;
    public SimulationController simulationController;
    public FaceCheck faceCheck;
    public ChildMovementController movementController;
    public EyeTrackingObjectFocuser eyeTrackingObjectFocuser;
    public ChildAnimationController childAnimationController;
    //Refs for GUI
    public FloatingBar attentionBar, tantrumBar;
    private Color attentionBarTempColor;
    //Behaviors which respond to attention and tantrum level
    public List<AttentionLevelRegion> attentionRegions;
    public List<TantrumRegion> tantrumRegions;
    public bool eyeTrackingUpdatesAttentionLevel = false;

   

    //boxed breathing UI
    public GameObject boxedbreather;
    private float boxedbreatherTimer;
    public RectTransform panelRectTransform;

    //Number of times these values are updated per second (Note: rates of change are intentionally independent)
    private float updateFrequencyPerSecond = 0.1f;

    //Begin the coroutine when this wakes up.

    private GameObject[] focusStatusTextMesh;
    private UserPrefs userPrefs;
    private void Start()
    {
        focusStatusTextMesh = GameObject.FindGameObjectsWithTag("FocusStatus");
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        boxedbreatherTimer = 0;
    }
    public void Awake()
    {
        attentionBarTempColor = new Color();
        //StartCoroutine("UpdateChild");
    }

    void Update()
    {

        //yield return new WaitForSeconds(updateFrequencyPerSecond);
        updateFrequencyPerSecond = Time.deltaTime;
        if(faceCheck.IsFacingPlayer())
        {
            childState.receivingAttention = true;
            updateFocusStatus(userPrefs.IsEnglishSpeaker() ? "You're focusing on the child" : "Estás prestando atención al niño");
            //handRenderer.material = handsMaterial;
        }
        else
        {
            childState.receivingAttention = false;
            updateFocusStatus(userPrefs.IsEnglishSpeaker() ? "You're ignoring the child" : "Estás ignorando al niño");
            //handRenderer.material = null;
        }

        //if tantrunlevel > 4 display boxed breather Ui

        if (childState.tantrumLevel > 60)
        {
            boxedbreather.SetActive(true);
            panelRectTransform.offsetMin = new Vector2(360, 0); // expand back ground panel
        }

        if (boxedbreather.activeSelf)
            boxedbreatherTimer += Time.deltaTime;

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
                /*if (simulationController.getTantrumTimeBelowTwenty() >= simulationController.getTantrumTwentyTimeLimit() && !tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel -= 10 * updateFrequencyPerSecond;*/
            }
            

            if (isWithinRange(0, 100, childState.attentionLevel + (attentionLevelChangePerSecond * updateFrequencyPerSecond)))
            {
                childState.attentionLevel += attentionLevelChangePerSecond * updateFrequencyPerSecond;
            }
            else
            {
                childState.attentionLevel = (childState.attentionLevel > 50) ? 100 : 0;
            }

            //If the attention level is too low, increase tantrum level
            if (childState.tantrumLevel < 25 && childState.attentionLevel <= 30)
            {
                childState.tantrumLevel += 3 * updateFrequencyPerSecond;
                if (childState.tantrumLevel > 25)
                    childState.tantrumLevel = 25;
            }
            

            if (childState.receivingAttention)
            {
                if (childState.tantrumLevel > 0 && tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel += region.tantrumLevelIncreasePerSecond * updateFrequencyPerSecond;
                else if (childState.tantrumLevel > 0 && !tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel -= region.tantrumLevelDecreasePerSecond * updateFrequencyPerSecond;
            }
            else
            {
                if (childState.tantrumLevel > 0 && tatrumchildbehavior.childIsTalking && !tatrumchildbehavior.negativeStatementSelected)
                    childState.tantrumLevel -= region.tantrumLevelDecreasePerSecond * updateFrequencyPerSecond;
                else if (childState.tantrumLevel > 0 && !tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel += 1 * updateFrequencyPerSecond;
            }

            

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

        //Wait for the next time step and then call the coroutine again
        //StartCoroutine("UpdateChild");

        // Update the tantrum tracker
        simulationController = GameObject.Find("SimulationController").GetComponent<SimulationController>();

        if (childState.tantrumLevel == 0 && tatrumchildbehavior.simluationOnGoing)
        {
            //Debug.Log("incrementTantrumTimeAtZero");
            simulationController.incrementTantrumTimeAtZero();
        }
        /*else if (childState.tantrumLevel <= 20 && tatrumchildbehavior.simluationOnGoing) {
            simulationController.incrementTantrumTimeBelowTwenty();
        }*/
        else if (childState.tantrumLevel > 80 && tatrumchildbehavior.simluationOnGoing)
        {
            simulationController.incrementTantrumTimeAboveEighty();
        }

        
    }

    private bool isWithinRange(float lo, float hi, float newValue)
    {
        return (newValue >= lo && newValue <= hi);
    }

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

    public bool CheckProbability(float probability)
    {
        return Random.Range(0f, 1f) < probability;
    }


    public void updateFocusStatus(string message) {
        foreach (GameObject focusStatusLabel in focusStatusTextMesh)
        {
            focusStatusLabel.GetComponent<TextMeshProUGUI>().text = message;
        }
    }
}
