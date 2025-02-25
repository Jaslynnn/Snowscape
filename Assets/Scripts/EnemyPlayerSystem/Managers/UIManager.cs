using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
/// <summary>
/// This script controls all the UI
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Player Stats")]
    //player Stats
    public TMP_Text playerHP;
    public TMP_Text playerWeapon;
    public TMP_Text enemyDefeatedCount;

    public TMP_Text playerAction;
    public TMP_Text playerHoldItem;


    [Header("Enemy Stats")]
    //Enemy Stats
    public TMP_Text enemyName;
    public TMP_Text attackedEnemy;
    public TMP_Text attackedEnemyHP;
    public TMP_Text enemiesLeft;


    [Header("GameState")]
    public TMP_Text currentGameState;

    [Header("GameUI")]
    public GameObject GameOverPage;
    public GameObject Level1CompletedPage;


    [Header("Linked scripts")]
    //Linked Scripts
    public PlayerClass playerClass;
    public PlayerAttack playerAttack;
    public CurrentEnemyClass currentEnemyClass;
    public GameController gameController;

    private void Awake()
    {
        GameOverPage.SetActive(false);

    }
    //Placed in update function of the gameController to update the UI
    public void UpdateTestUI()
    {
        playerHP.text = " HP : " + playerClass.PlayerHealth.ToString();

        if (playerClass.PlayerDamageValue == 10)
        {
            playerWeapon.text = "Weapon : Stick";
        }

        playerAction.text = "Action:" + playerAttack.ActionState.ToString();
        enemyDefeatedCount.text = "Defeated" + playerClass.EnemyDefeatedCounter.ToString();
        enemiesLeft.text = "Killed E: " + playerClass.EnemyDefeatedCounter.ToString() + " / " + currentEnemyClass.totalNoOfFiends.ToString();
        if (currentEnemyClass.currentEnemy != null)
        {

            enemyName.text = "Nearest : " + currentEnemyClass.currentEnemy.name;
        }

        if (currentEnemyClass.attackedEnemy != null)
        {
            attackedEnemy.text = "Attacked : " + currentEnemyClass.attackedEnemy.name;

            attackedEnemyHP.text = "HP : " + currentEnemyClass.attackedEnemyHealth.ToString();
        }

        if (playerClass.currentGrabbedObject != null)
        {
            playerHoldItem.text = "Holding : " + playerClass.currentGrabbedObject.name.ToString();

        }
        if (playerClass.currentGrabbedObject == null)
        {
            playerHoldItem.text = "Holding : None";
        }


        currentGameState.text = "State : " + gameController.State.ToString();
        if (gameController.State == GameController.GameStateEnums.Ended)
        {
            GameOverPage.SetActive(true);
        }

        if (gameController.State != GameController.GameStateEnums.Ended)
        {
            GameOverPage.SetActive(false);

        }
        if (gameController.Level == GameController.LevelsStateEnums.Level1)
        {
            Level1CompletedPage.SetActive(true);

        }
        if (gameController.Level != GameController.LevelsStateEnums.Level1)
        {
            Level1CompletedPage.SetActive(false);

        }
    }

}
