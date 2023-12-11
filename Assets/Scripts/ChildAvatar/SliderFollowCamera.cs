using Meta.WitAi.TTS.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SliderFollowCamera : MonoBehaviour
{
    private Transform mainCameraTrans;
    private GameObject[] attention_tantrum_Canvas; // each child object have a "Canvas" so we need to get all of them
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
        if (tatrumchildbehavior.simluationOnGoing)
        {

            foreach (GameObject canvas in attention_tantrum_Canvas)
            {

                canvas.transform.position = mainCameraTrans.position + mainCameraTrans.rotation * new Vector3(-0.7f, 0, 2) + new Vector3(0, 0.2f, 0.3f);
                canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - mainCameraTrans.position + new Vector3(0,-0.3f,0));

                if (canvas.transform.position.y < -0.5f) // check if canvas hit the floor
                    canvas.transform.position = new Vector3(canvas.transform.position.x, -0.5f, canvas.transform.position.z);
                else if (canvas.transform.position.y > 0.6f) // check if canvas hit the ceiling
                    canvas.transform.position = new Vector3(canvas.transform.position.x, 0.6f, canvas.transform.position.z);
                if (canvas.transform.position.z < -1.3f)
                { // check if canvas hit the wall on the left
                    canvas.transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y, -1.3f);
                    freezePanelZ = true;
                }
                else freezePanelZ = false;

                if (freezeCanvasX)
                {
                    if (canvas.transform.position.x > 4.6f) // freeze canvas when panel hit the wall on the right
                        canvas.transform.position = new Vector3(4.6f, canvas.transform.position.y, canvas.transform.position.z);
                }

            }

            speakPanel.transform.position = mainCameraTrans.position + mainCameraTrans.rotation * new Vector3(0.6f, 0, 2) + new Vector3(0, -0.5f, 0.3f);
            speakPanel.transform.rotation = Quaternion.LookRotation(speakPanel.transform.position - (playerObject.transform.position + mainCameraTrans.position) / 2f);
            if (speakPanel.transform.position.y < -1.1f) // check if panel hit the floor
                speakPanel.transform.position = new Vector3(speakPanel.transform.position.x, -1.1f, speakPanel.transform.position.z);
            else if (speakPanel.transform.position.y > -0.1f) // check if panel hit the ceiling
                speakPanel.transform.position = new Vector3(speakPanel.transform.position.x, -0.1f, speakPanel.transform.position.z);

            if (speakPanel.transform.position.x > 5.8f) // check if panel hit the wall on the right
            {
                speakPanel.transform.position = new Vector3(5.8f, speakPanel.transform.position.y, speakPanel.transform.position.z);
                freezeCanvasX = true;
            }
            else freezeCanvasX = false;

            if (freezePanelZ)
            {
                if (speakPanel.transform.position.z < -0.4f) // freeze panel as well
                    speakPanel.transform.position = new Vector3(speakPanel.transform.position.x, speakPanel.transform.position.y, -0.4f);
            }
            winOrLoseCanvas.transform.position = new Vector3(0f, 3, 0); // hide during simulation
            winOrLoseShowed = false;
        }
        else {
            // hide the canvases and speak panel, show winorlose at the end
            foreach (GameObject canvas in attention_tantrum_Canvas)
            {
                canvas.transform.position = new Vector3(0f, 3, 0);
            }
            speakPanel.transform.position = new Vector3(0f, 3, 0);

            //winOrLoseCanvas.transform.position = mainCameraTrans.position + mainCameraTrans.rotation * new Vector3(0f, 0, 2) + new Vector3(0, -0.3f, 0f);
            //winOrLoseCanvas.transform.rotation = Quaternion.LookRotation(winOrLoseCanvas.transform.position - mainCameraTrans.position);
            if (!winOrLoseShowed) {
                //winOrLoseCanvas.transform.position = playerObject.transform.position + playerObject.transform.forward * 2;
                winOrLoseCanvas.transform.position = mainCameraTrans.position + mainCameraTrans.forward * 2;
                winOrLoseCanvas.transform.position = new Vector3(winOrLoseCanvas.transform.position.x, -0.25f, winOrLoseCanvas.transform.position.z);
                winOrLoseCanvas.transform.rotation = Quaternion.LookRotation(winOrLoseCanvas.transform.position - mainCameraTrans.position);
                winOrLoseShowed = true;
            }
        }

    }
}
