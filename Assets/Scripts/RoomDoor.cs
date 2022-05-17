using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    Rigidbody2D body;
    public bool doorOpening;
    public float moveSpeed;
    Vector3 startingPosition;
    Vector3 currentPosition;
    Animator anim;
    public Room roomBehind;

    bool jammed = false;
    public bool locked;
    public int jamCounter = 0;
    bool open = false;

    [SerializeField] GameObject collider;

    void Awake()
    {
        doorOpening = false;
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        startingPosition = body.transform.position;
        currentPosition = body.transform.position;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(doorOpening)
        {
            OpenDoor();
        }
    }
    public bool CanOpen()
    {
        return !jammed && !locked && !open;
    }
    public void Jam()
    {
        jammed = true;
        jamCounter++;
    }
    public void UnJam()
    {
        jamCounter--;
        if(jamCounter <= 0)
        {
            jammed = false;
        }
    }

    public void OpenDoor()
    {
        if(currentPosition.y <= (startingPosition.y - transform.localScale.y)*2)
        {
            doorOpening = false;
            body.velocity = Vector2.zero;
            transform.gameObject.SetActive(false);
        }
        if(!(currentPosition.y <= (startingPosition.y - transform.localScale.y)*2))
        {
            anim.SetTrigger("Open");
            collider.SetActive(false);
            body.AddForce(Vector3.up * moveSpeed);
            currentPosition = body.transform.position;
            roomBehind.Discover();
            open = true;
        }
    }
}
