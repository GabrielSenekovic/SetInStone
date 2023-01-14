using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    Waterfall waterfall;
    [SerializeField]Timer closeTimer;

    private void Start()
    {
        waterfall = GetComponentInChildren<Waterfall>();
        closeTimer.Initialize(() => OpenClose(false));
    }

    private void FixedUpdate()
    {
        if(waterfall.GetOn())
        {
            closeTimer.Increment();
        }
    }

    public bool GetOn()
    {
        return waterfall.GetOn();
    }
    public void OpenClose(bool value)
    {
        waterfall.SetOn(value);
    }
}
