using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public float gravity = -9.81f;
    private Vector3 velocity;

    private readonly float[] allowedAngles = { 0f, 45f, 90f, 135f, 180f, -135f, -90f, -45f };

    public void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        Vector3 move = Vector3.zero; // Store final movement vector

        if (direction.magnitude >= 0.1f)
        {
            float rawAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float targetAngle = FindClosestAllowedAngle(rawAngle);
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        if (!controller.isGrounded)
        {
            velocity.y = 0.01f;
            Vector3 fixedPosition = transform.position;
            fixedPosition.y = 0f; // Force the character to stay at ground level
            transform.position = fixedPosition;
        }
            move = moveDirection.normalized * speed; // Store movement vector
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply both movement and gravity in a single call
        controller.Move((move + velocity) * Time.deltaTime);
    }



    private float FindClosestAllowedAngle(float rawAngle)
    {
        float closestAngle = allowedAngles[0];
        float smallestDifference = Mathf.Abs(Mathf.DeltaAngle(rawAngle, closestAngle));

        foreach (float allowedAngle in allowedAngles)
        {
            float difference = Mathf.Abs(Mathf.DeltaAngle(rawAngle, allowedAngle));
            if (difference < smallestDifference)
            {
                closestAngle = allowedAngle;
                smallestDifference = difference;
            }
        }

        return closestAngle;
    }
}