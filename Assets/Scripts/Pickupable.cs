using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    HealthModel healthModel;
    [SerializeField] int value;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(collision.GetComponent<HealthModel>())
            {
                pickUP(collision, false);
            }
            else if (collision.GetComponentInParent<HealthModel>())
            {
                pickUP(collision, true);
            }
        }
    }

    void pickUP(Collider2D collision, bool parent)
    {
        healthModel = parent ? collision.GetComponentInParent<HealthModel>() : collision.GetComponent<HealthModel>();
        if(healthModel.Damaged())
        {
            healthModel.Heal(value);
            Destroy(transform.parent.gameObject);
        }
        else if(healthModel.currentHealth == healthModel.maxHealth)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
