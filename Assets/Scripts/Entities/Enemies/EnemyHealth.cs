using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IAttackable
{
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    ActivatesWhenDefeatEnemies activatesWhenDefeatEnemies;

    private void Awake()
    {
        currentHealth = maxHealth; 
    }

    public void InsertRoom(ActivatesWhenDefeatEnemies activatesWhenDefeatEnemies)
    {
        this.activatesWhenDefeatEnemies = activatesWhenDefeatEnemies;
    }
    public void OnBeAttacked(int value, Vector2 dir)
    {
        currentHealth -= value;
        if(currentHealth <= 0)
        {
            activatesWhenDefeatEnemies.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }
}
