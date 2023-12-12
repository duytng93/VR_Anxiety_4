using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The SimulationController class is responsible for managing the simulation end states.
/// </summary>
public class SimulationController : MonoBehaviour
{
    private SceneController sceneController;
    private UserPrefs userPrefs;
    private SimulationLanguageUpdater simulationLanguageUpdater;

    private float elapsedTime;
    private float tantrumTimeAtZero = 0.0f;
    private float tantrumTimeAboveEighty = 0.0f;

    private float gameTimeLimit = 300.0f; // 5 minutes
    private float tantrumEightyTimeLimit = 30.0f; 

    public AudioSource audioSource;
    public AudioClip EndingCheeringSound;
    public Button resetButton;
    public TextMeshProUGUI winOrLoseStatus;

    void Start()
    {

        // Find the SceneController and UserPrefs objects and get their scripts
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        simulationLanguageUpdater = GameObject.Find("SimulationLanguageUpdater").GetComponent<SimulationLanguageUpdater>();

        // Track the elapsed time
        elapsedTime = 0.0f;
    }


    void Update()
    {
        
        if (tatrumchildbehavior.simluationOnGoing)
            resetButton.interactable = true;
        else resetButton.interactable = false;

        if (elapsedTime == 0.0f)
        {
            // We only want to run this once
            simulationLanguageUpdater.UpdateSimulationText();
        }

        // Increment the elapsed time
        if (tatrumchildbehavior.simluationOnGoing)
            elapsedTime += Time.deltaTime;

        if (tantrumTimeAtZero > 2 && !button1behavior.adultIsSpeaking)
        {
            resetTimer();
            winOrLoseStatus.text = userPrefs.IsEnglishSpeaker() ? "You did it!! The child is calm now!!! :)" : "¡¡Lo hiciste!! ¡¡¡El niño ya está tranquilo!!! :)";
            StartCoroutine(playAudioandEndGameWin(EndingCheeringSound));
        }
        else if (tantrumTimeAboveEighty > tantrumEightyTimeLimit || elapsedTime > gameTimeLimit)
        {
            resetTimer();
            winOrLoseStatus.text = userPrefs.IsEnglishSpeaker() ? "Let's try again! :(" : "¡Intentemoslo de nuevo! :(";
            StartCoroutine(showEndGameLose());
        }

    }

    public void incrementTantrumTimeAtZero()
    {
        tantrumTimeAtZero += Time.deltaTime;
    }

    public void incrementTantrumTimeAboveEighty()
    {
        tantrumTimeAboveEighty += Time.deltaTime;
    }


    public void resetTimer()
    {
        elapsedTime = 0.0f;
        tantrumTimeAtZero = 0.0f;
        tantrumTimeAboveEighty = 0.0f;
        tatrumchildbehavior.simluationOnGoing = false;
    }

    IEnumerator playAudioandEndGameWin(AudioClip clip)
    {
        audioSource.clip = clip;
        yield return new WaitForSeconds(23f); // the time kid play xylophone
        audioSource.Play();
        yield return new WaitForSeconds(5f);
        sceneController.UnloadScene(Enums.SceneNames.ChildScene);
        if (userPrefs.IsEnglishSpeaker())
            sceneController.LoadScene(Enums.SceneNames.EndSceneWin);
        else sceneController.LoadScene(Enums.SceneNames.EndSceneWinSpanish);
    }

    IEnumerator showEndGameLose()
    {
        yield return new WaitForSeconds(5f);
        sceneController.UnloadScene(Enums.SceneNames.ChildScene);
        if (userPrefs.IsEnglishSpeaker())
            sceneController.LoadScene(Enums.SceneNames.EndSceneLose);
        else sceneController.LoadScene(Enums.SceneNames.EndSceneLoseSpanish);
    }

    
}