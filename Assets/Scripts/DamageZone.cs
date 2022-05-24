using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    Vector3 pos;

    void Start()
    {
        pos = transform.position;
    }
    // Start is called before the first frame update
    public int contactDamage = 12;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 colPos = collision.transform.position;
        bool sitting = false;
        Pulka temp;
        if(temp = collision.gameObject.GetComponent<Pulka>())
        {
            sitting = ((int)temp.state == 2);
        }

        if (collision.gameObject.GetComponent<Movement>() && !(pos.y < colPos.y && sitting))
        {
            collision.gameObject.GetComponent<Attackable>().OnBeAttacked(contactDamage, Vector2.zero);
            //collision.gameObject.GetComponent<HealthModel>().ReturnToSafe();
            // maybe teleport
        }
    }
}
