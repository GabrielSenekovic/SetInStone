using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IAttackable
{
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] GameObject bubble;

    ActivatesWhenDefeatEnemies activatesWhenDefeatEnemies;

    private void Awake()
    {
        currentHealth = maxHealth;
        Debug.Assert(bubble != null);
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
            activatesWhenDefeatEnemies?.RemoveEnemy(this);
            Instantiate(bubble, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
