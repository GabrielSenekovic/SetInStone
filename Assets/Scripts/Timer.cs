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
        RESET_SELF = 1,
        START_FULL = 2
    }
    [SerializeField]int max_value;
    int current_value;
    Action OnHitMax;
    Action OnChange;
    TimerBehavior behavior;

    public int MaxValue => max_value;
    public int CurrentValue => current_value;
    public void Initialize(Action OnHitMax, TimerBehavior behavior = TimerBehavior.RESET_SELF, Action OnChange = null)
    {
        this.OnHitMax = OnHitMax;
        this.OnChange = OnChange;
        this.behavior = behavior;
        if(behavior == TimerBehavior.START_FULL)
        {
            current_value = max_value;
        }
        else
        {
            current_value = 0;
        }
        OnChange?.Invoke();
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
        OnChange?.Invoke();
    }
    public void Subtract(int value)
    {
        current_value = current_value - value < 0 ? 0 : current_value - value;
        OnChange?.Invoke();
    }
    public void Add(int value)
    {
        current_value = current_value + value > max_value ? max_value : current_value + value;
        OnChange?.Invoke();
    }
    public void Reset()
    {
        current_value = 0;
        OnChange?.Invoke();
    }
    public void IncreaseMax(int value)
    {
        max_value += value;
        current_value = max_value;
        OnChange?.Invoke();
    }
    public bool IsCounting() => current_value > 0;
    public bool IsLowerThanMax() => current_value < max_value;
    public bool IsFull() => current_value > max_value;

    public bool IsEmpty() => current_value <= 0;
}
