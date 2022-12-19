using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.VFX;
using System.Linq;
using System;
public class Movement : MonoBehaviour
{
    [SerializeField] NiyoMovementState movementState;
    public Animator playerAnimator;
    public Animator bubbleAnimator;
    public float cancelJumpSpeed = 5.0f;

    Rigidbody2D body;
    [System.NonSerialized] public HookShot hookShot;
    [System.NonSerialized] public Pulka pulka;

    [System.NonSerialized] public const float groundedRadius = .05f;
    [System.NonSerialized] public float gravityVelocity;
    float movingDirection = 0; //The direction you're currently moving in. If not moving, the direction is 0
    float verticalDirection = 0; //This is only used when swimming! If not swimming, vertical directon is 0
    [System.NonSerialized] public float facingDirection = 0; //The direction you last looked in. It's set to something even if not moving
    
    [System.NonSerialized] public int amntOfJumps = 0;
    [System.NonSerialized] public int jumpTimer = 0;
    [System.NonSerialized] public int jumpBufferTimer = 0;
    [SerializeField] public int jumpBufferMax; //This timer should count down to 0

    [System.NonSerialized] public float normGrav;

    Vector2 caneGroundCheck = new Vector2(0, -0.2f);
    [SerializeField] public float gravityModifier; //Used to increase the gravity when falling
    [SerializeField] public float jumpForce;
    [SerializeField] public float airJumpForce;
    [SerializeField] public float speed;
    [SerializeField] public float swimmingSpeed;
    [SerializeField] public float swimmingTurnSpeed;
    [SerializeField] public int jumpLimit;
    [SerializeField] public int amntOfJumpsMax;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public LayerMask whatIsWater;

    MovementDebug movementDebug;

    public Transform bodyTransform;

    public ParticleSystem doubleJump;
    public ParticleSystem caneVFX;

    [SerializeField] Transform groundCheck;
    public Transform ledgeCheck;
    public Transform wallCheck;
    public Transform surfaceCheck; //Will check if there's air over the water 

    public float wallCheckDistance;
    public float surfaceCheckDistance;
    public Vector2 ledgePosBot;
    public Vector2 ledgePos1;
    public Vector2 ledgePos2;
    public float ledgeClimbXOffset1;
    public float ledgeClimbYOffset1;
    public float ledgeClimbXOffset2;
    public float ledgeClimbYOffset2;

    public Collider2D mainCollider;

    [SerializeField]Timer ledgeClimbTimer;
    [SerializeField]Timer healthTimer;

    public PolygonCollider2D room;

    public HealthModel health;
    [System.Serializable] public class MovementDebug
    {
        [System.NonSerialized] public Vector2 buttonLiftPosition;
        [System.NonSerialized] public Vector2 buttonLiftPositionSecond;

        public int trajectoryInterval;
        public int maxTrajectory;

        public bool debugMessages;
        [System.NonSerialized] public int trajectoryTimer;
        [System.NonSerialized] public List<Vector2> trajectory = new List<Vector2>();
        public void Update(Vector2 position) 
        {
            if(trajectoryInterval > 0)
            {
                trajectoryTimer++;
                if(trajectoryTimer >= trajectoryInterval)
                {
                    trajectoryTimer = 0;
                    trajectory.Add(position);
                    if(trajectory.Count > maxTrajectory)
                        {trajectory.RemoveAt(0);}
                }
            }
        }
    }

    private void Start() 
    {
        Debug.Assert(health != null);
        ledgeClimbTimer.Initialize(() => ClimbLedge()); 
        healthTimer.Initialize(() => health.Heal(1)); 
        playerAnimator = GetComponentInChildren<Animator>();
        hookShot = GetComponent<HookShot>();
        pulka = GetComponent<Pulka>();
        body = GetComponent<Rigidbody2D>(); //? gets the rigidbody of the player

        jumpTimer = jumpLimit; //! cooldowntimer for jumping
        groundCheck.localPosition = caneGroundCheck;
        
        normGrav = body.gravityScale;
        if(movementDebug != null)
        {
            movementDebug.buttonLiftPosition = transform.position;
            movementDebug.buttonLiftPositionSecond = transform.position;
        }
        
        facingDirection = 1;
    }

    private void Update()
    {
        GroundCollisionCheck();
        if (!movementState.HasFlag(NiyoMovementState.SUBMERGED))
        {
            CheckLedgeClimb();
            Fall();
            if(movementState.HasFlag(NiyoMovementState.TOUCHING_WATER) && !movementState.HasFlag(NiyoMovementState.GROUNDED)
                && Physics2D.OverlapCircleAll(surfaceCheck.transform.position, 0.5f).Any(e => e.gameObject.layer == LayerMask.NameToLayer("Water")))
            {
                Submerge();
            }
        }
        else if(movementState.HasFlag(NiyoMovementState.SUBMERGED) && !movementState.HasFlag(NiyoMovementState.TOUCHING_SURFACE))
        {
            CheckSurfaceClose();
        }
        else if(movementState.HasFlag(NiyoMovementState.SUBMERGED | NiyoMovementState.TOUCHING_SURFACE))
        {
            CheckLedgeClimb();
            CheckSurfaceClose();
        }
        
        TryJump();
        PulkaRotate();

        if(pulka.GetState() != Pulka.PulkaState.NONE) {pulka.Use();}
    }

    private void FixedUpdate() 
    {
        movementDebug?.Update(transform.position);

        if(movementState.HasFlag(NiyoMovementState.SUBMERGED))
        {
            //healthTimer.Increment();
            if(body.gravityScale > 0)
            {
                float modifier = verticalDirection > 0?0.2f:0.05f;
                body.gravityScale = Mathf.Lerp(body.gravityScale, 0, modifier);
                body.velocity = new Vector2(body.velocity.x, Mathf.Lerp(body.velocity.y, 0, modifier));
                if(body.gravityScale < 0.01f)
                {
                    body.gravityScale = 0;
                    body.velocity = Vector2.zero;
                }
            }
        }
        
        pulka.TriggerDismount(this);
        
        TriggerSlide();

        if(movementState.HasFlag(NiyoMovementState.ACTIONBUFFER) || movementState.HasFlag(NiyoMovementState.DUCKING | NiyoMovementState.GROUNDED)) {return;}

        float speedMod = 1;

        if(movementState.HasFlag(NiyoMovementState.IS_FLUNG) && movingDirection != 0)
        {
            MoveWhenFlung(speedMod);
        }
        if(!movementState.HasFlag(NiyoMovementState.LEDGE_HANGING))
        {
            Move(speedMod);
        }
        else if(movementState.HasFlag(NiyoMovementState.LEDGE_HANGING) && (facingDirection == movingDirection 
            || movementState.HasFlag(NiyoMovementState.FORCE_LEDGE_CLIMB) 
            || movementState.HasFlag(NiyoMovementState.TOUCHING_SURFACE)))
        {
            if(movementState.HasFlag(NiyoMovementState.FORCE_LEDGE_CLIMB) || movementState.HasFlag(NiyoMovementState.TOUCHING_SURFACE))
            {
                ClimbLedge();
            }
        }
        if(movementState.HasFlag(NiyoMovementState.LEDGE_HANGING))
        {
            ledgeClimbTimer.Increment();
        }

        if(movementState.HasFlag(NiyoMovementState.JUMPING)) {jumpTimer++;}
        if(jumpBufferTimer > 0) {jumpBufferTimer--;}
    }
    public void ReturnToSafe()
    {
        health.ReturnToSafe();
    }
    void ClimbLedge()
    {
        playerAnimator.SetTrigger("ledgeClimb");
        movementState &= ~NiyoMovementState.FORCE_LEDGE_CLIMB;
        facingDirection = movingDirection == 0 ? facingDirection : movingDirection;
    }
    public void SetMovingDirection(float value)
    {
        movingDirection = Mathf.RoundToInt(value);
    }
    public void SetVerticalDirection(float value)
    {
        verticalDirection = value;
    }
    void MoveWhenFlung(float speedMod)
    {
        if (Mathf.Sign(body.velocity.x) == Mathf.Sign(movingDirection) && body.velocity.x != 0)
        {
            speedMod = 0; //If trying to move in the same direction as youre being flung, dont do anything
        }
        else
        {
            body.velocity = new Vector3(0, body.velocity.y); //If youre trying to move in the opposite direction, cancel the hookshot on twitter
        }
    }
    void Move(float speedMod)
    {
        if(!movementState.HasFlag(NiyoMovementState.SUBMERGED))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(facingDirection * 1.0f / 16 * 10, 0),
                new Vector2(facingDirection * 1.0f / 16 * 10, 0).magnitude, whatIsGround);
            if (!hit)
            {
                transform.position += new Vector3(movingDirection * speed * speedMod, 0, 0); //The normal movement
            }
        }
        else
        {
            Swim(speedMod);
        }
    }
    void Swim(float speedMod)
    {
        if (movementState.HasFlag(NiyoMovementState.SUBMERGED | NiyoMovementState.TOUCHING_SURFACE) && verticalDirection == 1) { verticalDirection = 0; }

        Vector2 swimmingDirection = (new Vector2(movingDirection, verticalDirection)).normalized;
        if(swimmingDirection == Vector2.zero)
        {
            return;
        }

        if(movementState.HasFlag(NiyoMovementState.SUBMERGED))
        {
            Quaternion target = Quaternion.FromToRotation(transform.right, swimmingDirection) * transform.rotation;

            target = new Quaternion(0,
                                    0,
                                    Mathf.Lerp(transform.rotation.z, target.z, swimmingTurnSpeed),
                                    Mathf.Lerp(transform.rotation.w, target.w, swimmingTurnSpeed));

            Vector2 movementDirection = target * Vector2.right;

            transform.rotation = target;
            if (target.eulerAngles.z - 180 > 90 || target.eulerAngles.z - 180 < -90)
            {
                bodyTransform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                bodyTransform.localScale = new Vector3(1, -1, 1);
            }

            body.velocity = movementDirection * swimmingSpeed * speedMod;
        }
        else
        {
            body.velocity = swimmingDirection * swimmingSpeed * speedMod;
        }
    }
    public void UnCollideWithWalls()
    {
        //The point of this function is to prevent the player from getting stuck when gliding along walls. It happens, for some reason
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(facingDirection * 1.0f / 16 * 10, 0),
                new Vector2(facingDirection * 1.0f / 16 * 10, 0).magnitude, whatIsGround);
        if(hit)
        {
            transform.position = hit.point - new Vector2(facingDirection * 1.0f / 16 * 10, 0);
        }
    }
    void Fall()
    {
        if(!movementState.IsGrounded() && !movementState.IsLedgeHanging()
            && Mathf.Abs(body.velocity.y) < 1 && jumpTimer > 0 && !movementState.CanMove()) //!If the down velocity or the up velocity is less than a threshold
        {
            body.gravityScale = normGrav * gravityModifier;
        }
    }
    public void Fling()
    {
        movementState |= NiyoMovementState.IS_FLUNG;
    }
    public void RemoveFlag(NiyoMovementState state)
    {
        movementState &= ~state;
    }
    public void AddFlag(NiyoMovementState state)
    {
        movementState |= state;
    }
    public bool HasFlag(NiyoMovementState state)
    {
        return movementState.HasFlag(state);
    }
    public void FaceMovingDirection()
    {
        if (!movementState.HasFlag(NiyoMovementState.LEDGE_HANGING) || !movementState.HasFlag(NiyoMovementState.SUBMERGED))
        { 
            facingDirection = movingDirection == 0 ? facingDirection : movingDirection;
            bodyTransform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }

    void CheckLedgeClimb()
    {
        if(!movementState.HasFlag(NiyoMovementState.GROUNDED)) {return;}
        bool isTouchingWall = Physics2D.Raycast(wallCheck.position, new Vector2(facingDirection, 0), wallCheckDistance, whatIsGround);
        bool isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, new Vector2(facingDirection, 0), wallCheckDistance, whatIsGround);
        bool isCloseToGround = Physics2D.Raycast(groundCheck.position, -transform.up, wallCheckDistance, whatIsGround);
        if(isTouchingWall && !isTouchingLedge && !isCloseToGround && !movementState.HasFlag(NiyoMovementState.LEDGE_DETECTED))
        {
            movementState |= NiyoMovementState.LEDGE_DETECTED;
            ledgePosBot = wallCheck.position;
        }
        if(movementState.HasFlag(NiyoMovementState.LEDGE_DETECTED) && !movementState.IsLedgeHanging())
        {
            movementState |= NiyoMovementState.LEDGE_HANGING;
            if(bodyTransform.transform.localScale.x > 0)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else if(bodyTransform.transform.localScale.x < 0)
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            body.gravityScale = 0; body.velocity = Vector2.zero;
            mainCollider.gameObject.SetActive(false);
            playerAnimator.SetTrigger("ledgeHang");
            transform.position = ledgePos1;
            movementState &= ~NiyoMovementState.ACTIONBUFFER;
            hookShot.FinishRetraction(); hookShot.state = HookShot.HookShotState.None;
        }
    }

    public void FinishLedgeClimb()
    {
        if(!movementState.IsLedgeHanging()) {return;}

        movementState &= ~NiyoMovementState.LEDGE_HANGING;
        movementState &= ~NiyoMovementState.LEDGE_DETECTED;
        transform.position = ledgePos2;
        mainCollider.gameObject.SetActive(true);
        body.gravityScale = normGrav;
        ledgeClimbTimer.Reset();
        playerAnimator.ResetTrigger("ledgeClimb");
    }
    public void CheckSurfaceClose()
    {
        if(body.gravityScale > 0){return;} //If still going down from water enter gravity, then dont check for surface

        if(!Physics2D.Raycast(surfaceCheck.position, new Vector2(0, 1), surfaceCheckDistance, whatIsGround) &&
           !Physics2D.Raycast(surfaceCheck.position, new Vector2(0, 1), surfaceCheckDistance, whatIsWater))
        {
            movementState |= NiyoMovementState.TOUCHING_SURFACE;
        }
        else
        {
            movementState &= ~NiyoMovementState.TOUCHING_SURFACE;
        }
        if(!movementState.IsTouchingSurface())
        { //This is a patch. Previously, when jumping out of water flush against a wall for a bit, swimming would be set to false and you wouldnt be far out of the water enough to exit it and reenter it
        //So when you fell back into the water, you were in a constant state of falling animation
            playerAnimator.SetBool("swimming", true);
        }
    }

    void GroundCollisionCheck()
    {
        if(movementState.HasFlag(NiyoMovementState.LEDGE_HANGING)){return;}
        if((body.velocity.y == 0 && !movementState.IsSubmerged())
            || pulka.GetState() == Pulka.PulkaState.SITTING 
            || movementState.HasFlag(NiyoMovementState.TOUCHING_SURFACE | NiyoMovementState.SUBMERGED))
        {
            if(Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround).Any(c => c.gameObject.layer == Mathf.Log(whatIsGround.value, 2)))
            {
                if (jumpTimer > 10 && !movementState.HasFlag(NiyoMovementState.GROUNDED)) 
                {
                    Ground();
                }
                movementState |= NiyoMovementState.GROUNDED;
                return;
            } 
        }
        else
        {
            if(movingDirection == 0)
            {
                playerAnimator.SetBool("walking", false);
            }
            movementState &= ~NiyoMovementState.GROUNDED;
        }
    }

    void PulkaRotate()
    {
        if(movementState.HasFlag(NiyoMovementState.DUCKING) && pulka.GetState() != Pulka.PulkaState.NONE)
        {
            float rot = body.transform.localRotation.eulerAngles.z;
            rot = rot > 50 && rot < 90 ? 50 : rot < 310 && rot > 90 ? 310 : rot;
            body.transform.localRotation = Quaternion.Euler(0,0, rot);
        }
    }

    void TriggerSlide()
    {
        if(movementState.HasFlag(NiyoMovementState.SLIDE_REQUEST) && movementState.HasFlag(NiyoMovementState.GROUNDED)) // ! and ducking maybe?
        {

            body.AddForce( new Vector2(movingDirection * speed * 20, 0), ForceMode2D.Impulse);
            movementState &= ~NiyoMovementState.SLIDE_REQUEST;
        }
    }

    private void Ground() //! when you hit ground this should be called
    {
        movementState |= NiyoMovementState.GROUNDED;
        movementState &= ~NiyoMovementState.JUMPING;
        movementState &= ~NiyoMovementState.IS_FLUNG;
        movementState &= ~NiyoMovementState.SUBMERGED;
        playerAnimator.SetBool("swimming", false);

        playerAnimator.SetTrigger("land");
        playerAnimator.SetBool("falling", false);
        amntOfJumps = 0;
        jumpTimer = jumpLimit; //* you can jump now :)
        AudioManager.PlaySFX("LandOnLand");

        if (!movementState.HasFlag(NiyoMovementState.DUCKING)) {body.velocity = Vector2.zero;}
        else {body.AddForce( new Vector2(movingDirection * speed * 20, 0), ForceMode2D.Impulse);}
        body.gravityScale = normGrav;
    }

    public void StopVelocity()
    {
        body.velocity = Vector2.zero;
    }

    public void SetCantRotate(bool value)
    {
        body.freezeRotation = value;
    }
    public void ResetRotation()
    {
        body.transform.localRotation = Quaternion.identity;
    }
    public void Duck()
    {
        movementState |= NiyoMovementState.DUCKING;
    }
    public bool IsDucking()
    {
        return movementState.HasFlag(NiyoMovementState.DUCKING);
    }

    public void TryJump()
    {
        if(jumpBufferTimer <= 0) {return;} //! you can't jump unless you've pressed jump
        if(movementState.HasFlag(NiyoMovementState.SUBMERGED) && !movementState.HasFlag(NiyoMovementState.TOUCHING_SURFACE)) {return;} //! cant jump if you're swimming, unless you're at the surface
        if(movementState.HasFlag(NiyoMovementState.ACTIONBUFFER)) {return;} //! if hookshotting, don't try to jump
        if(jumpTimer < jumpLimit) {return;} // ! you can't jump unless it's been a while since you last jumped
        if(pulka.GetState() == Pulka.PulkaState.SITTING) { return; }
        if(ledgeClimbTimer.IsCounting()) { return; }
        if(movementState.HasFlag(NiyoMovementState.LEDGE_HANGING)) //If ledge hanging and trying to jump away from it
        {
            movementState &= ~NiyoMovementState.LEDGE_HANGING;
            movementState &= ~NiyoMovementState.LEDGE_DETECTED;
            movementState |= NiyoMovementState.GROUNDED;
            mainCollider.gameObject.SetActive(true);
            body.gravityScale = normGrav;
            amntOfJumps = 0;
            bodyTransform.localScale = new Vector3(-facingDirection,1,1);
        }
        
        if (movementState.HasFlag(NiyoMovementState.GROUNDED) 
            || amntOfJumps < amntOfJumpsMax 
            || movementState.HasFlag(NiyoMovementState.TOUCHING_SURFACE)) // * if you are on the ground OR you are double jumping :) OR at the surface of the water
        {
            if(amntOfJumps == 0){caneVFX.Play();}
            playerAnimator.SetBool("swimming", false);
            playerAnimator.SetTrigger("jump");
            AudioManager.PlaySFX("Jump");
            AudioManager.PlaySFX("VoiceJump");
            jumpBufferTimer = 0;
            movementState |= NiyoMovementState.JUMPING;
            movementState &=~ NiyoMovementState.GROUNDED;
            if(amntOfJumps > 0)
            {
                doubleJump.Play();
            }
            StopVelocity();
            body.velocity = new Vector2(body.velocity.x, 0);
            body.AddForce( new Vector2(0, (amntOfJumps == 0 ? jumpForce : airJumpForce)), ForceMode2D.Impulse);
            amntOfJumps++; // * you jumped one more time :-o
            jumpTimer = 0; // ? reset timer!
            body.gravityScale = normGrav; // ? Otherwise gravity is still big when trying to double jump
        }
    }

    public void RequestJump()
    {
        if(movementState.HasFlag(NiyoMovementState.SUBMERGED) && !movementState.HasFlag(NiyoMovementState.TOUCHING_SURFACE)) {return;}
        //When you press the jump button, make a request for a jump
        jumpBufferTimer = jumpBufferMax;
        //You will only see this count down in the inspector if you are jumping midair,
        //since it resets when you jump successfully
    }
    public void StopJump()
    {
        if((movementState.HasFlag(NiyoMovementState.SUBMERGED) && !movementState.HasFlag(NiyoMovementState.TOUCHING_SURFACE)) 
            || movementState.HasFlag(NiyoMovementState.LEDGE_HANGING)){return;}

        if(jumpBufferTimer == 0 && movementDebug != null)
        {
            movementDebug.buttonLiftPosition = transform.position;
            movementDebug.buttonLiftPositionSecond = transform.position;
        }
        else if(movementDebug != null)
        {
            movementDebug.buttonLiftPositionSecond = transform.position;
        }
        body.AddForce(new Vector2(0, -cancelJumpSpeed));
        body.gravityScale = normGrav * gravityModifier;
        playerAnimator.SetBool("falling", true);
        jumpTimer = jumpLimit; // TODO: should only happen if you have a jump left ( if jumps == maxjumps )
        jumpBufferTimer = 0;

    }
    public void PlayCaneVFX()
    {
        caneVFX.Play();   
    }
    public void EnterFire()
    {
        movementState |= NiyoMovementState.ONFIRE;
        bubbleAnimator.SetBool("Fire", true);
    }
    public bool OnFire()
    {
        return movementState.HasFlag(NiyoMovementState.ONFIRE);
    }
    public bool GetGrounded()
    {
        return movementState.HasFlag(NiyoMovementState.GROUNDED);
    }
    public void SetGrounded(bool value)
    {
        if(value)
        {
            movementState |= NiyoMovementState.GROUNDED; return;
        }
        movementState &=~ NiyoMovementState.GROUNDED;
    }
    public Rigidbody2D GetBody()
    {
        return body;
    }
    public void SetGroundCheck(Vector2 value)
    {
        groundCheck.localPosition = value;
    }
    public void ResetGroundCheck()
    {
        groundCheck.localPosition = caneGroundCheck;
    }
    public void PutOutFire()
    {
        movementState &= ~NiyoMovementState.ONFIRE;
        bubbleAnimator.SetBool("Fire", false);
    }

    public bool IsInWater()
    {
        return movementState.HasFlag(NiyoMovementState.TOUCHING_WATER);
    }

    public void EnterWater()
    {
        if(!movementState.HasFlag(NiyoMovementState.GROUNDED))
        {
            movementState |= NiyoMovementState.TOUCHING_WATER;
            body.drag = 1;
            PutOutFire();
            if (body.velocity.y > -5)
            {
                body.velocity = new Vector2(body.velocity.x, -5);
            }
            
        }
    }
    public void Submerge()
    {
        movementState = movementState.Submerge();
        if (bodyTransform.localScale.x < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180)); //rotate body towards the facing direction
        }
        playerAnimator.SetBool("swimming", true);
    }
    public void ExitWater()
    {
        movementState = movementState.ExitWater();
        healthTimer.Reset();
        amntOfJumps = 0;
        body.drag = 0;
        transform.rotation = Quaternion.identity;
        if(playerAnimator.GetBool("swimming"))
        {
            bodyTransform.localScale = new Vector3(transform.localScale.y, 1, 1);
        }
        else
        {
            bodyTransform.localScale = new Vector3(facingDirection, 1, 1);
        }
        playerAnimator.SetBool("swimming", false);
        if(!movementState.HasFlag(NiyoMovementState.LEDGE_HANGING)){body.gravityScale = normGrav;}
    }
    public bool IsSubmerged()
    {
        return movementState.HasFlag(NiyoMovementState.SUBMERGED);
    }

    private void OnDrawGizmos()
    {
        /*
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(movementDebug.buttonLiftPositionSecond, 0.1f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(movementDebug.buttonLiftPosition, 0.1f);
        
        Gizmos.color = new Color32(200, 20, 200, 255);
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(facingDirection, 0));

        for(int i = 0; i < movementDebug.trajectory.Count; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(movementDebug.trajectory[i], 0.1f);
            if(i != 0) {Gizmos.DrawLine(movementDebug.trajectory[i-1], movementDebug.trajectory[i]);}
        }
        */

        Gizmos.color = movementState.HasFlag(NiyoMovementState.LEDGE_DETECTED) ? Color.green : Color.red;
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + new Vector3(wallCheckDistance, 0,0));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(wallCheckDistance, 0,0));

        Gizmos.color = movementState.HasFlag(NiyoMovementState.TOUCHING_SURFACE) ? Color.green : Color.red;
        Gizmos.DrawLine(surfaceCheck.position, surfaceCheck.position + new Vector3(0, surfaceCheckDistance,0));
    }
}
    