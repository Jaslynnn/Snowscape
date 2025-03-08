
using UnityEngine;
using TMPro;

using UnityEngine.Serialization;

/// <summary>
/// This script controls all the UI
/// </summary>
public class UIManager : MonoBehaviour
{
    [FormerlySerializedAs("playerHP")] [Header("Player Stats")]
    //player Stats
    public TMP_Text playerHp;
    public TMP_Text playerWeapon;
    public TMP_Text enemyDefeatedCount;

    public TMP_Text playerAction;
    public TMP_Text playerHoldItem;


    [Header("Enemy Stats")]
    //Enemy Stats
    public TMP_Text enemyName;
    public TMP_Text attackedEnemy;
    [FormerlySerializedAs("attackedEnemyHP")] public TMP_Text attackedEnemyHp;
    public TMP_Text enemiesLeft;
    


    [Header("GameState")]
    public TMP_Text currentGameState;

    [FormerlySerializedAs("GameOverPage")] [Header("GameUI")]
    public GameObject gameOverPage;
    [FormerlySerializedAs("Level1CompletedPage")] public GameObject level1CompletedPage;


    [Header("Linked scripts")]
    //Linked Scripts
    public PlayerClass playerClass;
    public PlayerAttack playerAttack;
    public CurrentEnemyClass currentEnemyClass;
    public GameController gameController;
    public PlayerHealthBar playerHealthBar;

    private void Awake()
    {
        gameOverPage.SetActive(false);

    }
    void Start()
    {
        playerClass.playerHealth = playerClass.playerMaxHealth;
        playerHealthBar.SetMaxHealth(playerClass.playerMaxHealth);
    }//THis function updates the player's health bar
   
   
   
    public void UpdatePlayerHealthBar()
    {
        playerHealthBar.SetHealth(playerClass.playerHealth); 
    }
    public void UpdateEnemyHealthBar()
    {
        
    }
    
    
    //Placed in update function of the gameController to update the UI
    public void UpdateTestUI()
    {
        playerHp.text = " HP : " + playerClass.playerHealth.ToString();

        if (playerClass.playerDamageValue == 10)
        {
            playerWeapon.text = "Weapon : Stick";
        }

        playerAction.text = "Action:" + playerAttack.actionState.ToString();
        enemyDefeatedCount.text = "Defeated" + playerClass.enemyDefeatedCounter.ToString();
        enemiesLeft.text = playerClass.enemyDefeatedCounter.ToString() + " / " + currentEnemyClass.totalNoOfFiends.ToString();
        if (currentEnemyClass.currentEnemy != null)
        {

            enemyName.text = "Nearest : " + currentEnemyClass.currentEnemy.name;
        }

        if (currentEnemyClass.attackedEnemy != null)
        {
            attackedEnemy.text = "Attacked : " + currentEnemyClass.attackedEnemy.name;

            attackedEnemyHp.text = "HP : " + currentEnemyClass.attackedEnemyHealth.ToString();
        }

        if (playerClass.currentGrabbedObject != null)
        {
            playerHoldItem.text = "Holding : " + playerClass.currentGrabbedObject.name;

        }
        if (playerClass.currentGrabbedObject == null)
        {
            playerHoldItem.text = "Holding : None";
        }


        currentGameState.text = "State : " + gameController.State.ToString();
        if (gameController.State == GameController.GameStateEnums.Ended)
        {
            gameOverPage.SetActive(true);
        }

        if (gameController.State != GameController.GameStateEnums.Ended)
        {
            gameOverPage.SetActive(false);

        }
        if (gameController.Level == GameController.LevelsStateEnums.Level1)
        {
            level1CompletedPage.SetActive(true);

        }
        if (gameController.Level != GameController.LevelsStateEnums.Level1)
        {
            level1CompletedPage.SetActive(false);

        }
    }

}
