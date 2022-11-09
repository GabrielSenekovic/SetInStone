using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    GoBackAndForth movement;
    public bool isOn;

    private void Awake()
    {
        movement = GetComponent<GoBackAndForth>();
        movement.OnAwake(false, false);
    }
    private void Update()
    {
        if(isOn)
        {
            movement.OnUpdate();
        }
    }
    private void FixedUpdate()
    {
        if(isOn)
        {
            movement.OnFixedUpdate();
        }
    }
}
