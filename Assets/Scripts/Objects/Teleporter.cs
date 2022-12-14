using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] Room room;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent.transform.position = destination.position;
            if (Game.GetCurrentCameraCollider() != room.GetCollider())
            {
                Game.SetCameraCollider(room.GetCollider());
            }
        }
    }
}
