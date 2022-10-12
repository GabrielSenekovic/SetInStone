using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulka : MonoBehaviour
{
    public enum PulkaState
    {
        NONE,
        SHIELD,
        SITTING
    }
    public float pulkaAngle;
    public Vector2 pulkaDir;
    public float pulkaDistanceRad;
    [SerializeField] GameObject pulka;
    [SerializeField] Vector2 sittingPosition;
    Vector2 pulkaGroundCheck = new Vector2(0, -0.5f);

    PulkaState state;
    public PulkaState GetState()
    {
        return state;
    }
    public void SetState(PulkaState value, Movement movement)
    {
        state = value;
        switch(value)
        {
            case PulkaState.NONE: Dismount(); break;
            case PulkaState.SITTING:
                movement.SetGroundCheck(pulkaGroundCheck);
                movement.SetCantRotate(false);
                movement.AddFlag(Movement.NiyoMovementState.SLIDE_REQUEST);
                movement.SetGrounded(false);
                break;
            case PulkaState.SHIELD: AudioManager.PlaySFX("Shield"); break;
        }
    }

    void Start()
    {
        state = PulkaState.NONE;
        pulka.SetActive(false);
    }

    public void Aim(Vector2 mousePosition) //! input stuff
    {
        pulkaDir = mousePosition;
        if(mousePosition.x != 0) 
        {
            pulkaAngle = mousePosition.y < 0 ? -1 * Vector2.Angle(Vector2.right, mousePosition) 
                : Vector2.Angle(Vector2.right, mousePosition);
        }
        else {pulkaAngle = 90 * Mathf.Sign(mousePosition.y);}
    }

    public void Use()
    {
        pulka.SetActive(true);
        if(state == PulkaState.SITTING)
        {
            //Sit down in the Pulka
            pulka.transform.localPosition = sittingPosition;
            pulka.GetComponentInChildren<BoxCollider2D>().isTrigger = false;
            pulka.transform.localRotation = Quaternion.identity;
        }
        else
        {
            //Use shield
            pulka.GetComponentInChildren<BoxCollider2D>().isTrigger = true;
            pulka.transform.localPosition = (new Vector2(Mathf.Cos(pulkaAngle * Mathf.Deg2Rad), 
                Mathf.Sin(pulkaAngle * Mathf.Deg2Rad)) * pulkaDistanceRad);

            pulka.transform.localRotation = Quaternion.Euler(0, 0, pulkaAngle + 90);
        }
    }
    public void TriggerDismount(Movement movement)
    {
        if (movement.HasFlag(Movement.NiyoMovementState.DISMOUNT_REQUEST) && movement.GetGrounded()) //* triggers when on the pulka on ground, so that you get pushed up
        {
            movement.GetBody().AddForce(new Vector2(0, movement.jumpForce), ForceMode2D.Impulse);
            movement.SetGrounded(false);
            movement.RemoveFlag(Movement.NiyoMovementState.DISMOUNT_REQUEST);
            SetState(PulkaState.NONE, movement);
            movement.ResetGroundCheck();
            movement.playerAnimator.SetBool("sitting", false);
        }
    }
    public void Dismount()
    {
        //*If you stop holding the shield, it goes away. Dont set it riding pulka, because it should dismount only when you hit the ground
        state = PulkaState.NONE;
        pulka.SetActive(false);
    }
}
