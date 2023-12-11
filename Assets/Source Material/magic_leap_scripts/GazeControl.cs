using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI;
#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;
#endif

public class GazeControl : MonoBehaviour
{
    /*
    #region Public Variables
    public GameObject indicator;
    public GameObject avatarHair;
    public GameObject avatarHead;
    public Camera mainCamera;
    public GameObject gazePointer;
    //public GameObject GoReceiveObj;
    public AudioSource cameravoice;
    public AudioClip StartVoice;
    #endregion

    #region Private Variables
    private Collider _avatarHeadCollider;
    private Collider _avatarHairCollider;
    private Collider _avatarCloseIndicator;
    private Material _indicatorMat;
    private const float MaxDistance = 10.0f;
    #endregion
    public static int placeflag;
    public static int tan_coe;
    public static int end_flag;
    float sendTime;
    float sendTimer;
    float countTime;
    int red;
    int yellow;
    int green;
    int total;

    bool reachhighlev;

    #region Unity Methods 
    private void Start()
    {
        // Get colliders of avatar 
        _avatarHeadCollider = avatarHead.GetComponent<MeshCollider>();
        _avatarHairCollider = avatarHair.GetComponent<SphereCollider>();
        _avatarCloseIndicator = indicator.GetComponent<SphereCollider>();
        // Get Indicator Material
        _indicatorMat = indicator.GetComponent<MeshRenderer>().material;
        // Setup and Start Magic Leap eye tracking 
        MLEyes.Start();
        // Send Coe Update Time
        sendTimer = 0;
        sendTime = 2.0f;
        red = 0;
        yellow = 0;
        green = 0;
        total = 0;
        tan_coe = 60; // inital value
        end_flag = 0;
        countTime = 0f;

        reachhighlev = false;
        //Play Start Voice
        cameravoice.clip = StartVoice;
        cameravoice.Play();
    }
    private void Update()
    {
        // Update the head lock state 
        if ((placeflag == 1) && (!(end_flag == 2) || !(end_flag == 6)) && (!Action_System.startpoint))
        {
            CheckStates();
        }
        //CheckStates();
    }
    private void OnDestroy()
    {
        MLEyes.Stop();
    }
    #endregion

    #region Private Methods

    /// CheckStates 
    /// Switch headlock mode depending on the world mode 
    /// 
    private void CheckStates()
    {
        //Debug.Log(gazePointer.position);
        var startingPoint = mainCamera.transform.position;
        var direction = MLEyes.FixationPoint - startingPoint;
        var eyeGazeRay = new Ray(startingPoint, direction);

        RaycastHit hit;
        _avatarCloseIndicator.enabled = !_avatarCloseIndicator.enabled;
        if (Physics.Raycast(startingPoint, direction, out hit, MaxDistance))
        {
            gazePointer.transform.position = hit.point;
        }
        else
        {
            gazePointer.transform.position = startingPoint + direction * 2.0f;
        }

        _avatarCloseIndicator.enabled = !_avatarCloseIndicator.enabled;
        if (_avatarHeadCollider.Raycast(eyeGazeRay, out hit, MaxDistance))
        {
            _indicatorMat.color = Color.red;
            red = red + 1;
        }
        else if (_avatarHairCollider.Raycast(eyeGazeRay, out hit, MaxDistance))
        {
            _indicatorMat.color = Color.red;
            //if (Action_System.fall1_count > 0)
            //    yellow = yellow + 1;
            //else
            red = red + 1;
        }
        else if (_avatarCloseIndicator.Raycast(eyeGazeRay, out hit, MaxDistance))
        {
            _indicatorMat.color = Color.yellow;
            //if (Action_System.fall1_count > 0)
            //    yellow = yellow + 1;
            //else
            yellow = yellow + 1;
        }
        else
        {
            _indicatorMat.color = Color.green;
            //if (Action_System.fall1_count > 0)
            //    green = green + 1;
            //else
            green = green + 1;
        }
        if (sendTimer > 0)
            sendTimer -= Time.deltaTime;
        if (sendTimer < 0)
            sendTimer = 0;
        if (sendTimer == 0)
        {
            total = red + yellow + green;
            // to do list: adjust by difficult lev
            if (((float)yellow / (float)total) > 0.6)
            {
                tan_coe = tan_coe + Random.Range(2, 5);
            }
            else if (((float)red / (float)total) > 0.6)
            {
                tan_coe = tan_coe + Random.Range(2, 5);
            }
            else if (((float)green / (float)total) > 0.6)
            {
                tan_coe = tan_coe - Random.Range(4, 6);
            }
            if (tan_coe > 100)
                tan_coe = 100;
            if (tan_coe < 0)
                tan_coe = 0;
            red = red / 4;
            yellow = yellow / 4;
            green = green / 4;
            sendTimer = sendTime;
        }
        if ((tan_coe == 100) && (end_flag == 0))
            end_flag = 1;
        else if ((tan_coe < 100) && (end_flag == 1))
            end_flag = 0;
        if (end_flag == 1)
            countTime += Time.deltaTime;
        else if (end_flag == 0)
            countTime = 0f;
        if (countTime > 5f && end_flag == 1)
            end_flag = 2;
        if (end_flag == 3)
        {
            sendTimer = sendTime;
            red = 0;
            yellow = 0;
            green = 0;
        }
        if ((tan_coe == 0) && (end_flag == 0) && (reachhighlev))
            end_flag = 5;
        else if ((tan_coe > 0) && (end_flag == 5))
            end_flag = 0;
        if (end_flag == 5)
            countTime += Time.deltaTime;
        else if (end_flag == 0)
            countTime = 0f;
        if (countTime > 1f && end_flag == 5)
            end_flag = 6;
        if (!reachhighlev && tan_coe > 60)
            reachhighlev = true;
    }
    /*void Updatetan_coe()
    {
        tan_coe = 90;
        gameObject.SendMessage("Tan_Coe_Exchange", tan_coe);
    #endregion
    }*/
}
