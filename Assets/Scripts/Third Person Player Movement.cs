using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    // Array of allowed angles for 6 main directions
    private readonly float[] allowedAngles = { 0f, 45f, 90f, 135f, 180f, -135f, -90f, -45f };

    void Update()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Check if the player is going in any direction
        // Find the closest allowed angle to the raw angle
        // Smoothly rotate the player towards the target angle
        // Move the player in the quantized direction

        if (direction.magnitude >= 0.1f)
        {
            
            float rawAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float targetAngle = FindClosestAllowedAngle(rawAngle);
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
    }

    // Find differences of rawAngle to closest angle 
    // 
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
