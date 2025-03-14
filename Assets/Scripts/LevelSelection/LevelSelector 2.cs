using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;

    private void Start()
    {
        button1.onClick.AddListener(() => GameFlowManager.Instance.LoadLevel(1));
        button2.onClick.AddListener(() => GameFlowManager.Instance.LoadLevel(2));
        button3.onClick.AddListener(() => GameFlowManager.Instance.LoadLevel(3));
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        button1.interactable = true;
        button2.interactable = unlockedLevels >= 2;
        button3.interactable = unlockedLevels >= 3;

    }
    

    
}

