using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Timer 
{
    public enum TimerBehavior
    {
        NONE = 0,
        RESET_SELF = 1
    }
    [SerializeField]int max_value;
    int current_value;
    Action OnHitMax;
    TimerBehavior behavior;
    public void Initialize(Action OnHitMax, TimerBehavior behavior = TimerBehavior.RESET_SELF)
    {
        this.OnHitMax = OnHitMax;
        current_value = 0;
        this.behavior = behavior;
    }
    public void Increment()
    {
        current_value++;
        if(current_value >= max_value)
        {
            Debug.Log(behavior);
            if(behavior == TimerBehavior.RESET_SELF)
            {
                current_value = 0;
            }
            OnHitMax();
        }
    }
    public void Reset()
    {
        current_value = 0;
    }
    public bool IsCounting() => current_value > 0;
    public bool IsLowerThanMax() => current_value < max_value;
    public bool IsFull() => current_value > max_value;
}
