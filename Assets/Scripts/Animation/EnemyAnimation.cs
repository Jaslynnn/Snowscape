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

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Enemy animator is missing!");
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
    }

    public void PlayFiendAttacked()
    {
        ChangeAnimationState(FIEND_ATTACKED);
    }

    public void PlayFiendAttack()
    {
        isAttacking = true;
        ChangeAnimationState(FIEND_ATTACK);
        //Debug.Log("FIENDATTACK STATE");

        StartCoroutine(ReturnToDefaultState());
    }

    public void PlayFiendWalk()
    {
        ChangeAnimationState(FIEND_WALK);
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
