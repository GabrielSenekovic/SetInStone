using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class ChasePlayer : MonoBehaviour
{
    Transform target;
    Rigidbody2D body;
    public LayerMask whatIsWater;

    public float radius;
    public float followSpeed;

    Vector2 startPosition;

    private void Start() {
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
        transform.localScale = new Vector3(Mathf.Sign(destination.x), 1,1);
        body.MovePosition((Vector2)transform.position + destination * followSpeed);
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
        return Physics2D.LinecastAll(transform.position, target.position).Any(e => e.collider.gameObject.layer == Mathf.Log(whatIsWater.value,2));
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = target == null ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
