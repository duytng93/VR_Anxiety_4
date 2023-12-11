using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;
#endif

public class MagiLeapControl : MonoBehaviour
{
    /*
    #region Private Variables
    //private GameObject _cube;
    //private Quaternion _originalOrientation;
    //private Vector3 _rotation = new Vector3(0, 0, 0);
    //private const float _rotationSpeed = 30.0f;
    private MLInput.Controller _controller;
    public static int dif_lev = 0;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Initializes the _cube GameObject and _controller object
    /// Captures the Cube's original rotation quaternion in _originalOrientation
    /// Starts receiving user input from the Control and sets up event handlers for input
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        //_cube = GameObject.Find("Cube");
        //_originalOrientation = _cube.transform.rotation;

        MLInput.Start();
        //MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnControllerButtonUp += OnButtonUp;
        _controller = MLInput.GetController(MLInput.Hand.Left);
    }

    /// <summary>
    /// Rotates the cube by the _rotation vector at _rotationSpeed speed
    /// Checks the state of the Trigger button
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        CheckTrigger();
    }
    #endregion

    /// <summary>
    /// Updates the _rotation's y component to 0 when the Bumper button is released
    /// Resets the Cube's orientation to its original orientation, when the Home Button is pressed and released
    /// </summary>
    /// <param name="controllerId"></param>
    /// <param name="button"></param>
    void OnButtonUp(byte controllerId, MLInput.Controller.Button button)
    {
        if (button == MLInput.Controller.Button.HomeTap)
        {
            if (GazeControl.end_flag == 0 || GazeControl.end_flag == 1)
                GazeControl.end_flag = 3;
            else if (GazeControl.end_flag == 3)
                GazeControl.end_flag = 4;
        }
        if (button == MLInput.Controller.Button.Bumper)
        {
            dif_lev = dif_lev + 1;
            if (dif_lev == 4)
                dif_lev = 1;
        }
    }

    void CheckTrigger()
    {
        if (_controller.TriggerValue > 0.2f)
        {
            FillBar.dif_lev_flag = true;
        }
    }
    */
}
