using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XylophoneManager : MonoBehaviour
{
    public GameObject key1;
    public GameObject key2;
    public GameObject key3;
    public GameObject key4;
    public GameObject key5;
    public GameObject key6;

    private AudioSource audKey1;
    private AudioSource audKey2;
    private AudioSource audKey3;
    private AudioSource audKey4;
    private AudioSource audKey5;
    private AudioSource audKey6;
    private Animator childAnimator;
    private UserPrefs userPrefs;
    // Start is called before the first frame update
    void Start()
    {

        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        audKey1 = key1.GetComponent<AudioSource>();
        audKey2 = key2.GetComponent<AudioSource>();
        audKey3 = key3.GetComponent<AudioSource>();
        audKey4 = key4.GetComponent<AudioSource>();
        audKey5 = key5.GetComponent<AudioSource>();
        audKey6 = key6.GetComponent<AudioSource>();

        switch (userPrefs.GetChildAvatar())
        {
            case Enums.ChildAvatars.Hispanic:
                childAnimator = GameObject.Find("TKGirlA").GetComponent<Animator>();
                break;
            case Enums.ChildAvatars.Black:
                childAnimator = GameObject.Find("TKGirlB").GetComponent<Animator>();
                break;
            case Enums.ChildAvatars.White:
                childAnimator = GameObject.Find("TKGirlC").GetComponent<Animator>();
                break;
            case Enums.ChildAvatars.Asian:
                childAnimator = GameObject.Find("TKGirlD").GetComponent<Animator>();
                break;
        }

        //StartCoroutine(PlayMaryHadALittleLamb());
    }

    public void kidPlayXylophone() {
        StartCoroutine(PlayMaryHadALittleLamb());
    }

     public IEnumerator PlayMaryHadALittleLamb()
    {
        if (!childAnimator.IsInTransition(0)) {
            childAnimator.Play("grab");
            yield return new WaitForSeconds(2f);
        }
            
        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey1.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.6f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.6f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey5.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey5.Play();

        yield return new WaitForSeconds(0.6f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey1.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey3.Play();

        yield return new WaitForSeconds(0.1f);

        playLeftHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey2.Play();

        yield return new WaitForSeconds(0.1f);

        playRightHandAnimation();
        yield return new WaitForSeconds(0.4f);
        audKey1.Play();

        yield return new WaitForSeconds(2f);
        playIdle();

    }

    public void playLeftHandAnimation()
    {
        //if (!animator.IsInTransition(0))
        childAnimator.CrossFade("lefthandplay", 0.05f);
    }
    public void playRightHandAnimation()
    {
        //if (!animator.IsInTransition(0))
        childAnimator.CrossFade("righthandplay", 0.05f);
    }

    public void playIdle()
    {
        if (!childAnimator.IsInTransition(0))
            childAnimator.CrossFade("doneplay", 0.05f);
    }

}
