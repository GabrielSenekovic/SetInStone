using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] PolygonCollider2D myCollider;
    [SerializeField] List<GameObject> enemies;
    public bool discovered;
   [SerializeField] GameObject door;
    public PolygonCollider2D GetCollider()
    {
        return myCollider;
    }

    void Update()
    {
       if(door != null && !door.GetComponent<RoomDoor>().doorOpening && enemies.Count > 0)
        {
            CheckEnemies();
        }
        if(door != null && enemies.Count == 0)
        {
            door.GetComponent<RoomDoor>().doorOpening = true;
        }
    }

    void CheckEnemies()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].GetComponentInChildren<Snail>().currentHealth <= 0)
            {
                Destroy(enemies[i]);
                enemies.Remove(enemies[i]);
                i--;
            }
        }
    }
}
