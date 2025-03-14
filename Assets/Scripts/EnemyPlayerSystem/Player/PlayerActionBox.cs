using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script is attached to the player action box and makes the collided item the currrent enemy object/ action object
/// </summary>
public class PlayerActionBox : MonoBehaviour
{
    public CurrentEnemyClass currentEnemyClass;
    public void OnTriggerStay(Collider collision)
    {
        Debug.Log(collision.gameObject.name);  
        if (collision.gameObject.CompareTag("Fiend"))
        { Debug.Log(collision.gameObject.name); 
            currentEnemyClass.currentEnemy = collision.gameObject; 
        }
       
    }
}
