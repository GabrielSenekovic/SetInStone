using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Input : MonoBehaviour
{
    PlayerControls controls;
    Movement movement;
    PlayerAttack attack;
    Animator playerAnimator;

    Inventory inventory;

    bool controllable;

    [System.NonSerialized] public HookShot hookShot;
    [System.NonSerialized] public Pulka pulka;

    [SerializeField]bool debug;

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerAnimator.SetBool("walking", false);
        movement = GetComponent<Movement>();
        attack = GetComponent<PlayerAttack>();
        hookShot = GetComponent<HookShot>();
        pulka = GetComponent<Pulka>();
        inventory = GetComponent<Inventory>();

        controllable = true;
    }

    private void Update() 
    {
        Aim();
    }

    void Aim()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue(); 
        mousePosition.z = 18; // cam z *-1

        mousePosition = Game.Instance.cameraFollowsMainCamera.ScreenToWorldPoint(mousePosition);
        mousePosition -= transform.position;
        //Put the position of the mouse in world space relative to the players position

        if (movement.GetGrounded())
        {
            //*Then limit the position to playerpositions y
            mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Mathf.Infinity);
        }

        movement.hookShot.Aim(mousePosition); //Give it to hookshot / pulka / attack
        attack.AimAttack(mousePosition);
        movement.pulka.Aim(mousePosition);
    }
    
    private void OnMove(InputValue value) //! input stuff
    {
        if (!controllable || movement.actionBuffer) {return;}
        if(!movement.submerged){playerAnimator.SetBool("walking", true);}
        movement.movingDirection = value.Get<float>();
        movement.FaceMovingDirection();
    }
    void OnDEBUGRESET()
    {
        movement.health.ReturnToSafe();
    }
    void OnStopMove()
    {
        if(debug){Debug.Log("Stopping Movement");}
        movement.movingDirection = 0;
        if(!movement.submerged){playerAnimator.SetBool("walking", false);}
    }
    void OnMoveVertical(InputValue value)
    {
        movement.verticalDirection = value.Get<float>();
    }
    void OnStopMoveVertical()
    {
        movement.verticalDirection = 0;
    }
    void OnZoom(InputValue value)
    {
        Game.Instance.Zoom(value.Get<Vector2>());
    }

    private void OnAttack()
    {
        if (!controllable || movement.actionBuffer || movement.hangingFromLedge) {return;}
        if(!movement.GetGrounded() && attack.Attack()) {playerAnimator.SetTrigger("attack"); movement.StopVelocity();}
    }

    void OnSpecial()
    {
        if (!controllable || movement.actionBuffer || !inventory.HasHookshot() || movement.submerged) {return;}
        if(movement.hookShot.Shoot()) {movement.StopVelocity();}
        movement.FaceMovingDirection();
    }
    void OnStopSpecial()
    {
        if(!inventory.HasHookshot()){return;}
        movement.hookShot.StopPull();
    }

    void OnPulka()
    {
        if (!controllable || movement.actionBuffer || !inventory.HasPulka()) {return;}

        if(debug){Debug.Log("Using Pulka");}

        if(movement.ducking)
        {
            playerAnimator.SetBool("sitting", true);
            pulka.SetState(Pulka.PulkaState.SITTING, movement);
        }
        else
        {
            pulka.SetState(Pulka.PulkaState.SHIELD, movement);
        }
    }

    void OnDismount() // if you are sitting and you "dismount" you request dismount and set free rotation. and you arent ducking?
    {
        if(pulka.GetState() == Pulka.PulkaState.SITTING)
        {
            if(debug){Debug.Log("Put in dismount request");}
            movement.dismountRequest = true;
            movement.SetCantRotate(true);
            movement.ResetRotation();
            movement.ducking = false;
        }
        else // if you arent sitting then dismount? maybe the state is changed right before
        {
            pulka.Dismount();
            movement.ResetGroundCheck();
            movement.ducking = false;
            playerAnimator.SetBool("sitting", false);
        }
    }

    void OnDuck()
    {
        if (!controllable || movement.actionBuffer) {return;}
        if(debug){Debug.Log("Duck");}
        movement.ducking = true;
    }

    void OnStandUp()
    {
        if(pulka.GetState() != Pulka.PulkaState.SITTING)
        movement.ducking = false;
        AudioManager.PlaySFX("SitOnSled");
    }

    void OnPause()
    {
        Game.Instance.Pause();
    }

    void OnMenu()
    {
        /*Game.Instance.Menu();*/
    }

    private void OnJump()
    {
        if(!controllable) { return; }
        movement.RequestJump();
    }

    private void OnStopJump()
    {
        movement.StopJump();
    }

    void OnNavigate(InputValue value)
    {
        if(Game.Instance.mapCanvas.alpha == 1)
        {
            Game.Instance.mapScript.SetMovement(value.Get<Vector2>());
        }
    }

    public void OnEnable() // ! 🤔 i wonder...
    {
        if (controls == null)
        {
            controls = new PlayerControls();
            // Tell the "gameplay" action map that we want to get told about
            // when actions get triggered.
        }
        controls.Enable();
    }
    void OnDisable() // ! 🤔 i wonder...
    {
        controls.Disable();
    }

    void OnDebugDecrease()
    {
        GetComponent<HealthModel>().TakeDamage(1);
    }

    void OnDebugIncrease()
    {
        GetComponent<HealthModel>().Heal(1);
    }

    public void SetControllable(bool value)
    {
        controllable = value;
    }
}
