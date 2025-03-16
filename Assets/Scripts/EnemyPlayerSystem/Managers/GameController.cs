using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public TutorialUIAnim tutorialUIAnim;
    public UIElementScaler uiElementScaler;
    

  
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

    void Start()
    {
        // Simulate pressing "1" in Start
        playerAttack.weaponState = PlayerAttack.PlayerWeaponState.Hit;
        StartCoroutine(InitializeUI());

    }
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
        //uiManager.UpdateTestUI();
        uiManager.UpdateFiendUI();
        CheckAllFiendsDead();
        

        //Put the checker for the buttons here
        if (playerAttack.actionState == PlayerAttack.PlayerActionStates.Null)
        {
            thirdPersonMovement.speed = 4f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("space key down");
                tutorialUIAnim.ShowGlow(KeyCode.Space);
                playerAttack.attackedEnemy = true;
                playerAttack.AttackEnemyCoroutine = StartCoroutine(playerAttack.AttackCurrentEnemy());

            }
            
            if (Input.GetKeyDown("1"))
            {
                audioManager.Play("ButtonPressed");

                playerAttack.weaponState = PlayerAttack.PlayerWeaponState.Hit;
                uiElementScaler.EnlargeUIElement("1");
            }
            if (Input.GetKeyDown("2"))
            {
                audioManager.Play("ButtonPressed");
                
                playerAttack.weaponState = PlayerAttack.PlayerWeaponState.Grab;
                uiElementScaler.EnlargeUIElement("2");
            }
            
            if (Input.GetKeyDown("3"))
            {
                audioManager.Play("ButtonHover");
                
            if (playerClass.noOfBombs > 0)
            {

                //Unfade the icon
               
                    audioManager.Play("ButtonPressed");

                    uiElementScaler.EnlargeUIElement("3");
                    playerAttack.weaponState = PlayerAttack.PlayerWeaponState.Bomb;
                   

                }
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
                StartCoroutine(TutorialCountdown());
                break;

            case GameStateEnums.Tutorial:
                if (playerAttack.actionState != PlayerAttack.PlayerActionStates.Attack)
                {
                    thirdPersonMovement.PlayerMovement();
                }

                if (playerClass.playerHealth <= 0 )
                {
       
                    
                    State = GameStateEnums.Ended;
                }
                break;

            case GameStateEnums.Started:
                break;

            case GameStateEnums.Paused:
                break;

            case GameStateEnums.Ended:
                
                if (playerClass.playerHealth <= 0)
                {
                yettyAnimation.PlayYettyDead();  // Play the Yetty dead animation
                StartCoroutine(HandleGameOver()); // Call coroutine to handle game over flow
                }
                else
                {
                    StartCoroutine(uiManager.CallLevelCompletedCanvas());
                    audioManager.Play("LevelCompleted");
                }

                
                
                
                break;
        }
    }

    // Coroutine to trigger Yetty death animation and then show the Game Over UI
    private IEnumerator HandleGameOver()
    {
        // Start the death animation and wait for it to finish
        playerClass.playerHealth = 0;
        yield return StartCoroutine(yettyAnimation.HandleYettyDeath());
        uiManager.CallGameOverCanvas();
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
        uiManager.FadeOutPanel();
        yield return new WaitForSeconds(1.5f);
        uiManager.FadeOutPanelFalse();
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
        // currentEnemyClass.currentNoOfFiends = playerClass.enemyDefeatedCounter;

        // Check if all fiends are defeated for the current level
        if (playerClass.enemyDefeatedCounter > 0)
        {
            if (playerClass.enemyDefeatedCounter == currentEnemyClass.totalNoOfFiends)
            {
                State = GameStateEnums.Ended;
              
                /*  switch (Level)
                  {
                      case LevelsStateEnums.None:
                          Level = LevelsStateEnums.Level1; // Transition to Level 1
                          levelCompletion.FinishLevel(0); // Finish current level (0)
                          //GameFlowManager.Instance.LoadLevelSelection(); // Go to Level Selection
                          playerClass.enemyDefeatedCounter = 0; // Reset enemy defeated counter
                          break;

                      case LevelsStateEnums.Level1:
                          Level = LevelsStateEnums.Level2; // Transition to Level 2
                          levelCompletion.FinishLevel(1); // Finish Level 1
                          //GameFlowManager.Instance.LoadLevelSelection(); // Go to Level Selection
                          playerClass.enemyDefeatedCounter = 0; // Reset enemy defeated counter
                          break;

                      case LevelsStateEnums.Level2:
                          // If you are at the last level (Level 2) or want to add more levels, do something
                          // For example, transitioning to a "congratulations" screen or game over
                          levelCompletion.FinishLevel(2); // Finish Level 2
                          //GameFlowManager.Instance.LoadLevelSelection(); // Go to Level Selection
                          playerClass.enemyDefeatedCounter = 0; // Reset enemy defeated counter
                          break;

                      default:
                          Debug.LogError("Unhandled level state.");
                          break;
                  }*/
            }
        }
        
    }
    
    //Linked to the button that pops up when the game ends
    public void ReloadScene()
    {
        StartCoroutine( PlayAgain());
    }

    public void LoadHomePage()
    {
     StartCoroutine( LoadHomeScene());
    }


    public IEnumerator PlayAgain()
    {
        audioManager.Play("ButtonPressed");
        uiManager.FadeInPanel();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
    public IEnumerator LoadHomeScene()
    {
        audioManager.Play("ButtonPressed");
        uiManager.FadeInPanel();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("CoverPage");
    }
    private IEnumerator InitializeUI()
    {
        yield return new WaitForEndOfFrame(); // Ensures UI is fully initialized before running EnlargeUIElement
        uiElementScaler.EnlargeUIElement("1");
    }

}
