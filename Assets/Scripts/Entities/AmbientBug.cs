using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientBug : MonoBehaviour
{
    Transform target;

    private void FixedUpdate() 
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.GetComponent<AttractBugs>())
        {
            target = other.transform;
        }
    }
}
