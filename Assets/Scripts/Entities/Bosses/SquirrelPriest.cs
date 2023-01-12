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
    [SerializeField]Behavior behavior;
    [SerializeField] GameObject gunProjectile;
    [SerializeField] GameObject acornProjectile;
    [SerializeField] Timer acornDropCounter;
    [SerializeField] Timer behaviorSwitcher;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance;
    [SerializeField] public LayerMask whatIsGround;
    int facingDirection;
    public int contactDamage = 1;
    public int currentHealth;
    public int maxHealth;
    Rigidbody2D body;
    public Transform acornDropPosition;

    readonly Vector2 jumpForce = new Vector2(30, 10f);

    private void Awake()
    {
        acornDropCounter.Initialize(DropAcorn);
        behaviorSwitcher.Initialize(SwitchBehavior);
        body = GetComponent<Rigidbody2D>();

        facingDirection = -1; //Is on right wall. Facing the left
        behavior = Behavior.NONE;
    }

    private void FixedUpdate()
    {
        switch(behavior)
        {
            case Behavior.NONE: behaviorSwitcher.Increment(); break;
            case Behavior.JUMPING: acornDropCounter.Increment(); CheckForWall(); break;
            case Behavior.SHOOTING: Shoot(); break;
            case Behavior.CLIMBING: Climb(); break;
        }
    }
    void SwitchBehavior()
    {
        int randValue = UnityEngine.Random.Range(0, 2);
        if (randValue == 0)
        {
            Debug.Log("Jump!");
            behavior = Behavior.JUMPING; acornDropCounter.Reset();
            body.gravityScale = 1;
            body.AddForce(new Vector2(jumpForce.x * facingDirection, jumpForce.y), ForceMode2D.Impulse);
        }
        else if(randValue == 1)
        {
            behavior = Behavior.SHOOTING;
        }
    }
    void CheckForWall()
    {
        RaycastHit2D isTouchingWall = Physics2D.Raycast(wallCheck.position, new Vector2(facingDirection, 0), wallCheckDistance, whatIsGround);
        if(isTouchingWall)
        {
            transform.position = isTouchingWall.point - new Vector2(facingDirection * wallCheckDistance, 0);
            LatchOntoWall();
        }
    }
    void Climb()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, new Vector2(facingDirection, 0), wallCheckDistance, whatIsGround);
        if (hit)
        {
            transform.position = hit.point - new Vector2(facingDirection * wallCheckDistance, 0);
        }
        body.AddForce(new Vector2(0, 0.1f), ForceMode2D.Impulse);
        if(transform.localPosition.y >= 3)
        {
            body.velocity = Vector2.zero;
            behavior = Behavior.NONE;
        }
    }
    void LatchOntoWall()
    {
        behavior = Behavior.CLIMBING; behaviorSwitcher.Reset();
        body.gravityScale = 0;
        facingDirection *= -1;
        body.velocity = Vector2.zero;
        Debug.Log("Latched onto wall");
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
