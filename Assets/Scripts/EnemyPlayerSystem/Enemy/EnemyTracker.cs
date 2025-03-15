using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

/// <summary>
/// This script tracks things that happen to the enemy not the player
/// </summary>
public class EnemyTracker : MonoBehaviour
{
    [SerializeField] BaseEnemyTranslation EnemyTranslationType;
    BaseEnemyTranslation EnemyTranslation;
    public List<EnemyTranslationTableEntry> enemies = new List<EnemyTranslationTableEntry>();
    public bool enemyExistsAlready = false;
    public Transform enemyHealthBarObject = null;
    public TMP_Text enemyHealthText = null;

    // Linking scripts 
    public PlayerClass playerClass;
    public CurrentEnemyClass currentEnemyClass;

    public EnemyHealthBar enemyHealthBar;
    // Start is called before the first frame update

    public EnemyAnimation enemyAnimation;
    private void Awake()
    {
        EnemyTranslation = ScriptableObject.Instantiate(EnemyTranslationType);

    }

    public void TakeDamage(GameObject enemy, int damage, string enemyTag)
    {
        //Checks through the list of classes to see whether the enemy name exists
        Debug.Log(enemy);
        enemies.RemoveAll(entry => entry == null || entry.EnemyObject == null || entry.EnemyHealth <= 0);
        foreach (EnemyTranslationTableEntry entry in enemies.ToList())
        {

            if (entry.EnemyObject == null || !entry.EnemyObject.activeInHierarchy) // Also checks if the enemy is inactive
            {
                // Remove the destroyed enemy from the list
                enemies.Remove(entry);
                continue; // Skip processing this entry since it's already destroyed
            }

            if (entry.EnemyObject == enemy)
            {

                currentEnemyClass.attackedEnemyHealth = entry.EnemyHealth;
                //int currentEnemyHealth = entry.EnemyHealth;
                entry.EnemyHealth -= damage;
                enemyAnimation.PlayFiendAttacked();
                Debug.Log("Fiend attacked" + entry.EnemyHealth);
              currentEnemyClass.attackedEnemyHealth = entry.EnemyHealth;
                //***Add abit of force here to make the object shake so that player gets the hint to keep on hitting it
                if (entry.EnemyHealth <= 0) 
                {

                    //***Place destruction code here
                    enemyAnimation.PlayFiendAttacked();
                    enemies.Remove(entry);
                    entry.EnemyObject.SetActive(false);
                    if (entry.EnemyObject.CompareTag("Fiend"))
                    {
                    // put dead anim here + 3 secs
                    playerClass.enemyDefeatedCounter += 1;

                    }



                }
                return;

            }
              
         
                    
            

                
        }
       
        if (enemy) 
        {
            EnemyTranslationTableEntry enemy01 = EnemyTranslation.TakeDamage(enemy, damage, enemyTag);

            if (enemy01 != null && enemy01.EnemyHealth > 0 && !enemies.Contains(enemy01))
            {
                enemies.Add(enemy01);
            currentEnemyClass.attackedEnemyHealth = enemy01.EnemyHealth;
            }
        }


        //Adds new class to the list of classes:\
        //returns the enemyHealth
        //EnemyTranslation.TakeDamage(enemyName, damage, enemyTag);
        if (enemy.CompareTag("Fiend"))
        {
            
        if (!enemyHealthBarObject)
        {
            enemyHealthBarObject = enemy.transform.Find("Canvas/EnemyHealthBar");
            enemyHealthText = enemyHealthBarObject.Find("EnemyHealthNo").GetComponent<TMP_Text>();
            if (enemyHealthBarObject != null)
            {
                
            Debug.Log(enemyHealthBarObject.gameObject.name);
            enemyHealthBarObject.gameObject.SetActive(true);
            // enemyHealthBar.SetMaxHealth(currentEnemyClass.attackedEnemyHealth);
            // enemyHealthBar.SetHealth(currentEnemyClass.attackedEnemyHealth);
            enemyHealthText.text = currentEnemyClass.attackedEnemyHealth.ToString();
            }
            //Set the current enemy health
        }
        
        enemyHealthBarObject = enemy.transform.Find("Canvas/EnemyHealthBar");
        enemyHealthText = enemyHealthBarObject.Find("EnemyHealthNo").GetComponent<TMP_Text>();
            
            
        }
        


   


    }





}
