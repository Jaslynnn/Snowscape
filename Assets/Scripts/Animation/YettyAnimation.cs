using System.Collections;
using UnityEngine;

public class YettyAnimation : MonoBehaviour
{
    Animator animator;

    // Animation state names
    const string YETTY_IDLE = "Yetty_Idle";
    const string YETTY_ATTACK_STICK = "Yetty_Attack_Stick";
    const string YETTY_GRAB = "Yetty_Grab";
    const string YETTY_IDLE_GRAB = "Yetty_Idle_Grab";
    const string YETTY_IDLE_STICK = "Yetty_Idle_Stick";
    const string YETTY_WALK = "Yetty_Walk";
    const string YETTY_DEAD = "Yetty_Dead";
    const string YETTY_MOVEMENT = "Movement"; // Default state

    public string currentState;
    public AudioManager audioManager;

    // Sound names 
    const string YETTY_WOOSH = "Woosh";
    const string YETTY_FOOTSTEPS = "YettyFootstep";
    const string YETTY_LOSE = "Lose";

    public UIManager uiManager;
    private bool isYettyStepsPlaying = false;
    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
            return;
        }

        currentState = YETTY_MOVEMENT;
        ChangeAnimationState(YETTY_MOVEMENT);
    }

    public void UpdateMovementSpeed(float speed)
    {
        animator.SetFloat("speed", speed);
        //Debug.Log("Yetty speed:" + speed);
        if (speed > 0.05f && !isYettyStepsPlaying)
        {
            audioManager.Play(YETTY_FOOTSTEPS);
            //Debug.Log("Playing YettyFootsteps");
            isYettyStepsPlaying = true;
        }
        else if (speed <=0.5f && isYettyStepsPlaying)
        {
            isYettyStepsPlaying = false;        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (animator == null) return;

        // Stop the animation from interrupting itself
        if (currentState == newState) return;

        // Check if the state exists in the Animator
        if (!AnimatorHasState(newState))
        {
            Debug.LogError($"Animation state '{newState}' not found in Animator Controller!");
            return;
        }

        // Play the animation
        animator.Play(newState);
        currentState = newState;
    }


    public void PlayYettyAttackStick()
    {
        audioManager.Play(YETTY_WOOSH);
        ChangeAnimationState(YETTY_ATTACK_STICK);
        StartCoroutine(ReturnToMovementDefault(YETTY_ATTACK_STICK));
    }

    public void PlayYettyGrab()
    {
        ChangeAnimationState(YETTY_GRAB);
        StartCoroutine(ReturnToMovementDefault(YETTY_GRAB));
    }

    public void PlayYettIdleGrab()
    {
        ChangeAnimationState(YETTY_IDLE_GRAB);
        StartCoroutine(ReturnToMovementDefault(YETTY_IDLE_GRAB));
    }

    public void PlayYettyDead()
    {
        Debug.Log("1. Playing YettyLose from PlayYettyDead");
        audioManager.Play(YETTY_LOSE);
        //ChangeAnimationState(YETTY_DEAD);
    }

    private IEnumerator ReturnToMovementDefault(string currentAnimation)
    {
        yield return new WaitForSeconds(GetAnimationClipLength(currentAnimation));
        ResetToMovementState();
    }

    public void ResetToMovementState()
    {
        ChangeAnimationState(YETTY_MOVEMENT);
    }



    private float GetAnimationClipLength(string animationName)
    {
        if (animator == null) return 1f;

        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == animationName)
                return clip.length;
        }

        Debug.LogWarning($"Animation clip '{animationName}' not found. Using default duration of 1s.");
        return 1f; // Default duration if not found
    }

    private bool AnimatorHasState(string stateName)
    {
        return animator.HasState(0, Animator.StringToHash(stateName));
    }

    public IEnumerator HandleYettyDeath()
    {
        Debug.Log(" 2. Handling Yetty Death");
        ChangeAnimationState(YETTY_DEAD);

        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration + 3f);
        Debug.Log("3. 3secs later");

        uiManager.CallGameOverCanvas();
    }


}
