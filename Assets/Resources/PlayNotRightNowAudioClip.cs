using UnityEngine;

public class PlayNotRightNowAudioClip : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip notRightNowAudioClip;

    public void PlayNotRightNow()
    {
        if (audioSource != null && notRightNowAudioClip != null)
        {
            audioSource.clip = notRightNowAudioClip;
            audioSource.Play();
        }
    }
}
