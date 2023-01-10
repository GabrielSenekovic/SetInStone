using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SquirrelPriest : MonoBehaviour, IAttackable
{
    public enum Behavior
    {
        NONE = 0,
        SHOOTING = 1,
        GOING_FOR_HATCH = 2,
        HANGING = 3,
        CLIMBING = 4,
        JUMPING = 5
    }
    Behavior behavior;
    public GameObject gunProjectile;
    public GameObject acornProjectile;
    [SerializeField] Timer acornDropCounter;
    [SerializeField] Timer behaviorSwitcher;
    public Transform wallCheck;
    public float wallCheckDistance;
    [SerializeField] public LayerMask whatIsGround;
    int facingDirection;
    public int contactDamage = 1;
    public int currentHealth;
    public int maxHealth;
    Rigidbody2D body;
    public Transform acornDropPosition;

    private void Awake()
    {
        acornDropCounter.Initialize(DropAcorn);
        behaviorSwitcher.Initialize(SwitchBehavior);
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        switch(behavior)
        {
            case Behavior.NONE: behaviorSwitcher.Increment(); break;
            case Behavior.JUMPING: acornDropCounter.Increment(); CheckForWall(); break;
            case Behavior.SHOOTING: Shoot(); break;
        }
    }
    void SwitchBehavior()
    {
        int randValue = UnityEngine.Random.Range(0, 2);
        if (randValue == 0)
        {
            behavior = Behavior.JUMPING; acornDropCounter.Reset();
        }
        else if(randValue == 1)
        {
            behavior = Behavior.SHOOTING;
        }
    }
    void CheckForWall()
    {
        bool isTouchingWall = Physics2D.Raycast(wallCheck.position, new Vector2(facingDirection, 0), wallCheckDistance, whatIsGround);
        if(isTouchingWall)
        {
            LatchOntoWall();
        }
    }
    void LatchOntoWall()
    {
        behavior = Behavior.NONE; behaviorSwitcher.Reset();
    }
    void DropAcorn()
    {
        Instantiate(acornProjectile, acornDropPosition.position, Quaternion.identity, transform);
    }
    public void OnBeAttacked(int value, Vector2 dir)
    {
        currentHealth -= value;
        body.AddForce(dir, ForceMode2D.Impulse);
        Die();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IAttackable>().OnBeAttacked(contactDamage, (collision.transform.position - transform.position).normalized * 4);
        }
    }
    public void Shoot()
    {
        behavior = Behavior.NONE; behaviorSwitcher.Reset();
        float angle = 90 * facingDirection;
        for(int i = 0; i < 3; i++)
        {
            Rigidbody2D body = Instantiate(gunProjectile).GetComponentInChildren<Rigidbody2D>();
        }
    }
    public void Die()
    {
        if (currentHealth <= 0)
        {
            // Game.Instance.visualEffects.ChangePosition(deathCloud, transform.position);
            // deathCloud.effect.Play();
            gameObject.SetActive(false);
        }
    }
}
