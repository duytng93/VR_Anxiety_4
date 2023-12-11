using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityAnimationEvent : UnityEvent<string> { };

[RequireComponent(typeof(Animator))]
public class AnimationEventDispatcher : MonoBehaviour
{
    private Animator animator;
    public UnityAnimationEvent OnAnimationStart;
    public UnityAnimationEvent OnAnimationComplete;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in animationClips)
        {
            AnimationEvent animationStartEvent = new AnimationEvent
            {
                time = 0,
                functionName = "AnimationStartHandler",
                stringParameter = clip.name
            };

            AnimationEvent animationEndEvent = new AnimationEvent
            {
                time = clip.length,
                functionName = "AnimationCompleteHandler",
                stringParameter = clip.name
            };

            clip.AddEvent(animationStartEvent);
            clip.AddEvent(animationEndEvent);
        }
    }

    private void AnimationStartHandler(string animationName)
    {
        OnAnimationStart.Invoke(animationName);
    }

    private void AnimationCompleteHandler(string animationName)
    {
        OnAnimationComplete.Invoke(animationName);
    }
}
