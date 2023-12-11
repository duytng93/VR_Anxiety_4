using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class resetButton : MonoBehaviour
{
    private SceneController sceneController;
    private UserPrefs userPrefs;
    private Boolean confirmed;
    public TextMeshProUGUI warningMessage;
    public SimulationController simControl;
    private GameObject PlayerObject;
    private GameObject PlayerOriginPosition;
    // Start is called before the first frame update

    private void Start()
    {
        // Find the SceneController and UserPrefs objects and get their scripts
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        PlayerObject = GameObject.Find("PlayerObject");
        PlayerOriginPosition = GameObject.Find("PlayerOriginPosition");

        confirmed = false;
        warningMessage.enabled = false;
        
    }
    // Update is called once per frame
    public void resetSimulation()
    {
        if (!confirmed)
        {
            confirmed = true;
            warningMessage.enabled = true;
            StartCoroutine(resetConfirmAfter3s());
        }
        else {
           
            if(simControl != null)
                simControl.resetTimer();

            // Load the necessary scenes
            sceneController.UnloadScene(Enums.SceneNames.ChildScene);
            sceneController.LoadScene(Enums.SceneNames.ChildScene);
            /*sceneController.UnloadScene(Enums.SceneNames.EndSceneWin);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneLose);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneLoseSpanish);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneWinSpanish);
            sceneController.LoadScene(Enums.SceneNames.StartScene);
            sceneController.ToggleBackgroundScene(0); // load School, unload others*/


            // Set the defaults
            /*userPrefs.SetScenario(Enums.Scenarios.School);
            userPrefs.SetChildAvatar(Enums.ChildAvatars.Hispanic);
            userPrefs.SetLanguage(Enums.Languages.English);*/

            PlayerObject.transform.position = PlayerOriginPosition.transform.position;
            PlayerObject.transform.rotation = PlayerOriginPosition.transform.rotation;


        }
        
    }

    IEnumerator resetConfirmAfter3s() {
        yield return new WaitForSeconds(3f);
        confirmed = false;
        warningMessage.enabled = false;
    }
}
