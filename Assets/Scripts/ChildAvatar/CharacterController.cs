using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerR : MonoBehaviour
{

    public GameObject TKBoyA;
    public GameObject TKBoyB;
    public GameObject TKBoyC;
    public GameObject TKBoyD;

    public string avatar;

    // Start is called before the first frame update
    void Start()
    {
        UserPrefs userPrefs = FindObjectOfType<UserPrefs>();

        Enums.ChildAvatars childAvatar = userPrefs.GetChildAvatar();
        /*TKBoyA = GameObject.Find("TKBoyA");
        TKBoyB = GameObject.Find("TKBoyB");
        TKBoyC = GameObject.Find("TKBoyC");
        TKBoyD = GameObject.Find("TKBoyD");*/
        avatar = userPrefs.GetChildAvatar().ToString();
        Debug.Log("Avatar: " + avatar);

        if (avatar == "Black")
        {
            TKBoyB.SetActive(true);
            TKBoyA.SetActive(false);
            TKBoyC.SetActive(false);
            TKBoyD.SetActive(false);
        }
        else if (avatar == "White")
        {
            TKBoyC.SetActive(true);
            TKBoyB.SetActive(false);
            TKBoyA.SetActive(false);
            TKBoyD.SetActive(false);
        }
        else if (avatar == "Asian")
        {
            TKBoyD.SetActive(true);
            TKBoyB.SetActive(false);
            TKBoyC.SetActive(false);
            TKBoyA.SetActive(false);

        }
        else { // Default to Hispanic
            TKBoyA.SetActive(true);
            TKBoyB.SetActive(false);
            TKBoyC.SetActive(false);
            TKBoyD.SetActive(false);
        }
    }
}