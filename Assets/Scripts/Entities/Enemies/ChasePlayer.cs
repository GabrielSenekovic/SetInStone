using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class ChasePlayer : MonoBehaviour
{
    public Transform target;
    Rigidbody2D body;
    LayerMask whatIsWater;

    public float radius;
    public float followSpeed;

    private void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        if(GetTarget())
        {
            if(IsTargetOutOfRange() && IsTargetAccessible())
            {
                target = null; return;
            }
            body.MovePosition(transform.position + (target.position - transform.position).normalized * followSpeed);
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
    bool IsTargetAccessible()
    {
        //Check if water is in the way
        return !Physics2D.LinecastAll(transform.position, target.position).Any(e => e.collider.gameObject.layer == Mathf.Log(whatIsWater.value,2));
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = target == null ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
