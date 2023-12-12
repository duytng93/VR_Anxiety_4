
using UnityEngine;

public class SliderFollowCamera : MonoBehaviour
{
    private Transform mainCameraTrans;
    private GameObject[] attention_tantrum_Canvas; 
    private GameObject speakPanel;
    private GameObject playerObject;
    private bool freezeCanvasX;
    private bool freezePanelZ;
    private bool winOrLoseShowed;
    private GameObject winOrLoseCanvas;
    // Start is called before the first frame update
    void Start()
    {
        mainCameraTrans = GameObject.Find("Main Camera").transform;
        attention_tantrum_Canvas = GameObject.FindGameObjectsWithTag("ChildTantrumAttentionCanvas");
        speakPanel = GameObject.Find("SpeakPanel");
        playerObject = GameObject.Find("PlayerObject");
        winOrLoseCanvas = GameObject.Find("WinOrLoseCanvas");

    }

    private void Awake()
    {
        winOrLoseShowed = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if simulation is on then show anxiety meters and speak panel, and make them follow the camera
        // and hide the winorlose status canvas
        if (tatrumchildbehavior.simluationOnGoing)
        {
            foreach (GameObject canvas in attention_tantrum_Canvas)
            {
                //make the meters canvas follow the camera
                canvas.transform.position = mainCameraTrans.position + mainCameraTrans.rotation * new Vector3(-0.8f, 0, 2) + new Vector3(0, 0.2f, 0.3f);
                canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - mainCameraTrans.position + new Vector3(0,-0.3f,0));

                // check if canvas hit the floor. if so freeze it
                if (canvas.transform.position.y < -0.5f) 
                    canvas.transform.position = new Vector3(canvas.transform.position.x, -0.5f, canvas.transform.position.z);
                // check if canvas hit the ceiling. if so freeze it
                else if (canvas.transform.position.y > 0.6f) 
                    canvas.transform.position = new Vector3(canvas.transform.position.x, 0.6f, canvas.transform.position.z);

                // check if canvas hit the wall on the left. if so, if so freeze it and set flag to freeze the speak panel as well
                if (canvas.transform.position.z < -1.3f)
                { 
                    canvas.transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y, -1.3f);
                    freezePanelZ = true;
                }
                else freezePanelZ = false;

                // try to freeze the meters canvas if the flag is set to true
                if (freezeCanvasX)
                {
                    if (canvas.transform.position.x > 4.6f) // freeze canvas when panel hit the wall on the right
                        canvas.transform.position = new Vector3(4.6f, canvas.transform.position.y, canvas.transform.position.z);
                }

            }

            //make the speak panel follow the camera
            speakPanel.transform.position = mainCameraTrans.position + mainCameraTrans.rotation * new Vector3(0.6f, 0, 2) + new Vector3(0, -0.5f, 0.3f);
            speakPanel.transform.rotation = Quaternion.LookRotation(speakPanel.transform.position - (playerObject.transform.position + mainCameraTrans.position) / 2f);

            // check if panel hit the floor. if so freeze it
            if (speakPanel.transform.position.y < -1.1f) 
                speakPanel.transform.position = new Vector3(speakPanel.transform.position.x, -1.1f, speakPanel.transform.position.z);
            // check if panel hit the ceiling. if so freeze it
            else if (speakPanel.transform.position.y > -0.1f) 
                speakPanel.transform.position = new Vector3(speakPanel.transform.position.x, -0.1f, speakPanel.transform.position.z);
            // check if panel hit the wall on the right. if so freeze it and set the flag to freeze the meters canvas as well 
            if (speakPanel.transform.position.x > 5.8f) 
            {
                speakPanel.transform.position = new Vector3(5.8f, speakPanel.transform.position.y, speakPanel.transform.position.z);
                freezeCanvasX = true;
            }
            else freezeCanvasX = false;

            // try to freeze the speak panel if the flag is set to true
            if (freezePanelZ)
            {
                if (speakPanel.transform.position.z < -0.4f) // freeze panel as well
                    speakPanel.transform.position = new Vector3(speakPanel.transform.position.x, speakPanel.transform.position.y, -0.4f);
            }

            // hide winorlose status during simulation
            winOrLoseCanvas.transform.position = new Vector3(0f, 3, 0); 
            winOrLoseShowed = false;
        }

        // if not during simulation, hide the canvases and speak panel, show winorlose
        else
        {
            //hide the canvases and speak panel
            foreach (GameObject canvas in attention_tantrum_Canvas)
            {
                canvas.transform.position = new Vector3(0f, 3, 0);
            }
            speakPanel.transform.position = new Vector3(0f, 3, 0);

            //show winorlose
            if (!winOrLoseShowed) {
                winOrLoseCanvas.transform.position = mainCameraTrans.position + mainCameraTrans.forward * 2;
                winOrLoseCanvas.transform.position = new Vector3(winOrLoseCanvas.transform.position.x, -0.25f, winOrLoseCanvas.transform.position.z);
                winOrLoseCanvas.transform.rotation = Quaternion.LookRotation(winOrLoseCanvas.transform.position - mainCameraTrans.position);
                winOrLoseShowed = true;
            }
        }

    }
}
