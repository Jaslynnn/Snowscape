using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    public string PlayerName;
    public int PlayerHealth;
    public int PlayerDamageValue;
    public int EnemyDefeatedCounter;
    public bool Quest1 = false;
    public bool Quest2 = false;

    public PlayerClass( string playerName, int playerHealth , int playerDamageValue , int enemyDefeatedCounter , bool quest1 , bool quest2 )
    {
        this.PlayerName = playerName;
        this.PlayerHealth = playerHealth;
        this.PlayerDamageValue = playerDamageValue;
        this.EnemyDefeatedCounter = enemyDefeatedCounter;
        this.Quest1 = quest1;
        this.Quest2 = quest2;
    }


}
