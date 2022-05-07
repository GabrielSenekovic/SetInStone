using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{
    float length, startPos;
    public float parallaxFactor;
    public bool loop;

    private void Start() {
        startPos = transform.position.x;
        if(GetComponent<SpriteRenderer>())
        {
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }
        else if(GetComponent<TilemapRenderer>())
        {
            length = GetComponent<TilemapRenderer>().bounds.size.x;
        }

    }

    private void Update() {
       // float temp = cam.transform.position.x * (1 - parallaxFactor);

        float distance = Camera.main.transform.position.x * parallaxFactor;
        Vector3 newPosition = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        transform.position = newPosition;

       /* if(loop && temp > startPos + (length / 2))
        {
            startPos += length;
        }
        else if(loop && temp < startPos - (length / 2))
        {
            startPos -= length;
        }*/
    }
}
