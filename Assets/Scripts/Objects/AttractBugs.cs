using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractBugs : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("FlyingBug"))
        {
            
        }
    }
}
