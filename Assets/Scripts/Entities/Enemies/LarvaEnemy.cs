using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(GoBackAndForth))]
public class LarvaEnemy : MonoBehaviour
{
    [SerializeField] int range;
    GoBackAndForth movement;
    [SerializeField] int attackCounterMax;
    float attackCounter = 0;
    [SerializeField] GameObject mouth;
    HurtPlayer hurtPlayer;
    private void Awake()
    {
        movement = GetComponent<GoBackAndForth>();
        movement.OnAwake();
        mouth.SetActive(false);
    }
    private void Update()
    {
        if(attackCounter >= attackCounterMax)
        {
            if (mouth.activeSelf) { mouth.SetActive(false); }
            movement.OnUpdate();
        }
        else
        {
            attackCounter += Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        movement.OnFixedUpdate();
        TryAttack();
    }
    void TryAttack()
    {
        if (Physics2D.LinecastAll(transform.position, (Vector2)transform.position + movement.GetDirection()).Any(e => e.collider.gameObject.CompareTag("Player")))
        {
            Attack();
        }
    }
    void Attack()
    {
        attackCounter = 0;
        mouth.SetActive(true);
    }
}
