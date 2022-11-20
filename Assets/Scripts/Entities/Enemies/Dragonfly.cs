using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragonfly : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] float speed;
    Timer movementInterval; //The timer for when it stops moving and before it moves to a new position
    [SerializeField] float maxDistance;
    bool moving = true;
    Vector2 destination;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        movementInterval = new Timer(ChooseNewDestination, 20);
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(!moving)
        {
            movementInterval.Increment();
        }
    }
    void ChooseNewDestination()
    {
        //use maxdistance to figure out a position it can go to, and use raycast to see if its possible
    }
}
