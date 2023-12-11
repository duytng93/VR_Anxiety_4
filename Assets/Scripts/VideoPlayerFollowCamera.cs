using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerFollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform mainCameraTrans;
    private Transform playerObjectTrans;
    void Start()
    {
        mainCameraTrans = GameObject.Find("Main Camera").transform;
        playerObjectTrans = GameObject.Find("PlayerObject").transform;
        transform.position = playerObjectTrans.position + playerObjectTrans.forward * 2;
        transform.position = new Vector3(transform.position.x, -0.1f, transform.position.z);
        transform.rotation = Quaternion.LookRotation(transform.position - mainCameraTrans.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*transform.position = mainCameraTrans.position + mainCameraTrans.forward*3;
        transform.position = new Vector3(transform.position.x, -0.2f, transform.position.z);
        transform.rotation = Quaternion.LookRotation(transform.position - mainCameraTrans.position);*/
    }
}
