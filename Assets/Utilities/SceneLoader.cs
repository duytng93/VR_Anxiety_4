using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen; // Reference to the loading screen UI (e.g., a canvas)

    // Call this method to load a scene by index asynchronously
    public void LoadSceneByIndexAsync(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private System.Collections.IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        // While the scene is loading, update the progress
        while (!asyncOperation.isDone)
        {
            // Calculate the progress as a percentage (0 to 1)
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9 is the progress when the scene is fully loaded

            // Update your loading UI here (e.g., set a loading bar fill amount or display the percentage)
            // For example, if you have a loading bar with a Slider component:
            // loadingBarSlider.value = progress;

            // You can also display the percentage in a text element:
            // loadingText.text = "Loading: " + (progress * 100f).ToString("F0") + "%";

            yield return null; // Wait for the next frame
        }

        // Hide the loading screen after the scene is fully loaded
        loadingScreen.SetActive(false);
    }
}
