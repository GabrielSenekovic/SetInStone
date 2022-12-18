using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Movement movement;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Water"))
        {
            movement.EnterWater();
        }
        if(other.GetComponent<Waterfall>())
        {
            movement.PutOutFire();
        }
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Water"))
        {
            movement.ExitWater();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.TryGetComponent<IFlammable>(out IFlammable flammable) && movement.OnFire() && !flammable.OnFire())
        {
            flammable.SetOnFire();
        }
    }
}
