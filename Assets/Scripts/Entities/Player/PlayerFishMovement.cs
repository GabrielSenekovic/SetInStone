using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishMovement : MonoBehaviour
{
    public Transform fishPosition;
    public float movementSpeed;

    private void FixedUpdate() 
    {
        transform.position = Vector3.Lerp(transform.position, fishPosition.position, movementSpeed);
    }
}
