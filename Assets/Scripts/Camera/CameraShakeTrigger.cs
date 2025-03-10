using UnityEngine;
using Cinemachine;

public class CameraShakeTrigger : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    public void GenerateImpulseSource()
    {
        impulseSource.GenerateImpulse();
    }
}





            