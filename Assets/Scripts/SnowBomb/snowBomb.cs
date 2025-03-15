using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// UnityEngin
/// <summary>
/// This script does the explosion and creates damage for every fiend that is near it
/// For each item that collides with this item after 2 seconds, item take damage
/// spawn an explosion at the transform position of each fiend
/// Can be picked up by the yeti
/// </summary>
public class snowBomb : MonoBehaviour
{
    public GameObject explosionEffect;
    
    public int bombDamageValue;
    public Coroutine ExplodeCoroutine;
    //Linked scripts
    public GameObject enemyTracker; 
    
    public void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Fiend"))
        {
            ExplodeCoroutine = StartCoroutine(Explode(collision.gameObject));
 
        }
      

    }



    public IEnumerator Explode(GameObject victim )
    {
        
        yield return new WaitForSeconds(1f);
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 1f); // Destroy the explosion effect after 1 second
        //DISPLAY some UI here as a countdown
        
        yield return new WaitForSeconds(1f);  
        
        GameObject explosion1 = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion1, 1f); // Destroy the explosion effect after 1 second
        
        yield return new WaitForSeconds(1f);  
        GameObject explosion2 = Instantiate(explosionEffect, victim.transform.position, Quaternion.identity);
        Destroy(explosion2, 1f); // Destroy the explosion effect after 1 second
       
        enemyTracker.GetComponent<EnemyTracker>().TakeDamage(victim, bombDamageValue,victim.tag); 
        Debug.Log(victim.name);
    }
}