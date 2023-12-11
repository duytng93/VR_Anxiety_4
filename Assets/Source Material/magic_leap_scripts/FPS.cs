using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FPS : MonoBehaviour
{
    private float _hudRefreshRate = 1f;
    public Text display_Text;
    private float _timer;
    private string fileName = "/tmp/FPSCount.txt";

    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int avgFrameRate = (int)(1f / Time.unscaledDeltaTime);

            display_Text.text = avgFrameRate.ToString() + " FPS";
            _timer = Time.unscaledTime + _hudRefreshRate;
            StreamWriter writer = new StreamWriter(fileName, true);
            writer.WriteLine(avgFrameRate.ToString());
            writer.Close();
        }
    }
}