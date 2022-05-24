using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pickupable : MonoBehaviour
{
    HealthModel healthModel;
    [SerializeField] int value;

    private void FixedUpdate() 
    {
        Collider2D hit = Physics2D.OverlapCircleAll(transform.position, 0.5f).FirstOrDefault(c => c.CompareTag("Player"));
        if(hit != null)
        {
            pickUP(hit);
        }
    }

    void pickUP(Collider2D collision)
    {
        healthModel = collision.GetComponentInParent<HealthModel>();
        if(healthModel.currentHealth == healthModel.maxHealth)
        {
            return;
        }
        if(healthModel.Damaged())
        {
            healthModel.Heal(value);
            Destroy(gameObject);
        }
        else if(healthModel.currentHealth == healthModel.maxHealth)
        {
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
