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
    public EnemyTracker enemyTracker; 
    
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
        //DISPLAY some UI here as a countdown
        Debug.Log(2);
        yield return new WaitForSeconds(1f);  
        Debug.Log(1);
        enemyTracker.TakeDamage(victim, bombDamageValue,victim.tag); 
        
    }
}