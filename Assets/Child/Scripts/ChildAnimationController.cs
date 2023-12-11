using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAnimationController : MonoBehaviour
{
    /*
     * Child Animation Controller
     * Looks at tantrum regions and tantrum level to find valid behaviors
     * Checks its override behavior
     * Enacts the override behavior or a random valid behavior if there is no override behavior
     * Behaviors
     *  If it's a stationary animation behavior, plays the animation then finds a new behavior
     *  If it's a walk behavior, disregards animation end events, sets animator in fixedUpdate and waits to reach destination, and then finds a new behavior
     * Sequence behaviors from an external source
     *  Talk to user
     *      Walk to user is set as an override behavior
     *      Once the override behavior completes we start the talk to player behavior where the child faces the player and an interaction UI is displayed.
     *      No walking is allowed during this interaction,
     *      Once the interaction completes, the UI is hidden and the child returns to the base loop
     *  Play with toy
     *      Walk to User,
     *      Ask user if they can play with a toy (Shows the UI and allows the user to pick between different dialogue lines
     *      If the Child's tantrum level is above a certain threshold, plays the throw toy tantrum sequence behavior
     *  Throw toy
     *      Walk to toy
     *      Pick up toy
     *      Throw toy in random direction
     */

    //Driven by this script
    private Animator animator;
    
    //Drives this script
    ChildController childController;

    //Plays one animation after the other in the Queue unless an overrideBehavior is active
    public List<ChildBehaviorState> validBehaviors;
    
    //Plays the override behavior then clears it.
    public ChildBehaviorEnum overrideBehavior = ChildBehaviorEnum.Null, currentBehavior = ChildBehaviorEnum.Null;

    private void Awake()
    {
        //Fill any null refs
        if (animator == null)
            animator = GetComponent<Animator>();
        if (childController == null)
            childController = GetComponent<ChildController>();
        if (validBehaviors == null)
            validBehaviors = new();
    }

    private void FixedUpdate()
    {
        //If we're walking
        if (currentBehavior == ChildBehaviorEnum.Walking)
        {
            //Tell animator to walk
            animator.SetFloat("Aspeed", childController.movementController.agent.velocity.magnitude);
    
            //check if we should stop walking
            if(ShouldStopWalking())
            {
                currentBehavior = ChildBehaviorEnum.Null;
                OnAnimationEnd();
            }
        }
    }

    //Check if we have reached our destination
    private bool ShouldStopWalking()
    {
        if (!childController.movementController.agent.pathPending)
        {
            if (childController.movementController.agent.remainingDistance <= childController.movementController.agent.stoppingDistance)
            {
                if (!childController.movementController.agent.hasPath || childController.movementController.agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Called when an animation ends
    private bool waitingAndChoosing = false;
    public void OnAnimationEnd()
    {
        //Walking is handled in Fixed Update
        if (currentBehavior == ChildBehaviorEnum.Walking) 
            return;

        if(waitingAndChoosing == false)
        {
            waitingAndChoosing = true;
            StartCoroutine("WaitAndChooseNewBehavior");
        }
    }

    private IEnumerator WaitAndChooseNewBehavior()
    {
        //Be idle for a second
        if(validBehaviors.Count > 0)
            animator.Play(validBehaviors[Random.Range(0, validBehaviors.Count - 1)].ToString(), 1);
        //Wait
        yield return new WaitForSeconds(1);
        
        //Find the next behavior
        if (overrideBehavior == ChildBehaviorEnum.Null)
        {
            ChangeBehavior(GetNewRandomBehavior());
        }
        else
        {
            ChangeBehavior(overrideBehavior);
        }

        //Play the animation
        if (animator.GetCurrentAnimatorClipInfo(1).ToString() != currentBehavior.ToString())
        {
            animator.Play(currentBehavior.ToString(), 1);
        }
        waitingAndChoosing = false;
    }

    //Returns a random behavior from validBehaviors
    public ChildBehaviorEnum GetNewRandomBehavior()
    {
        //Update what is valid behavior
        validBehaviors = childController.GetValidBehaviors();

        //Make sure we have validBehaviors
        if (validBehaviors == null || validBehaviors.Count <= 0)
            return ChildBehaviorEnum.Null;

        return validBehaviors[Random.Range(0, validBehaviors.Count)].state;
    }

    //Sets up a new behavior
    private void ChangeBehavior(ChildBehaviorEnum newBehavior)
    {
        //Keep doing the same behavior
        if (currentBehavior == newBehavior)
            return;

        //If beginning to walk
        if(newBehavior == ChildBehaviorEnum.Walking)
        {
            //Set animator to the walk cycle layer
            SetAnimationLayer(0);
            //Tell the navmesh agent to find a target and walk to it.
            childController.movementController.GoToRandomSpot();
        }
        else if(currentBehavior != ChildBehaviorEnum.Null)
        {
            //Set animator to show the stationary animations
            SetAnimationLayer(1);
            //Play the current behavior's respective animation
            animator.Play(currentBehavior.ToString(), 1);
        }

        //Update current behavior
        currentBehavior = newBehavior;
    }

    //Tells the animator which layer to use
    private void SetAnimationLayer(int layerIndex)
    {
        //Hide all layers
        animator.SetLayerWeight(0, 0);
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(2, 0);

        //Show the target layer
        animator.SetLayerWeight(layerIndex, 1);
    }
}
