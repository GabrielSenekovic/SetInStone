using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCrab : MonoBehaviour
{
    GoBackAndForth movement;

    private void Awake()
    {
        movement = GetComponent<GoBackAndForth>();
        movement.OnAwake(false, false);
    }

    private void Update()
    {
        movement.OnUpdate();
    }

    private void FixedUpdate()
    {
        movement.OnFixedUpdate();
    }
}
