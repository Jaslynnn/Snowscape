using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerClass : MonoBehaviour
{
    public string PlayerName;
    public int playerMaxHealth = 100;
    [FormerlySerializedAs("PlayerHealth")] public int playerHealth = 100;
    [FormerlySerializedAs("PlayerDamageValue")] public int playerDamageValue = 10;
    [FormerlySerializedAs("PlayerThrowDamageValue")] public int playerThrowDamageValue = 20;
    public GameObject currentGrabbedObject;
    [FormerlySerializedAs("EnemyDefeatedCounter")] public int enemyDefeatedCounter;
  
    /*public PlayerClass( string playerName, int playerHealth , int playerDamageValue , int enemyDefeatedCounter , bool quest1 , bool quest2 )
    {
        this.PlayerName = playerName;
        this.playerHealth = playerHealth;
        this.playerDamageValue = playerDamageValue;
        this.enemyDefeatedCounter = enemyDefeatedCounter;
        this.Quest1 = quest1;
        this.Quest2 = quest2;
    }
    */

}
