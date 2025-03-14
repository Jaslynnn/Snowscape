using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextSceneTest : MonoBehaviour
{
    [SerializeField] LevelCompletion levelCompletion;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            levelCompletion.FinishLevel(1);
        }
    }
}
