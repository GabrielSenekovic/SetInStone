using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class ChasePlayer : MonoBehaviour
{
    [Flags]
    public enum ChaseBehavior
    {
        NONE = 0,
        AVOID_WATER = 1,
        LEAVING = 1 << 1 //No longer chasing, moving away
    }
    Transform target;
    Rigidbody2D body;
    [SerializeField] LayerMask whatIsWater;
    [SerializeField] LayerMask whatIsGround;

    [SerializeField] float radius;
    [SerializeField] float followSpeed;
    [SerializeField] byte distanceFromGround; 

    Vector2 startPosition;

    int returnTimer = 0;
    int returnTimer_max = 40;

    public ChaseBehavior chaseBehavior;

    private void Start() 
    {
        body = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void FixedUpdate() 
    {
        Vector2 destination = Vector2.zero;
        if(GetTarget())
        {
            if(IsTargetOutOfRange() || IsTargetInaccessible())
            {
                target = null; return;
            }
            destination = (target.position - transform.position).normalized;
        }
        else
        {
            destination = (startPosition - (Vector2)transform.position).normalized;
        }
        
        if ((chaseBehavior & ChaseBehavior.LEAVING) == ChaseBehavior.LEAVING)
        {
            Leave(ref destination);
        }
        if (distanceFromGround > 0)
        {
            AvoidGround(ref destination);
        }
        Move(destination);
    }
    private void Move(Vector2 destination)
    {
        transform.localScale = new Vector3(Mathf.Sign(destination.x), 1, 1);
        body.MovePosition((Vector2)transform.position + destination * followSpeed);
    }
    void AvoidGround(ref Vector2 destination)
    {
        if (Physics2D.LinecastAll(transform.position, Vector2.down).Any(e => e.collider.gameObject.layer == Mathf.Log(whatIsGround.value, 2)))
        {
            destination += new Vector2(0, distanceFromGround);
        }
    }
    void Leave(ref Vector2 destination)
    {
        bool leaving = (chaseBehavior & ChaseBehavior.LEAVING) == ChaseBehavior.LEAVING;
        destination *= leaving ? -1 : 1;
        returnTimer++;
        if (returnTimer >= returnTimer_max)
        {
            returnTimer = 0;
            chaseBehavior &=~ ChaseBehavior.LEAVING;
            GetComponent<Collider2D>().enabled = true;
        }
    }
    bool GetTarget()
    {
        if(!target)
        {
            RaycastHit2D temp2 = Physics2D.CircleCastAll(transform.position, radius, Vector2.up).FirstOrDefault(t => t.collider.gameObject.CompareTag("Player"));
            target = temp2.collider == null? null : temp2.collider.transform;
            return target != null;
        }
        return true;
    }
    bool IsTargetOutOfRange()
    {
        return (target.position - transform.position).magnitude > radius;
    }
    bool IsTargetInaccessible()
    {
        //Check if water is in the way
        if((chaseBehavior & ChaseBehavior.AVOID_WATER) == ChaseBehavior.AVOID_WATER)
        {
            return Physics2D.LinecastAll(transform.position, target.position).Any(e => e.collider.gameObject.layer == Mathf.Log(whatIsWater.value, 2));
        }
        return false;
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = target == null ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
