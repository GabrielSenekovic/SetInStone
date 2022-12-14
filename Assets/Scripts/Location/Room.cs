using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Linq;
using UnityEditor;

public class Room : MonoBehaviour
{
    [SerializeField] Collider2D myCollider;
    [SerializeField] Light2D roomLight;
    bool discovered;
    public List<Area.RoomToDoor> links = new List<Area.RoomToDoor>();

    ActivatesWhenDefeatEnemies activatesWhenDefeatEnemies;
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
        TryGetComponent(out activatesWhenDefeatEnemies);
    }
    public void SetLinkedRooms(Area.RoomToDoor[] links)
    {
        this.links = links.ToList();
    }
    public Room FetchLinkedRoom(int value) => links.Where(r => r.value == value).First().destination;

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
}
