using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCheck : MonoBehaviour
{
    private GameObject userCamera, aboveNPC; // The Meta Quest Pro headset camera
    private float allowedAngle = 30; // The allowed angle in degrees

    private void Start()
    {
        aboveNPC = GameObject.Find("AboveChildPoint");
    }
    private void Update()
    {
        // get a transform of an object above the NPC
        aboveNPC.transform.position = transform.position + new Vector3(0, 1f, 0);
    }
    // Update is called once per frame
    public bool IsFacingPlayer()
    {
        userCamera = GameObject.Find("Main Camera");

        // Direction from the user to the NPC
        Vector3 toNPC = (transform.position - userCamera.transform.position).normalized;

        // Directrion from the user to the above NPC
        Vector3 toAboveNPC = (aboveNPC.transform.position - userCamera.transform.position).normalized;

        // Direction the user is facing
        Vector3 userDirection = userCamera.transform.forward;

        // Calculate the angle between the directions
        float angle = Vector3.Angle(toNPC, userDirection);
        float angle2 = Vector3.Angle(toAboveNPC, userDirection);

        // Check if within the allowed angle
        return angle < allowedAngle | angle2< allowedAngle;
    }
}
