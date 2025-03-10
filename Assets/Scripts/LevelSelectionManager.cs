using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    private BackgroundMusicManager backgroundMusicManager;
    private void Awake()
    {
        backgroundMusicManager = FindObjectOfType<BackgroundMusicManager>();
        if (backgroundMusicManager == null)
        {
            Debug.Log("BackgroundMusicManager not found in the Game Scene!");
        }
    }
    private void Start()
    {
        if (backgroundMusicManager != null)
        {
            backgroundMusicManager.ChangeMusic(0); // Switches to soundtrack index 1
        }
    }

}
