using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButtonControl : MonoBehaviour
{
    private SceneController sceneController;
    //private UserPrefs userPrefs;
    // Start is called before the first frame update
    void Start()
    {
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        //userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void enterGameplay(){
        //SceneManager.LoadScene(Enums.SceneNames.ChildScene.ToString(),LoadSceneMode.Additive);
        //SceneManager.UnloadSceneAsync(Enums.SceneNames.TutorialScene.ToString());
        sceneController.LoadScene(Enums.SceneNames.ChildScene);
        sceneController.UnloadScene(Enums.SceneNames.TutorialSceneSpanish);
        sceneController.UnloadScene(Enums.SceneNames.TutorialScene);
    }
}
