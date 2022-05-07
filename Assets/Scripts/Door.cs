using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Room myRoom; //The room it transitions to
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.GetComponent<Movement>())
        {
            collision.transform.parent.GetComponent<HealthModel>().safePos = collision.transform.position;
            if (Game.GetCurrentCameraCollider() != myRoom.GetCollider())
            {
                Game.SetCameraCollider(myRoom.GetCollider());
            }
        }
    }
}
