using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Rigidbody2D body;
    public bool doorOpening;
    public float moveSpeed;
    Vector3 startingPosition;
    Vector3 currentPosition;
    Animator anim;
    public Room roomBehind; //if roomBehind is null, then it's simply in the middle of a room

    bool jammed = false;
    public bool locked;
    public int jamCounter = 0;
    
    [SerializeField] bool open = false;
    [SerializeField] GameObject collider;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startingPosition = body.transform.position;
        currentPosition = body.transform.position;
        if (open)
        {
            doorOpening = false;
            FinishOpen();
        }
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
            gameObject.SetActive(false);
        }
        if(!(currentPosition.y <= (startingPosition.y - transform.localScale.y)*2))
        {
            anim.SetTrigger("Open");
            collider.SetActive(false);
            body.AddForce(Vector3.up * moveSpeed);
            currentPosition = body.transform.position;
            roomBehind?.Discover();
            open = true;
        }
    }
    public void FinishOpen()
    {
        body.velocity = Vector2.zero;
        body.transform.position = new Vector2(transform.position.x, (startingPosition.y - transform.localScale.y) * 2);
    }
    public void CloseDoor()
    {
        body.velocity = Vector2.zero;
        body.transform.position = startingPosition;
    }
}
