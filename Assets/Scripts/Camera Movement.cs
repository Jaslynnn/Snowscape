using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;   
    public float smoothSpeed = 0.125f; 

    // add offset to player transform
    // smoothly move camera
    // apply new position
    void LateUpdate()
    {
        if (player != null)
        {
          
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

        }
    }
}

