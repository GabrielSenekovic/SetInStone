using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum NiyoMovementState
{
    NONE = 0,
    GROUNDED = 1,
    JUMPING = 1 << 1,
    DUCKING = 1 << 2,

    TOUCHING_WATER = 1 << 3,
    TOUCHING_SURFACE = 1 << 4,
    SUBMERGED = 1 << 5,

    WADING = GROUNDED | TOUCHING_SURFACE,

    LEDGE_DETECTED = 1 << 6,
    LEDGE_HANGING = 1 << 7,
    FORCE_LEDGE_CLIMB = 1 << 8,

    ONFIRE = 1 << 9,

    IS_FLUNG = 1 << 10, //Used for hookshotting. If moving in the opposite direction of velocity you break it but if you move in the same direction nothing happens

    DISMOUNT_REQUEST = 1 << 11,
    SLIDE_REQUEST = 1 << 12,

    ACTIONBUFFER = 1 << 13
}
public static class NiyoMovementStateExtensions
{
    public static bool CanMove(this NiyoMovementState state)
    {
        return state.HasFlag(NiyoMovementState.ACTIONBUFFER);
    }
    public static NiyoMovementState ExitWater(this NiyoMovementState state)
    {
        state &= ~NiyoMovementState.SUBMERGED;
        state &= ~NiyoMovementState.TOUCHING_WATER;
        state &= ~NiyoMovementState.TOUCHING_SURFACE;
        return state;
    }
    public static bool IsGrounded(this NiyoMovementState state)
    {
        return state.HasFlag(NiyoMovementState.GROUNDED);
    }
    public static bool IsLedgeHanging(this NiyoMovementState state)
    {
        return state.HasFlag(NiyoMovementState.LEDGE_HANGING);
    }
    public static bool IsSubmerged(this NiyoMovementState state)
    {
        return state.HasFlag(NiyoMovementState.SUBMERGED);
    }
    public static bool IsTouchingSurface(this NiyoMovementState state)
    {
        return state.HasFlag(NiyoMovementState.TOUCHING_SURFACE);
    }
    public static NiyoMovementState Submerge(this NiyoMovementState state)
    {
        state |= NiyoMovementState.SUBMERGED;
        state &= ~NiyoMovementState.TOUCHING_SURFACE;
        Debug.Log("Submerge");
        return state;
    }
}
