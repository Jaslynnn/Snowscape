using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private BackgroundMusicManager bgmManager;

    private void Awake()
    {
        bgmManager = FindObjectOfType<BackgroundMusicManager>();
        if (bgmManager == null)
            Debug.LogWarning("No BackgroundMusicManager found in Main Menu!");
    }

    private void Start()
    {
        if (bgmManager != null)
            bgmManager.PlayMusic(0);

    }
}
