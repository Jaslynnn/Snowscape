using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    [SerializeField] BaseEnemyTranslation EnemyTranslationType;
    BaseEnemyTranslation EnemyTranslation;
    // Start is called before the first frame update
    private void Awake()
    {
        EnemyTranslation = ScriptableObject.Instantiate(EnemyTranslationType);

    }

    public void TakeDamage(string enemyName, int damage, string enemyTag)
    {
        EnemyTranslation.TakeDamage(enemyName, damage, enemyTag);
    }

}
