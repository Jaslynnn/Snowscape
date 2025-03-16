using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance; // Singleton instance

    public GameObject mainMenuCanvas; 
    public GameObject levelSelectionCanvas;
    private BackgroundMusicManager backgroundMusicManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // keeps this object across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instance
        }
        
    }

        private void Start()
         {
        if(!PlayerPrefs.HasKey("UnlockedLevels"))
        {
            if (backgroundMusicManager != null)
                backgroundMusicManager.PlayMusic(0);
            PlayerPrefs.SetInt("UnlockedLevels", 1); // Start with level 1 unlocked
            PlayerPrefs.Save();

        }
        }

    public void StartGame()
    {
        SceneManager.LoadScene("Level2Bomb"); //Load tutorial
        
    }

    public void LoadLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
        
        return;

    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("PlaceholderLevel" + levelIndex);
    }


    
    public void CompleteLevel(int currentLevel)
    {
        int nextLevel = currentLevel + 1;
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        if (nextLevel > unlockedLevels && nextLevel <= 3) // Ensure max is level 3 
        {
            PlayerPrefs.SetInt("UnlockedLevels", nextLevel);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("LevelSelection"); // Return to level selection
    }
}
