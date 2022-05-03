using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private void LateUpdate() 
    {
        transform.position = Camera.main.gameObject.transform.position;
    }
}