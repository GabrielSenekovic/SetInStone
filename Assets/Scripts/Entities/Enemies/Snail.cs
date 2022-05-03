using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Snail : MonoBehaviour, Attackable
{
    public int atkCooldown;
    int timeSinceAttack;
    public Vector3 moveDir;
    public float moveForce = 0f;
    public int collisionTimerMax;
    public int collisionTimer;
    public int currentHealth;
    public int maxHealth;
    Vector2 atkPos;
    float atkAngle;
    public float atkSpeed;
    Rigidbody2D body = null;
    public GameObject atkPrefab;
    GameObject atkProjectile;
    bool shot = false;
    [SerializeField] float atkRadius;
    [SerializeField] public GameObject bubble;

    [SerializeField] GameObject visuals;

    [SerializeField]VisualEffectEntry deathCloud;

    public VisualEffect VFX_prefab;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        moveDir = ChooseDir();
        collisionTimer = collisionTimerMax;
        currentHealth = maxHealth;
        timeSinceAttack = atkCooldown;
        atkProjectile = Instantiate(atkPrefab, transform.position, Quaternion.identity);
        atkProjectile.GetComponent<EnemyProjectile>().snailScript = this;
        atkProjectile.SetActive(false);

        deathCloud.effect = Instantiate(VFX_prefab, transform.position, Quaternion.identity, transform);
        Game.Instance.visualEffects.Add(deathCloud, false);
    }

    void Update()
    {
        SnailMove();
    }

    void FixedUpdate()
    {
        if(timeSinceAttack == atkCooldown && !shot)
            {SnailAttack();}
        else if(timeSinceAttack == atkCooldown && shot)
            {shot = false;}
        else if(timeSinceAttack < atkCooldown && shot)
            {timeSinceAttack++;}
    }

    public void SnailAttack()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(body.position, atkRadius, LayerMask.GetMask("Entity"), 
            body.transform.position.z, body.transform.position.z);

        foreach(Collider2D obj in hitObjects)
        {
            if(obj.CompareTag("Player"))
            {
                atkProjectile.SetActive(true);
                AudioManager.PlaySFX("SnailShoot");
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), atkProjectile.GetComponent<Collider2D>());
                atkPos = obj.transform.position - body.transform.position;
                atkProjectile.GetComponent<EnemyProjectile>().Shoot(atkPos, atkSpeed, atkAngle, body.position);
                shot = true;
                timeSinceAttack = 0;
            }
        }
    }

    public void OnBeAttacked(int value)
    {
        currentHealth -= value;
        if(currentHealth == 0)
        {
            Game.Instance.visualEffects.ChangePosition(deathCloud, transform.position);
            deathCloud.effect.Play();
            Instantiate(bubble, transform.position, transform.rotation);
            AudioManager.PlaySFX("SnailDeath");
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void SnailMove()
    {
        body.velocity = moveDir * moveForce;

        visuals.transform.position = new Vector2(body.transform.position.x, body.transform.position.y);

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(body.position, 30, LayerMask.GetMask("Entity"), 
            body.transform.position.z, body.transform.position.z);

        foreach(Collider2D obj in hitObjects)
        {
            if(obj.GetComponent<Snail>())
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>(), true);
            }
        }

        Collider2D[] terrainCheck;

        if(transform.rotation.z <= 0)
        {
            terrainCheck = Physics2D.OverlapCircleAll(new Vector2(body.position.x-transform.localScale.x, body.position.y-transform.localScale.y), 0.5f, LayerMask.GetMask("Ground"), 
            body.transform.position.z, body.transform.position.z);
        }
        else
        {
            terrainCheck = Physics2D.OverlapCircleAll(new Vector2(body.position.x+transform.localScale.x, body.position.y-transform.localScale.y), 0.5f, LayerMask.GetMask("Ground"), 
            body.transform.position.z, body.transform.position.z);
        }

        if(terrainCheck.Length == 0 && (collisionTimer == collisionTimerMax))
        {
            SwitchDirection();
        }

        if(collisionTimer < collisionTimerMax)
        {
            collisionTimer++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Player") && (collisionTimer == collisionTimerMax))
        {
            SwitchDirection();
        }
    }

    void SwitchDirection()
    {
        moveDir = -moveDir;
        gameObject.GetComponentInParent<Transform>().localScale.Set
        (gameObject.GetComponentInParent<Transform>().localScale.x, 
        gameObject.GetComponentInParent<Transform>().localScale.y, 
        -gameObject.GetComponentInParent<Transform>().localScale.z);
        collisionTimer = 0;
    }

    Vector3 ChooseDir()
    {
        int j = Random.Range(0, 2);
        Vector3 temp = new Vector3();
        if(j == 0)
        {
            temp = transform.right;
        }
        else
        {
            temp = -transform.right;
        }

        return temp;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if(body != null)
        {
            Gizmos.DrawWireSphere(body.transform.position, atkRadius);
        }
    }
}
