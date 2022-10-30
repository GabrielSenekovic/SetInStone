using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Input : MonoBehaviour
{
    PlayerControls controls;
    Movement movement;
    ITool tool1;
    ITool tool2;
    Animator playerAnimator;

    Inventory inventory;

    bool controllable;

    [System.NonSerialized] public HookShot hookShot;
    [System.NonSerialized] public Pulka pulka;
    [SerializeField] InputChange inputChange;

    [SerializeField]bool debug;
    [SerializeField] GameObject aimArrow;

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerAnimator.SetBool("walking", false);
        movement = GetComponent<Movement>();
        tool1 = GetComponent<Cane>();
        tool2 = GetComponent<Slap>();
        hookShot = GetComponent<HookShot>();
        pulka = GetComponent<Pulka>();
        inventory = GetComponent<Inventory>();

        controllable = true;
    }

    private void Update() 
    {
        Aim();
    }

    void Aim() //Aiming with a mouse
    {
        if(inputChange.currentScheme == "KeyboardMouse")
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
            tool1.Aim(mousePosition);
            tool2.Aim(mousePosition);
            movement.pulka.Aim(mousePosition);

            Vector2 dir = mousePosition.normalized;

            aimArrow.transform.localPosition = (Vector3)dir * 3;
            float angle = Vector2.Angle(Vector2.up, dir);
            aimArrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    void OnAim(InputValue value) //Aiming with a controller
    {
        Vector2 dir = value.Get<Vector2>().normalized;
        tool1.SetAngle(dir);
        tool2.SetAngle(dir);
        movement.hookShot.SetAngle(dir);
        aimArrow.transform.localPosition = (Vector3)dir * 3;
        float angle = Vector2.Angle(Vector2.up, dir);
        aimArrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    
    private void OnMove(InputValue value) //! input stuff
    {
        if (!controllable || movement.HasFlag(NiyoMovementState.ACTIONBUFFER)) {return;}
        if(!movement.IsSubmerged()){playerAnimator.SetBool("walking", true);}
        movement.SetMovingDirection(value.Get<float>());
        movement.FaceMovingDirection();
    }
    void OnDEBUGRESET()
    {
        movement.health.ReturnToSafe();
    }
    void OnStopMove()
    {
        if(debug){Debug.Log("Stopping Movement");}
        movement.SetMovingDirection(0);
        if(!movement.IsSubmerged()){playerAnimator.SetBool("walking", false);}
    }
    void OnMoveVertical(InputValue value)
    {
        movement.SetVerticalDirection(value.Get<float>());
    }
    void OnStopMoveVertical()
    {
        movement.SetVerticalDirection(0);
    }
    void OnZoom(InputValue value)
    {
        if(GameMenu.Instance != null)
        {
            GameMenu.Instance.Zoom(value.Get<Vector2>());
        }
    }

    private void OnAttack()
    {
        if (!controllable || movement.HasFlag(NiyoMovementState.ACTIONBUFFER) || movement.HasFlag(NiyoMovementState.LEDGE_HANGING)) {return;}
        if(!movement.GetGrounded() && tool1.Use()) 
        {
            playerAnimator.SetTrigger("attack"); 
            movement.StopVelocity();
        }
        else if(movement.GetGrounded())
        {
            tool2.Use();
        }
    }

    void OnSpecial()
    {
        if (!controllable || !inventory.HasHookshot() || movement.IsSubmerged()) {return;}
        movement.hookShot.Activate();
        // if(movement.hookShot.Shoot()) {movement.StopVelocity();}
        movement.FaceMovingDirection();
    }
    void OnStopSpecial()
    {
        if(!inventory.HasHookshot()){return;}
        //movement.hookShot.StopPull();
        if (movement.hookShot.Release()) { movement.StopVelocity(); }
    }

    void OnPulka()
    {
        if (!controllable || movement.HasFlag(NiyoMovementState.ACTIONBUFFER) || !inventory.HasPulka()) {return;}

        if(debug){Debug.Log("Using Pulka");}

        if(movement.IsDucking())
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
            movement.AddFlag(NiyoMovementState.DISMOUNT_REQUEST);
            movement.SetCantRotate(true);
            movement.ResetRotation();
        }
        else // if you arent sitting then dismount? maybe the state is changed right before
        {
            pulka.Dismount();
            movement.ResetGroundCheck();
            playerAnimator.SetBool("sitting", false);
        }
        movement.RemoveFlag(NiyoMovementState.DUCKING);
    }

    void OnDuck()
    {
        if (!controllable || movement.HasFlag(NiyoMovementState.ACTIONBUFFER)) {return;}
        if(debug){Debug.Log("Duck");}
        movement.Duck();
    }

    void OnStandUp()
    {
        if(pulka.GetState() != Pulka.PulkaState.SITTING)
        movement.RemoveFlag(NiyoMovementState.DUCKING);
        AudioManager.PlaySFX("SitOnSled");
    }

    void OnPause()
    {
        if(GameMenu.Instance == null) { return; }
        GameMenu.Instance.Pause();
    }

    void OnMap()
    {
        GameMenu.Instance.Map();
    }

    private void OnJump()
    {
        if(!controllable) { return; }
        if (hookShot.state == HookShot.HookShotState.None && hookShot.hit)
        {
            hookShot.LetGo();
        }
        movement.RequestJump();
        
    }

    private void OnStopJump()
    {
        movement.StopJump();
    }

    void OnNavigate(InputValue value)
    {
        if(GameMenu.Instance && GameMenu.Instance.mapCanvas.alpha == 1)
        {
            GameMenu.Instance.mapScript.SetMovement(value.Get<Vector2>());
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
