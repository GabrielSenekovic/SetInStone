using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
    [SerializeField] Timer shootTimer;
    [SerializeField] Timer shootCounter;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] Hatch leftHatch;
    [SerializeField] Hatch rightHatch;
    int facingDirection;
    public int contactDamage = 1;
    public int currentHealth;
    public int maxHealth;
    Rigidbody2D body;
    public Transform acornDropPosition;
    public Transform gunShootPosition;
    GameObject[] gunProjectiles = new GameObject[3];

    readonly Vector2 jumpForce = new Vector2(30, 10f);

    private void Awake()
    {
        for(int i = 0; i < gunProjectiles.Length; i++)
        {
            gunProjectiles[i] = Instantiate(gunProjectile, transform.position, Quaternion.identity);
        }
        acornDropCounter.Initialize(DropAcorn);
        behaviorSwitcher.Initialize(SwitchBehavior);
        shootTimer.Initialize(() => { }, Timer.TimerBehavior.NONE);
        shootCounter.Initialize(() => { behavior = Behavior.NONE; behaviorSwitcher.Reset();});
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
            case Behavior.GOING_FOR_HATCH: behaviorSwitcher.Increment(); break;
        }
    }
    void SwitchBehavior()
    {
        int randValue = UnityEngine.Random.Range(0, 3);
        if (randValue == 0)
        {
            behavior = Behavior.JUMPING; acornDropCounter.Reset();
            body.gravityScale = 1;
            body.AddForce(new Vector2(jumpForce.x * facingDirection, jumpForce.y), ForceMode2D.Impulse);
        }
        else if (randValue == 1 && gunProjectiles.All(g => !g.activeSelf))
        {
            behavior = Behavior.SHOOTING;
        }
        else if(randValue == 2)
        {
            if(!OpenHatch())
            {
                behavior = Behavior.NONE;
            }
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
        transform.localScale = new Vector3(facingDirection, 1, 1);
        body.velocity = Vector2.zero;
    }
    bool OpenHatch()
    {
        if(facingDirection == -1 && !rightHatch.GetOn())
        {
            rightHatch.OpenClose(true);
            behavior = Behavior.GOING_FOR_HATCH;
            behaviorSwitcher.Reset();
            return true;
        }
        else if (facingDirection == 1 && !leftHatch.GetOn())
        {
            leftHatch.OpenClose(true);
            behavior = Behavior.GOING_FOR_HATCH;
            behaviorSwitcher.Reset();
            return true;
        }
        else
        {
            return false;
        }
    }
    void DropAcorn()
    {
        Instantiate(acornProjectile, acornDropPosition.position, Quaternion.identity, null);
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
        shootTimer.Increment();
        if(shootTimer.IsFull())
        {
            gunProjectiles[shootCounter.CurrentValue].SetActive(true);
            AudioManager.PlaySFX("SnailShoot");
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), gunProjectiles[shootCounter.CurrentValue].GetComponent<Collider2D>());
            Vector2 atkPos = Game.GetPlayerPosition() - (Vector2)body.transform.position;
            gunProjectiles[shootCounter.CurrentValue].GetComponent<EnemyProjectile>().Shoot(atkPos, 10, 0, body.position);
            shootCounter.Increment();
            float angle = 90 * facingDirection;
            shootTimer.Reset();
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
