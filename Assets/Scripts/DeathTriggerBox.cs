using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggerBox : MonoBehaviour
{
    [Header("Linked scripts")]
    public GameController gameController;
    public PlayerClass playerClass;
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerClass.playerHealth = 0;
            gameController.State = GameController.GameStateEnums.Ended;

        }
      

    }
}
