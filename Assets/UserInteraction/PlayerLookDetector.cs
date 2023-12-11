using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookDetector : MonoBehaviour
{
    public Transform npcTransform;
    public Transform userHeadTransform;
    public float angleInDegrees = 45f;
    public bool isLookingAtTarget = false;
    private float epsilon = 0;

    private void Start()
    {
        epsilon = Mathf.Cos(Mathf.Deg2Rad * angleInDegrees);
        npcTransform = GameObject.Find("TKBoyA(Clone)").transform;
        userHeadTransform = GameObject.Find("Main Camera").transform;
    }

    private void FixedUpdate()
    {
        //Get a vector from the player to the NPC
        Vector3 playerToNPCLocal = (npcTransform.position - userHeadTransform.position).normalized;
        //Flatten it to the XZ plane
        playerToNPCLocal.y = 0.0f;

        //Get the player's forward vector
        Quaternion horizontalRotation = Quaternion.Euler(0f, userHeadTransform.rotation.eulerAngles.y, 0f);
        //Rotate the player's forward vector to the NPC's local space
        Vector3 playerForward = horizontalRotation * Vector3.forward;
        //Flatten it to the XZ plane
        playerForward.y = 0.0f;

        // Draw a ray from the user's head to the NPC, colored blue
        Debug.DrawRay(userHeadTransform.position, playerToNPCLocal * 10, Color.blue);
        
        // Draw a ray in the direction the user is looking, colored red
        Debug.DrawRay(userHeadTransform.position, playerForward * 10, Color.red);

        //Get the dot product of the two vectors
        float dotProduct = Vector3.Dot(playerForward, playerToNPCLocal);

        //If the dot product is greater than the cosine of the angle, we're looking at the target
        isLookingAtTarget = (dotProduct > epsilon);
    }
}
