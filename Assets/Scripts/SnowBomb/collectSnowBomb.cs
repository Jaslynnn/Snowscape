using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectSnowBomb : MonoBehaviour
{
   public PlayerClass playerClass;
   public bool collectingBomb;
   public PlayerHealthBar playerHealthBar;
   public void OnTriggerEnter(Collider collision)
   {
      if (collision.gameObject.CompareTag("Player"))
      {
         Debug.Log("Player collided with snowbomb!");

         StartCoroutine(BombCollected());
         

      }
      

   }

   public IEnumerator BombCollected()
   {
      collectingBomb = true;
      
     if (collectingBomb = true)
      {
         if (playerClass.playerHealth < 100)
         {
            playerClass.playerHealth += 10;
            playerHealthBar.SetHealth(playerClass.playerHealth);
         }
         
         playerClass.noOfBombs += 1;

      }
      
      yield return new WaitForSeconds(0.1f);
      collectingBomb = false;
      transform.gameObject.SetActive(false);
      
      
   }
   
}

