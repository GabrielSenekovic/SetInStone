using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPickup : MonoBehaviour, IPickupable
{
    [SerializeField] Inventory.Currency currency;

    public void PickUp(Collider2D collision)
    {
        collision.gameObject.GetComponentInParent<Inventory>().AddCurrency(Inventory.Currency.ACORN, 1);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickUp(collision);
        }
    }
}
