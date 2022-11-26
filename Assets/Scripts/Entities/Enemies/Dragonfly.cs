using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dragonfly : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] float speed;
    Timer movementInterval; //The timer for when it stops moving and before it moves to a new position
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;
    bool moving;
    [SerializeField] Vector2 target;
    [SerializeField] Vector2 startPosition;
    [SerializeField] Vector2 previousPosition;
    float distanceToTravel;
    public LayerMask whatIsGround;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        movementInterval = new Timer(ChooseNewDestination, 40);
        moving = true;
        startPosition = transform.position;
        previousPosition = transform.position;
    }
    private void FixedUpdate()
    {
        if (!moving)
        {
            movementInterval.Increment();
        }
        else if (CheckIfFinishedTravel())
        {
            moving = false;
            body.velocity = Vector2.zero;
        }
        else
        {
            Move();
        }
    }
    private void Move()
    {
        Vector2 destination = target - (Vector2)transform.position;
        body.velocity = destination.normalized * speed;
        transform.localScale = new Vector3(Mathf.Sign(destination.x), 1, 1);
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
        moving = true;
        previousPosition = transform.position;
        Debug.Log("Chose a new target");
    }
}
