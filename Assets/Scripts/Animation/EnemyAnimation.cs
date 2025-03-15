using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator;
    public string currentState;
    public bool isAttacking = false;

    // Animation state names
    const string FIEND_IDLE = "Fiend_Idle";
    const string FIEND_WALK = "Fiend_Walk";
    const string FIEND_CHASE = "Fiend_Chase";
    const string FIEND_ATTACKED = "Fiend_Attacked";
    const string FIEND_ATTACK = "Fiend_Attack";
    const string FIEND_MOVEMENT = "Fiend_Movement";
    const string FIEND_DEAD = "Fiend_Death";

    public AudioManager audioManager;

    // Sound names 
    const string FIEND_BITE = "FiendBite";
    const string FIEND_SNARL = "FiendSnarl";
    const string FIEND_WHIMPER = "FiendWhimper";
    const string FIEND_FOOTSTEPS = "FiendFootsteps";
    const string FIEND_BARK = "FiendBark";

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Enemy animator is missing!");
        }
        else
        {
            Debug.Log(animator + "found");
        }
    }

    public void UpdateMovementSpeed(float speed)
    {
        // Only update movement if not attacking.
        if (!isAttacking)
        {
            animator.SetFloat("fiendSpeed", speed);
            //Debug.Log("Animator speed set to: " + speed);
        }
    }

    void ChangeAnimationState(string newState)
    {
        // Stop the same animation from interrupting itself
        if (currentState == newState) return;

        // Play the animation   
        animator.Play(newState);

        // Reassign the current state
        currentState = newState;
    }

    public void PlayFiendChase()
    {
        ChangeAnimationState(FIEND_CHASE);
        audioManager.Play(FIEND_FOOTSTEPS);
    }

    public void PlayFiendAttacked()
    {
        ChangeAnimationState(FIEND_ATTACKED);
        audioManager.Play(FIEND_SNARL);
    }

    public void PlayFiendAttack()
    {
        audioManager.Play(FIEND_BITE);
        isAttacking = true;
        ChangeAnimationState(FIEND_ATTACK);
        

        StartCoroutine(ReturnToDefaultState());
    }

    public void PlayFiendWalk()
    {
        ChangeAnimationState(FIEND_WALK);
        audioManager.Play(FIEND_FOOTSTEPS);
    }

    public void PlayFiendDead()
    {
        audioManager.Play(FIEND_WHIMPER);
        ChangeAnimationState(FIEND_DEAD);
    }

    public void PlayFiendIdle()
    {
        ChangeAnimationState(FIEND_IDLE);
    }

    private IEnumerator ReturnToDefaultState()
    {
        // Wait for the duration of the attack animation.
        // It's more robust to use a set duration rather than reading the blend tree's current state's length.
        float attackAnimationDuration = 1f; // Set this value to match your attack clip duration
        yield return new WaitForSeconds(attackAnimationDuration);

        isAttacking = false;
        ChangeAnimationState(FIEND_MOVEMENT);
    }
}
