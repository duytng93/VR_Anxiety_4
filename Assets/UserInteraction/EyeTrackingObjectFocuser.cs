using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(OVREyeGaze))]
public class EyeTrackingObjectFocuser : MonoBehaviour
{
    public Transform eyeTransform; // Reference to the head transform
    public float raycastDistance = 10f; // Maximum raycast distance
    public float sphereRadius = 0.1f; // Radius of the sphere gizmo
    public GameObject focusedObject;
    OVREyeGaze eyeGaze;
    public UnityEvent OnNewObjectFocused;

    void Start()
    {
        eyeGaze = GetComponent<OVREyeGaze>();
    }

    private void Update()
    {
        if (eyeGaze == null) return;

        if (eyeGaze.EyeTrackingEnabled)
        {
            // Get the eye position and rotation
            Vector3 eyePosition = eyeTransform.position;
            Quaternion eyeRotation = eyeGaze.transform.rotation;

            // Cast a ray from the eye position into the world
            RaycastHit hit;
            if (Physics.Raycast(eyePosition, eyeRotation * Vector3.forward, out hit, raycastDistance))
            {
                // Debug.Log("Hit: " + hit.collider.gameObject.name); // Uncomment this line to print the hit object's name
                // Do something with the hit object, such as storing a reference to it
                if (focusedObject != hit.collider.gameObject)
                    OnNewObjectFocused.Invoke();
                focusedObject = hit.collider.gameObject;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (eyeGaze == null) return;
        // Draw the ray as a debug visual gizmo
        Vector3 eyePosition = eyeTransform.position;
        Quaternion eyeRotation = eyeGaze.transform.rotation;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(eyePosition, eyeRotation * Vector3.forward * raycastDistance);

        // Cast a ray from the eye position into the world and draw a sphere gizmo at the collision point
        RaycastHit hit;
        if (Physics.Raycast(eyePosition, eyeRotation * Vector3.forward, out hit, raycastDistance))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hit.point, sphereRadius);
        }
    }
}
