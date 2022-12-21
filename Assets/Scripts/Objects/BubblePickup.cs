using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BubblePickup : MonoBehaviour, IPickupable
{
    HealthModel healthModel;
    [SerializeField] int value;

    private void FixedUpdate() 
    {
        Collider2D hit = Physics2D.OverlapCircleAll(transform.position, 0.5f).FirstOrDefault(c => c.CompareTag("Player"));
        if(hit != null)
        {
            PickUp(hit);
        }
    }

    public void PickUp(Collider2D collision)
    {
        healthModel = collision.GetComponentInParent<HealthModel>();
        Timer healthCounter = healthModel.GetCounter();
        if(healthCounter.IsFull())
        {
            return;
        }
        if(healthModel.Damaged())
        {
            healthModel.Heal(value);
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
