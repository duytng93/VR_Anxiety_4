using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    /*
    public Slider slider;
    public Text displayText;
    public Text noteboard;
    public Image fillimage;
    public Image EmoBox;

    public static bool dif_lev_flag = false;

    private float currentValue = 0f;

    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            currentValue = value;
            slider.value = currentValue;
            displayText.text = "<color=black><size=14>" + "Tantrum Coefficient: " + "</size></color>" +
            "<color=black><size=16>" + GazeControl.tan_coe.ToString() + "\n" + "</size></color>";
            noteboard.text = "";
            if (Action_System.tan_lev == 0)
                fillimage.color = Color.gray;
            else if (Action_System.tan_lev == 1)
                fillimage.color = Color.blue;
            else if (Action_System.tan_lev == 2)
                fillimage.color = Color.green;
            else if (Action_System.tan_lev == 3)
                fillimage.color = Color.yellow;
            else if (Action_System.tan_lev == 4)
                fillimage.color = Color.red;
            else if (Action_System.tan_lev == 5)
                fillimage.color = Color.magenta;

            if (Action_System.emo_lev == 0)
                EmoBox.color = Color.green;
            else if (Action_System.emo_lev == 1)
                EmoBox.color = Color.white;
            else if (Action_System.emo_lev == 2)
                EmoBox.color = Color.red;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentValue = 0f;
        displayText.text = "<color=red><size=16>" + "Ready to Start." + "</size></color>";
        noteboard.text = "<color=white><size=12>" + "Please choose the difficulty level for the kid tantrum simulation by tap the bumper button." + "</size></color>";
    }

    // Update is called once per frame
    void Update()
    {
        if (!dif_lev_flag)
            Adjust_Dif_Lev();
        else if (dif_lev_flag && GazeControl.placeflag == 1)
        {
            if (GazeControl.end_flag == 2)
            {
                displayText.text = "<color=black><size=14>" + "The simulation has ended." + "</size></color>";
                noteboard.text = "<color=white><size=16>" + "You have reached the highest tantrum level." + "</size></color>";
            }
            else if (GazeControl.end_flag == 3)
            {
                displayText.text = "<color=black><size=12>" + "The simulation has paused." + "</size></color>";
                noteboard.text = "<color=white><size=16>" + "Please tap the home button to continue the simulation." + "</size></color>";
            }
            else if (GazeControl.end_flag == 6)
            {
                displayText.text = "<color=black><size=12>" + "The simulation has ended." + "</size></color>";
                noteboard.text = "<color=white><size=16>" + "You succesfully make the tantrum level decreased to 0." + "</size></color>";
            }
            else
                CurrentValue = ((float)GazeControl.tan_coe / 100f);
        }
        //else if (!dif_lev_flag)
        //{
        //    displayText.text = "<color=red><size=16>" + "Ready to Start" + "</size></color>";
            //noteboard.text = "<color=white><size=16>" + "You have reached the highest tantrum level." + "</size></color>";
        //}
        else if (dif_lev_flag && GazeControl.placeflag == 0)
        {
            displayText.text = "<color=red><size=16>" + "Ready to Start." + "</size></color>";
            noteboard.text = "<color=white><size=16>" + "The avatar will be placed when you focus on an empty area on the floor." + "</size></color>";
        }
    }

    //Adjust Difficult Level
    void Adjust_Dif_Lev()
    {
        if (!(MagiLeapControl.dif_lev == 0))
        {
            if (MagiLeapControl.dif_lev == 1)
            {
                noteboard.text = "<color=white><size=12>" + "The current difficulty level is\n" + 
                    "</size></color>" + "<color=green><size=20>" + "EASY\n" + "</size></color>" +
                    "<color=white><size=12>" + "Please confirm by holding the Trigger." + "</size></color>";
            }
            else if (MagiLeapControl.dif_lev == 2)
            {
                noteboard.text = "<color=white><size=12>" + "The current difficulty level is\n" +
                    "</size></color>" + "<color=yellow><size=20>" + "NORMAL\n" + "</size></color>" +
                    "<color=white><size=12>" + "Please confirm by holding the Trigger." + "</size></color>";
            }
            else if (MagiLeapControl.dif_lev == 3)
            {
                noteboard.text = "<color=white><size=12>" + "The current difficulty level is\n" +
                    "</size></color>" + "<color=red><size=20>" + "HARD\n" + "</size></color>" +
                    "<color=white><size=12>" + "Please confirm by holding the Trigger." + "</size></color>";
            }
        }
    }
    */
}