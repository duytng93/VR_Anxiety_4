using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChildAvatarAppearanceController : MonoBehaviour 
{
    private UserPrefs userPrefs;
    private GameController gameController;
    private SceneController sceneController;
    private TMPro.TMP_Dropdown dropdown;
    private TMPro.TMP_Text captionText;
    public GameObject TKBoyA, TKBoyB, TKBoyC, TKBoyD;
    void Start()
    {
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        dropdown = GameObject.Find("ChildAvatarDropdown").GetComponent<TMPro.TMP_Dropdown>();
    }

    public void SetChildAvatar()
    {
        captionText = dropdown.captionText;
        Debug.Log("Child avatar set to " + captionText.text);
        switch (captionText.text) {
            case "Hispanic":
                userPrefs.SetChildAvatar(Enums.ChildAvatars.Hispanic);
                showChildAvatar(TKBoyA);
                break;

            case "Hispano":
                userPrefs.SetChildAvatar(Enums.ChildAvatars.Hispanic);
                showChildAvatar(TKBoyA);
                break;

            case "Black or African American":
                userPrefs.SetChildAvatar(Enums.ChildAvatars.Black);
                showChildAvatar(TKBoyB);
                break;

            case "Negro o Afroamericano":
                userPrefs.SetChildAvatar(Enums.ChildAvatars.Black);
                showChildAvatar(TKBoyB);
                break;

            case "White": 
                userPrefs.SetChildAvatar(Enums.ChildAvatars.White);
                showChildAvatar(TKBoyC);
                break;

            case "Blanco":
                userPrefs.SetChildAvatar(Enums.ChildAvatars.White);
                showChildAvatar(TKBoyC);
                break;

            case "Asian":
                userPrefs.SetChildAvatar(Enums.ChildAvatars.Asian);
                showChildAvatar(TKBoyD);
                break;

            case "Asiático":
                userPrefs.SetChildAvatar(Enums.ChildAvatars.Asian);
                showChildAvatar(TKBoyD);
                break;

            default:
                Debug.Log("Invalid child avatar");
                break;
        }

        // Debug.Log("Child avatar set to " + selectedAvatar);
    }

    private void showChildAvatar(GameObject TKBoy) {
        TKBoyA.SetActive(false);
        TKBoyB.SetActive(false);
        TKBoyC.SetActive(false);
        TKBoyD.SetActive(false);
        TKBoy.SetActive(true);
    }
}