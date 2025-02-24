using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THis script uses enum states to control the game, including conditions for things to change
/// </summary>

public class GameController : MonoBehaviour
{

    [Header("GameObjects")]
    //GameObjects to be linked
    //Start Game
    public GameObject allTheAssets;
    public GameObject[] AllAssetsArray;
    public GameObject playerMesh;
    public GameObject spawnPoint;


    [Header("Linked scripts")]
    //Scripts to be linked
    public PlayerClass playerClass;
    public ThirdPersonMovement thirdPersonMovement;
    public UIManager uiManager;


    public enum GameStateEnums
    {
        NotStarted,
        Tutorial,
        Started,
        Paused,
        Ended
    }

    public enum TutorialStateEnums
    {

    }

    public GameStateEnums State;

    void Update()
    {
        ChangeGameState();
        uiManager.UpdateTestUI();
    }

    public void ChangeGameState()
    {
        switch (State)
        {
            case GameStateEnums.NotStarted:
                //playerMesh.transform.position = spawnPoint.transform.position;
                //if the start button is pressed, start the game
               
                break;

            case GameStateEnums.Tutorial:
                thirdPersonMovement.PlayerMovement();
                if (playerClass.PlayerHealth <= 0)
                {
                    Debug.Log("Health = 0");
                    State = GameStateEnums.Ended;
                    playerClass.PlayerHealth = 0;
                    //uiManager.ChangeToGameEndedAnimationState();
                }
                //Play the tutorial
                break;

            case GameStateEnums.Started:
                //Reposition the player to the spawn point
                //FindObjectOfType<AudioManager>().AdjustVolume("Theme", 0.1f);
                thirdPersonMovement.PlayerMovement();

            
                if (playerClass.PlayerHealth <= 0)
                {
                    Debug.Log("GameEnded");
                    State = GameStateEnums.Ended;
                    playerClass.PlayerHealth = 0;
                    //uiManager.ChangeToGameEndedAnimationState();
                }
                break;
   

            case GameStateEnums.Paused:
 
                break;

            case GameStateEnums.Ended:

                break;

        }

    }

    public void NotStartedGame()
    {
        //Attached to a button
        playerClass.PlayerHealth = 100;
        playerClass.EnemyDefeatedCounter = 0;
        State = GameStateEnums.NotStarted;
        ReshowAllAssets();


    }
    //Attached to a button
    public void StartGame()
    {
        //Attached to a button
        State = GameStateEnums.Started;


    }

    public void StartTutorial ()
    {
        //Attached to a button
        State = GameStateEnums.Tutorial;
    }

    public void PauseGame()
    {
        //Attached to a button
        State = GameStateEnums.Paused;
    }
    public void EndGame()
    {
        //Attached to a button
        State = GameStateEnums.Ended;
    }

    public void ReshowAllAssets()
    {
        AllAssetsArray = new GameObject[allTheAssets.transform.childCount];
        for (int i = 0; i < AllAssetsArray.Length; i++)
        {
            AllAssetsArray[i] = allTheAssets.transform.GetChild(i).gameObject;
        }
        foreach (GameObject Asset in AllAssetsArray)
        {
            Asset.SetActive(true);
        }
    }



    public void PlayAgain()
    {
        //SceneManager.LoadScene("FiendControl");

        /*gameTime = 0;
        xpTracker.ResetXP();
        playerHealth.AddAllHealthHeartUIs();
        

        ReshowAllAssets();
        State = GameStateEnums.NotStarted;
        
        // Add all the items to an array and then set active again
        //Make the player spawn at a specific spot in the game
        //uiManager.PlayAgainUI();//resets all the stats for the game
        */


    }

}
