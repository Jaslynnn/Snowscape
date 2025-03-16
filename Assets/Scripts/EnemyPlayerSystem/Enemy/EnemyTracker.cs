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
    public EnemyAnimation enemyAnimation;

    private void Awake()
    {
        EnemyTranslation = ScriptableObject.Instantiate(EnemyTranslationType);

    }

    public void TakeDamage(GameObject enemy, int damage, string enemyTag)
    {
        // Remove all enemies that are either null, inactive, or have 0 health
        Debug.Log(enemy);
        enemies.RemoveAll(entry => entry == null || entry.EnemyObject == null || entry.EnemyHealth <= 0);

        foreach (EnemyTranslationTableEntry entry in enemies.ToList())
        {
            if (entry.EnemyObject == null || !entry.EnemyObject.activeInHierarchy) // Skip inactive enemies
            {
                // Remove the destroyed enemy from the list
                enemies.Remove(entry);
                continue;
            }

            if (entry.EnemyObject == enemy)
            {
                currentEnemyClass.attackedEnemyHealth = entry.EnemyHealth;
                entry.EnemyHealth -= damage;
                Debug.Log("Fiend attacked, health: " + entry.EnemyHealth);
                currentEnemyClass.attackedEnemyHealth = entry.EnemyHealth;

                // If health is still above 0, play the attacked animation
                if (entry.EnemyHealth > 0)
                {
                    enemyAnimation.PlayFiendAttacked();
                }
                else // Health is 0, play the death animation
                {
                    enemies.Remove(entry);
                    if (entry.EnemyObject.CompareTag("Fiend"))
                    {
                        // Play death animation and then deactivate after delay
                        UnityEngine.AI.NavMeshAgent agent = entry.EnemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
                        agent.enabled = false;
                        entry.EnemyObject.layer = LayerMask.NameToLayer("Default");
                        entry.EnemyObject.tag = "Untagged";
                        StartCoroutine(HandleEnemyDeath(entry.EnemyObject));
                        
                        playerClass.enemyDefeatedCounter += 1;
                        
                    }
                }

                return;
            }
        }

        // Handle the case for new enemies
        if (enemy)
        {
            EnemyTranslationTableEntry enemy01 = EnemyTranslation.TakeDamage(enemy, damage, enemyTag);

            if (enemy01 != null && enemy01.EnemyHealth > 0 && !enemies.Contains(enemy01))
            {
                enemies.Add(enemy01);
                currentEnemyClass.attackedEnemyHealth = enemy01.EnemyHealth;
            }
        }

        // Update health bar if needed
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
                    enemyHealthText.text = currentEnemyClass.attackedEnemyHealth.ToString();
                }
            }
        }

    }

    private IEnumerator HandleEnemyDeath(GameObject enemyObject)
    {
        // Log to check if the death is being triggered
        Debug.Log("Handling enemy death for: " + enemyObject.name);

        // Play the death animation
        enemyAnimation.PlayFiendDead();

        // Log to check if the animation is being triggered
        Debug.Log("Played FiendDead animation");

        // Wait for 3 seconds (adjust based on animation duration)
        yield return new WaitForSeconds(3f); // Adjust this duration based on your animation length

        // Log to check if we're deactivating the enemy
        Debug.Log("Deactivating enemy: " + enemyObject.name);

        // Deactivate the enemy object after the animation is complete
        enemyObject.SetActive(false);
    }







}
