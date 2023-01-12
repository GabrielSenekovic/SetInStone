using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    GoBackAndForth movement;
    public bool isOn;
    [SerializeField] bool isMoving;

    private void Awake()
    {
        movement = GetComponent<GoBackAndForth>();
        movement.OnAwake(false, false);
    }
    private void Update()
    {
        if(isOn && isMoving)
        {
            movement.OnUpdate();
        }
    }
    private void FixedUpdate()
    {
        if(isOn && isMoving)
        {
            movement.OnFixedUpdate();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isMoving 
            && collision.gameObject.CompareTag("Player") 
            && collision.transform.position.y > transform.position.y 
            && collision.gameObject.GetComponent<Movement>().GetGrounded())
        {
            isMoving = true;
        }
    }
}
