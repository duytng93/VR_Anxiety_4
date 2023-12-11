using UnityEngine;

/// <summary>
/// The UserPrefs class is responsible for managing and storing user preferences.
/// </summary>
public class UserPrefs : MonoBehaviour
{
    public void SetLanguage(Enums.Languages language)
    {
        PlayerPrefs.SetString("Language", language.ToString());
    }

    public Enums.Languages GetLanguage()
    {
        string storedLanguage = PlayerPrefs.GetString("Language", Enums.Languages.English.ToString());
        return (Enums.Languages)System.Enum.Parse(typeof(Enums.Languages), storedLanguage);
    }

    public bool IsEnglishSpeaker()
    {
        return GetLanguage() == Enums.Languages.English;
    }

    public void SetScenario(Enums.Scenarios scenario)
    {
        PlayerPrefs.SetString("Scenario", scenario.ToString());
    }

    public Enums.Scenarios GetScenario()
    {
        string storedScenario = PlayerPrefs.GetString("Scenario", Enums.Scenarios.Home.ToString());
        return (Enums.Scenarios)System.Enum.Parse(typeof(Enums.Scenarios), storedScenario);
    }

    public void SetChildAvatar(Enums.ChildAvatars childAvatar)
    {
        PlayerPrefs.SetString("ChildAvatar", childAvatar.ToString());
    }

    public Enums.ChildAvatars GetChildAvatar()
    {
        string storedChildAvatar = PlayerPrefs.GetString("ChildAvatar", Enums.ChildAvatars.Hispanic.ToString());
        return (Enums.ChildAvatars)System.Enum.Parse(typeof(Enums.ChildAvatars), storedChildAvatar);
    }
}
