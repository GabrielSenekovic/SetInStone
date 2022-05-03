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
    }

    void FixedUpdate()
    {
        if(doorOpening)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        if(currentPosition.y <= (startingPosition.y - transform.localScale.y)*2)
        {
            doorOpening = false;
            body.velocity = Vector2.zero;
            transform.gameObject.SetActive(false);
        }
        if(!(currentPosition.y <= (startingPosition.y - transform.localScale.y)*2))
        {
            collider.SetActive(false);
            body.AddForce(Vector3.down * moveSpeed);
            currentPosition = body.transform.position;
        }
    }
}
