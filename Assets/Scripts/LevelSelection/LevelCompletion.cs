using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletion : MonoBehaviour
{
    public void FinishLevel(int levelIndex)
    {
        if (GameFlowManager.Instance == null)
        {
            Debug.LogError("GameFlowManager instance is null!");
        }
        else
        {
            GameFlowManager.Instance.CompleteLevel(levelIndex);
        }
    }

}
