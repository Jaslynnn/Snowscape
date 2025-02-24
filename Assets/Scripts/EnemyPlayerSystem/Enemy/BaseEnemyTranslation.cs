using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyTranslation : ScriptableObject
{
    public string EnemyName { get; protected set; } = "Enemy";
    public GameObject EnemyObject { get; protected set; } = null;
    public enum EnemyType { Nothing, Fiend, FiendNest, Breakable};
    public int EnemyHealth { get; protected set; } = 100;

    public enum ItemsDropped { Snowcube , Bone , Eyeball, Meat , Tail , Coins};

    public List<ItemsDropped> itemDroppedList;
    public string DroppedItem => DropItem(itemDroppedList);


    public abstract EnemyTranslationTableEntry TakeDamage(GameObject enemyObject, int damage, string enemyTag);


    //Function that randomises what the enemy will drop
    public abstract string DropItem(List<ItemsDropped> itemDroppedList);



}
