using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class credits : MonoBehaviour
{
    
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector2.up*speed*Time.timeScale);

       // if (Input.GetKeyDown(KeyCode.Space))
        {
         //   speed = 0.25f;
        }
    }
}
