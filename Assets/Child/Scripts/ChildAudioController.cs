using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAudioController : MonoBehaviour
{
    private ChildController childControl;
    private AudioSource audioSource;
    public string audioFolderName = "Audio";
    public List<AudioClip> audioClips = new List<AudioClip>();
    public float secondsBetweenVocalizations = 3.0f;
    public static bool childIsTalking;
    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (childControl == null)
            childControl = GetComponent<ChildController>();
        LoadAudioClips();
        StartCoroutine("Vocalize");
        childIsTalking = audioSource.isPlaying;
    }

    private void Update()
    {
        childIsTalking = audioSource.isPlaying;
    }

    private IEnumerator Vocalize()
    {
        yield return new WaitForSeconds(secondsBetweenVocalizations);
        if(audioSource.isPlaying == false)
        {
            string newClip = "";
            foreach (var region in childControl.tantrumRegions)
            {
                //If we are not in this region's bounds, skip it
                if (!isWithinRange(region.activatePercentageLoBound, region.activatePercentageHiBound, childControl.childState.tantrumLevel))
                    continue;

                if (region.utterances.Count > 0)
                {
                   
                    AudioClip clip = region.utterances[Random.Range(0, region.utterances.Count)];
                    audioSource.PlayOneShot(clip);
                    yield return new WaitForSeconds(clip.length);
                    
                }
            }
        }
        StartCoroutine("Vocalize");
    }

    void LoadAudioClips()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(audioFolderName);

        if (clips != null && clips.Length > 0)
        {
            audioClips.AddRange(clips);
            Debug.Log("Loaded " + clips.Length + " audio clips from the " + audioFolderName + " folder.");
        }
        else
        {
            Debug.LogWarning("No audio clips found in the " + audioFolderName + " folder.");
        }
    }

    public void PlayClip(string clipName)
    {
        foreach(AudioClip clip in audioClips)
        {
            if(clip.name == clipName)
            {
                audioSource.PlayOneShot(clip);
                return;
            }
        }
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private bool isWithinRange(float lo, float hi, float newValue)
    {
        return (newValue >= lo && newValue <= hi);
    }
}
