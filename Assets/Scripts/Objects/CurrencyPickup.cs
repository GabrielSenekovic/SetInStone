using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    [SerializeField] Inventory.Currency currency;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<Inventory>().AddCurrency(Inventory.Currency.ACORN, 1);
            Destroy(gameObject);
        }
    }
}
