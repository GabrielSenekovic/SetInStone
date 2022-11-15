using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Linq;

public class Room : MonoBehaviour
{
    [System.Serializable]public class RoomToDoor
    {
        public int value;
        public Room destination;
    }
    [SerializeField] Collider2D myCollider;
    [SerializeField] List<GameObject> enemies;
    public bool discovered;
    [SerializeField] GameObject door;
    [SerializeField] Light2D roomLight;
    [SerializeField] List<RoomToDoor> linkedRooms;
    public Collider2D GetCollider()
    {
        return myCollider;
    }

    void Awake()
    {
        if(!myCollider.gameObject.CompareTag("PassThrough"))
        {
            myCollider.gameObject.tag = "PassThrough";
        }
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
    public Room FetchLinkedRoom(int value) => linkedRooms.Where(r => r.value == value).First().destination;

    public void Discover()
    {
        discovered = true;
    }
    void FixedUpdate()
    {
        if(discovered && roomLight && roomLight.intensity < 1)
        {
            roomLight.intensity += 0.05f;
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
