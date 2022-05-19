using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour
{
    public Movement movement;
    public void ClimbLedge()
    {
        movement.FinishLedgeClimb();
    }
    public void PlayCaneVFX()
    {
        movement.PlayCaneVFX();
    }
}
