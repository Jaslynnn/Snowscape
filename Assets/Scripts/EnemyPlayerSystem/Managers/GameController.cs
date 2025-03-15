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
    public GameObject tutorial;

    [Header("Linked scripts")]
    //Scripts to be linked
    public PlayerClass playerClass;

    public PlayerAttack playerAttack;
    public CurrentEnemyClass currentEnemyClass;
    public ThirdPersonMovement thirdPersonMovement;
    public UIManager uiManager;
    public PlayerHealthBar playerHealthBar;
    public EnemyHealthBar enemyHealthBar;
    public LevelCompletion levelCompletion;
    public YettyAnimation yettyAnimation;
    public AudioManager audioManager;
   


  
    public enum GameStateEnums
    {
        NotStarted,
        Tutorial,
        Started,
        Paused,
        Ended
    }

    public enum LevelsStateEnums
    {
        None,
        Level1,
        Level2,
        Level3,
    }
    public enum TutorialStateEnums
    {

    }

    public GameStateEnums State;
    public LevelsStateEnums Level;

    
    void Awake()
    {
        CountTotalFiends();
        // backgroundMusicManager = FindObjectOfType<BackgroundMusicManager>();
        // if (backgroundMusicManager == null)
        // {
            // Debug.Log("BackgroundMusicManager not found in the Game Scene!");
        // }
        playerMesh.transform.position = spawnPoint.transform.position;
    }




    void Update()
    {
        ChangeGameState();
        uiManager.UpdateTestUI();
        CheckAllFiendsDead();
        //Put the checker for the buttons here
        if (playerAttack.actionState == PlayerAttack.PlayerActionStates.Null)
        {
            thirdPersonMovement.speed = 4f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerAttack.attackedEnemy = true;
                playerAttack.AttackEnemyCoroutine = StartCoroutine(playerAttack.AttackCurrentEnemy());

            }
            if (Input.GetKeyDown("1"))
            {

                playerAttack.weaponState = PlayerAttack.PlayerWeaponState.Hit;

            }
            if (Input.GetKeyDown("2"))
            {

                playerAttack.weaponState = PlayerAttack.PlayerWeaponState.Grab;

            }
            if (Input.GetKeyDown("3"))
            {

                playerAttack.weaponState = PlayerAttack.PlayerWeaponState.Bomb;

            }

        }

        if (playerAttack.actionState == PlayerAttack.PlayerActionStates.Defense)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                thirdPersonMovement.speed = 2f;
                if (playerAttack.weaponState == PlayerAttack.PlayerWeaponState.Grab)
                {
                    yettyAnimation.PlayYettyGrab();
                    StartCoroutine(playerAttack.ReleaseCurrentEnemy());
                }
            }
        }
       


    }

    public void ChangeGameState()
    {
        switch (State)
        {
            case GameStateEnums.NotStarted:
                //playerMesh.transform.position = spawnPoint.transform.position;
                //if the start button is pressed, start the game
                StartCoroutine(TutorialCountdown());
                break;

            case GameStateEnums.Tutorial:
                if (playerAttack.actionState != PlayerAttack.PlayerActionStates.Attack)
                {
                    thirdPersonMovement.PlayerMovement(); 
                }
                
                if (playerClass.playerHealth <= 0)
                {
                    Debug.Log("Health = 0");
                    State = GameStateEnums.Ended;
                    playerClass.playerHealth = 0;
                    //uiManager.ChangeToGameEndedAnimationState();
                }
                //Play the tutorial
                break;

            case GameStateEnums.Started:

                break;
   

            case GameStateEnums.Paused:
 
                break;

            case GameStateEnums.Ended:
                // GameFlowManager.Instance.StartGame();
                break;

        }

    }


    //State Changing Functions
    //Attached to a button
    public void NotStartedGame()
    {
        //Attached to a button
        playerClass.playerHealth = 100;
        playerClass.enemyDefeatedCounter = 0;
        State = GameStateEnums.NotStarted;
        ReshowAllAssets();


    }
    public void StartGame()
    {
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

    public IEnumerator TutorialCountdown()
    {
        yield return new WaitForSeconds(4f);
        tutorial.SetActive(false);
        State = GameStateEnums.Tutorial;
    }
    //RUns through the asset loop to reshow everything again
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
    public void CountTotalFiends()
    {
        AllAssetsArray = new GameObject[allTheAssets.transform.childCount];
        for (int i = 0; i < AllAssetsArray.Length; i++)
        {
            AllAssetsArray[i] = allTheAssets.transform.GetChild(i).gameObject;
        }
        foreach (GameObject Asset in AllAssetsArray)
        {
            if (Asset.CompareTag("Fiend"))
            {
                currentEnemyClass.totalNoOfFiends += 1;
            }
            
        }
    }

    public void CheckAllFiendsDead()
    {
        currentEnemyClass.currentNoOfFiends = playerClass.enemyDefeatedCounter;
       
            switch (Level)
            {
                case LevelsStateEnums.None:
                if (playerClass.enemyDefeatedCounter == currentEnemyClass.totalNoOfFiends)
                {
                    Level = LevelsStateEnums.Level1;

                    //Use sceneLoader to go to the next level

                    GameFlowManager.Instance.LoadLevelSelection();
                    playerClass.enemyDefeatedCounter = 0;

                }
                    break;

                case LevelsStateEnums.Level1:
                if (playerClass.enemyDefeatedCounter == currentEnemyClass.totalNoOfFiends)
                {
                    Level = LevelsStateEnums.Level2;
                }
                    break;

                case LevelsStateEnums.Level2:
                if (playerClass.enemyDefeatedCounter == currentEnemyClass.totalNoOfFiends)
                {
                    Level = LevelsStateEnums.Level2;
                }
                    break;

            }
            //Take the totalFiends - playerClass.enemyDefeated
            //If Fiends = 0 , Level 1 = true , show the congrats UI
        
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
