using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    [SerializeField] int contactDamage;
    [SerializeField] int pushStrength;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IAttackable>().OnBeAttacked(contactDamage, (collision.transform.position - transform.position).normalized * pushStrength);
        }
    }
}
