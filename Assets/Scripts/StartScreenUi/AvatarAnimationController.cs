using UnityEngine;

public class AvatarAnimationController : MonoBehaviour
{
    private Animator animator;
    private Transform mainCameraTrans;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("TK_idlehappy");
        mainCameraTrans = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = mainCameraTrans.position;
        Vector3 pointBelowCamera = new Vector3(cameraPosition.x, -1f, cameraPosition.z); // floor position

        // Ensure that the boy is looking at the camera in a straight up position
        transform.rotation = Quaternion.LookRotation((pointBelowCamera - transform.position).normalized);
        
    }
}
