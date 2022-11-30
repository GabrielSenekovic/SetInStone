using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dragonfly : MonoBehaviour
{
    public enum Behavior
    {
        NONE,
        MOVING, //Moving, but also when pursuing player. The target is different 
        WAITING, //Between moves
        CHARGING, //Charge up fire
    }
    Rigidbody2D body;
    [SerializeField] float speed;
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;
    [SerializeField] Behavior behavior;
    Vector2 target;
    Vector2 startPosition;
    Vector2 previousPosition;
    float distanceToTravel;
    public LayerMask whatIsGround;
    Transform player;

    [SerializeField]Timer movementTimer; //The timer for when it stops moving and before it moves to a new position
    [SerializeField]Timer attackTimer; //The timer for how often the dragonfly will try to attack the player
    [SerializeField]Timer chargeTimer; //The timer for how long it takes the fire to charge up

    [SerializeField] GameObject projectilePrefab;
    Rigidbody2D projectile;
    Animator projectileAnim;
    [SerializeField] Transform holdingTransform;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        movementTimer.Initialize(ChooseNewDestination);
        attackTimer.Initialize(StartCharge);
        chargeTimer.Initialize(StartPursue);

        behavior = Behavior.MOVING;
        startPosition = transform.position;
        previousPosition = transform.position;

        projectile = Instantiate(projectilePrefab, Vector2.zero, Quaternion.identity, holdingTransform).GetComponent<Rigidbody2D>();
        projectile.gameObject.SetActive(false);
        projectile.isKinematic = true;
        projectileAnim = projectile.GetComponentInChildren<Animator>();

        player = Game.GetCurrentPlayer().transform;
    }
    private void FixedUpdate()
    {
        switch(behavior)
        {
            case Behavior.MOVING:
                if (CheckIfFinishedTravel())
                {
                    if(projectile.gameObject.activeSelf)
                    {
                        DropProjectile();
                    }
                    behavior = Behavior.WAITING;
                    body.velocity = Vector2.zero;
                }
                else
                {
                    Move();
                }
                attackTimer.Increment();
                break;
            case Behavior.CHARGING:
                chargeTimer.Increment();
                break;
            case Behavior.WAITING:
                movementTimer.Increment();
                attackTimer.Increment();
                break;
            default: break;
        }
    }
    void Move()
    {
        Vector2 destination = target - (Vector2)transform.position;
        body.velocity = destination.normalized * speed;
        transform.localScale = new Vector3(Mathf.Sign(destination.x), 1, 1);
    }
    void StartCharge()
    {
        projectile.transform.parent = holdingTransform;
        projectile.transform.localPosition = Vector2.zero;
        projectile.velocity = Vector2.zero;

        projectile.isKinematic = true;
        projectile.gravityScale = 0;
        projectile.gameObject.SetActive(true);
        behavior = Behavior.CHARGING;
        body.velocity = Vector2.zero;

        projectileAnim.SetBool("Falling", false);
    }
    void StartPursue()
    {
        behavior = Behavior.MOVING;
        target = player.position + new Vector3(0, 4, 0);
        previousPosition = transform.position;
        distanceToTravel = Vector2.Distance(transform.position, target);
    }
    void DropProjectile()
    {
        projectile.transform.parent = null;
        projectile.gravityScale = 1;
        projectile.isKinematic = false;
        projectileAnim.SetBool("Falling", true);
    }
    bool CheckIfFinishedTravel()
    {
        float distanceTravelled = Vector2.Distance(previousPosition, transform.position);
        return distanceTravelled >= distanceToTravel;
    }
    void ChooseNewDestination()
    {
        //Gets a new position within the range of the start position and sees if it can move there from the current position. Otherwise, choose new position
        int buffer = 0;
        do
        {
            Vector2 direction = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;
            distanceToTravel = Random.Range(minDistance, maxDistance);
            direction *= new Vector2(1, 0.05f);
            target = direction * distanceToTravel;
            distanceToTravel = Vector2.Distance(transform.position, target);
            target += startPosition;
            buffer++;
            if(buffer >= 100)
            {
                Debug.Log("Something went wrong with the dragonfly");
                Destroy(gameObject);
                return;
            }
        }
        while (Physics2D.LinecastAll(target, transform.position).Any(e => e.collider.gameObject.layer == Mathf.Log(whatIsGround.value, 2)));
        behavior = Behavior.MOVING;
        previousPosition = transform.position;
    }
}
