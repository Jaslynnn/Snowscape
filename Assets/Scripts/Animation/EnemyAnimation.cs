using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator;

    public string currentState;

    //animation states 
    const string FIEND_IDLE = "Fiend_Idle";
    const string FIEND_WALK = "Fiend_Walk";
    const string FIEND_CHASE = "Fiend_Chase";
    const string FIEND_ATTACKED = "Fiend_Attacked";
    const string FIEND_ATTACK = "Fiend_Attack";

    private void Start()
    {
        animator = GetComponent<Animator>();   
        
    }

    void ChangeAnimationState(string newState)
    {
        // stop the same animation from interrupting itself 
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
    public void PlayFiendChase()
    {
        ChangeAnimationState(FIEND_CHASE);
    }
    public void PlayFiendAttacked()
    {
        ChangeAnimationState(FIEND_ATTACKED); 
    }
    public void PlayFiendAttack()
    { 
        ChangeAnimationState(FIEND_ATTACK); 
    }
    public void PlayFiendWalk()
    {
        ChangeAnimationState(FIEND_WALK); 
    }
    public void PlayFiendIdle()
    {
        ChangeAnimationState(FIEND_IDLE); 
    }

}
