using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Timer : ScriptableObject
{
    public enum TimerBehavior
    {
        NONE = 0,
        RESET_SELF = 1
    }
    int max_value;
    int current_value;
    Action OnHitMax;
    TimerBehavior behavior;
    public Timer(Action OnHitMax, int max_value, TimerBehavior behavior = TimerBehavior.RESET_SELF)
    {
        this.max_value = max_value;
        this.OnHitMax = OnHitMax;
        current_value = 0;
    }
    public void Increment()
    {
        current_value++;
        if(current_value >= max_value)
        {
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
    public bool IsCounting()
    {
        return current_value > 0;
    }
    public bool IsLowerThanMax()
    {
        return current_value < max_value;
    }
}
