using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateCtrlYetty : MonoBehaviour
{
    Animator animator;
    float velocity = 0.0f;
    public float acceleration = 0.1f;
    int VelocityHash;

    private bool rightClickPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        //set reference for animator 
        animator = GetComponent<Animator>();

        //increase performance 
        VelocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool rightClickPressed = Input.GetMouseButtonDown(1);

        if (forwardPressed)
        {
            velocity += Time.deltaTime * acceleration;
        }

        if (rightClickPressed)
        {
            animator.SetTrigger("YettyGrab");
            Debug.Log("RightClickPressed");
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Ensure animation is in "YettyGrab" state and has completed
            if (stateInfo.IsName("YettyGrab") && stateInfo.normalizedTime >= 0.99f)
            {
                rightClickPressed = false;
                Debug.Log("RightClick Reset");
            }
            animator.SetFloat(VelocityHash, velocity);
        }

    }
}