using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YettyAnimation : MonoBehaviour
{
    Animator animator;

    const string YETTY_IDLE = "Yetty_Idle";
    const string YETTY_ATTACK_STICK = "Yetty_Attack_Stick";
    const string YETTY_GRAB = "Yetty_Grab";
    const string YETTY_IDLE_GRAB = "Yetty_Idle_Grab";
    const string YETTY_IDLE_STICK = "Yetty_Idle_Stick";
    const string YETTY_WALK = "Yetty_Walk";

    public string currentState;
    void Start()
    {
        currentState = YETTY_IDLE;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("animator in anim is empty");

        }
        else
        {
            Debug.Log("animator found");
        }
    }
    public void ChangeAnimationState(string newState)
    {
        Debug.Log("Changing animation state");
        // stop the animation from interrupting itself 
        if (currentState == newState) return;

        // play the animation
        animator.Play(newState);

        currentState = newState;

    }
    public void PlayYettyWalk()
    {
        ChangeAnimationState(YETTY_WALK);
    }
    public void PlayYettyAttackStick()
    {
        ChangeAnimationState(YETTY_ATTACK_STICK);
    }
    public void PlayYettyGrab()
    {
        ChangeAnimationState(YETTY_GRAB);
    }
    public void PlayYettIdleGrab()
    {
        ChangeAnimationState(YETTY_IDLE_GRAB);
    }
    public void PlayYettyIdleStick()
    {
        ChangeAnimationState(YETTY_IDLE_STICK);
    }
    public void PlayYettyIdle()
    {
        ChangeAnimationState(YETTY_IDLE);
    }
}
