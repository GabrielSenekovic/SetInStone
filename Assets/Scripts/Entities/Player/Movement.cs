using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.VFX;
public class Movement : MonoBehaviour
{
    Animator playerAnimator;
    public bool grounded = true;
    public bool ducking;
    public float cancelJumpSpeed = 5.0f;

    Rigidbody2D body;
    [System.NonSerialized] public HookShot hookShot;
    [System.NonSerialized] public Pulka pulka;

    [System.NonSerialized] public const float groundedRadius = .05f;
    [System.NonSerialized] public float gravityVelocity;
    [System.NonSerialized] public float movingDirection = 0; //The direction you're currently moving in. If not moving, the direction is 0
    [System.NonSerialized] public float facingDirection = 0; //The direction you last looked in. It's set to something even if not moving
    
    [System.NonSerialized] public int amntOfJumps = 0;
    [System.NonSerialized] public int jumpTimer = 0;
    [System.NonSerialized] public int jumpBufferTimer = 0;
    
    [System.NonSerialized] public bool jumping = false;
    [System.NonSerialized] public bool dismountRequest = false;
    [System.NonSerialized] public bool slideRequest = false;

    [System.NonSerialized] public float normGrav;
    [System.NonSerialized] public bool actionBuffer;

    [SerializeField] public Vector2 caneGroundCheck;
    [SerializeField] public Vector2 pulkaGroundCheck;
    [SerializeField] public float gravityModifier; //Used to increase the gravity when falling
    [SerializeField] public float jumpForce;
    [SerializeField] public float airJumpForce;
    [SerializeField] public float speed;
    [SerializeField] public int jumpBufferMax; //This timer should count down to 0
    [SerializeField] public int jumpLimit;
    [SerializeField] public int amntOfJumpsMax;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public Transform groundCheck;

    public MovementDebug movementDebug;

    public Transform bodyTransform;
    public VisualEffectEntry doubleJumpVFX;

    public VisualEffect doubleJump_prefab;

    public Vector2 currentVelocity;
    public bool isFlung; //Used for hookshotting. If moving in the opposite direction of velocity you break it but if you move in the same direction nothing happens
    public bool hangingFromLedge;
    public bool ledgeDetected;
    public Transform ledgeCheck;
    public Transform wallCheck;
    public float wallCheckDistance;
    public Vector2 ledgePosBot;
    public Vector2 ledgePos1;
    public Vector2 ledgePos2;
    public float ledgeClimbXOffset1;
    public float ledgeClimbYOffset1;
    public float ledgeClimbXOffset2;
    public float ledgeClimbYOffset2;

    public Collider2D mainCollider;

    int ledgeClimbTimer;
    int ledgeClimbTimer_max = 20;

    public bool forceLedgeClimb = false;

    public bool touchingWater = false;

    int healthTimer;
    int healthTimer_max = 50;

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
        playerAnimator = GetComponentInChildren<Animator>();
        hookShot = GetComponent<HookShot>();
        pulka = GetComponent<Pulka>();
        body = GetComponent<Rigidbody2D>(); //? gets the rigidbody of the player

        jumpTimer = jumpLimit; //! cooldowntimer for jumping
        groundCheck.localPosition = caneGroundCheck;
        
        normGrav = body.gravityScale;
        movementDebug.buttonLiftPosition = transform.position;
        movementDebug.buttonLiftPositionSecond = transform.position;
        
        facingDirection = 1;
        actionBuffer = false;
        ducking = false;
        hangingFromLedge = false;

        doubleJumpVFX.effect = Instantiate(doubleJump_prefab, transform.position, Quaternion.identity, transform);
        Game.Instance.visualEffects.Add(doubleJumpVFX, false);
    }

    private void Update()
    {
        currentVelocity = body.velocity;
        GroundCollisionCheck();

        CheckLedgeClimb();

        
        if(!grounded && !hangingFromLedge && Mathf.Abs(body.velocity.y) < 1 && jumpTimer > 0 && actionBuffer == false) //!If the down velocity or the up velocity is less than a threshold
        {
            body.gravityScale = normGrav * gravityModifier;
        }
        
        TryJump();
        PulkaRotate();

        if(pulka.state != Pulka.PulkaState.NONE) {pulka.Use();}
    }

    private void FixedUpdate() 
    {
        movementDebug.Update(transform.position);

        if(touchingWater)
        {
            healthTimer++;
            if(healthTimer>=healthTimer_max)
            {
                healthTimer = 0;
                health.Heal(1);
            }
        }
        
        TriggerDismount();
        
        TriggerSlide();

        if(actionBuffer || (ducking && grounded)) {return;}

        float speedMod = 1;

        if(isFlung && movingDirection != 0)
        {
            if(Mathf.Sign(body.velocity.x) == Mathf.Sign(movingDirection) && body.velocity.x != 0)
            {
                speedMod = 0; //If trying to move in the same direction as youre being flung, dont do anything
            }
            else
            {
                body.velocity = new Vector3(0, body.velocity.y); //If youre trying to move in the opposite direction, cancel the hookshot on twitter
            }
        }
        if(!hangingFromLedge)
        {
            transform.position +=  new Vector3(movingDirection * speed * speedMod,0,0);
        }
        else if(hangingFromLedge && (facingDirection == movingDirection || forceLedgeClimb))
        {
            if(ledgeClimbTimer >= ledgeClimbTimer_max || forceLedgeClimb)
            {
                ledgeClimbTimer = 0;
                playerAnimator.SetTrigger("ledgeClimb");
                forceLedgeClimb = false;
            }
        }
        if(hangingFromLedge)
        {
            ledgeClimbTimer++;
        }

        if(jumping) {jumpTimer++;}
        if(jumpBufferTimer > 0) {jumpBufferTimer--;}
    }

    void CheckLedgeClimb()
    {
        if(grounded){return;}
        bool isTouchingWall = Physics2D.Raycast(wallCheck.position, new Vector2(facingDirection, 0), wallCheckDistance, whatIsGround);
        bool isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, new Vector2(facingDirection, 0), wallCheckDistance, whatIsGround);
        bool isCloseToGround = Physics2D.Raycast(groundCheck.position, -transform.up, wallCheckDistance, whatIsGround);
        if(isTouchingWall && !isTouchingLedge && !isCloseToGround && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
        if(ledgeDetected && !hangingFromLedge)
        {
            hangingFromLedge = true;
            if(transform.localScale.x > 0)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else if(transform.localScale.x < 0)
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            body.gravityScale = 0; body.velocity = Vector2.zero;
            mainCollider.gameObject.SetActive(false);
            playerAnimator.SetTrigger("ledgeHang");
            transform.position = ledgePos1;
            actionBuffer = false;
            hookShot.FinishRetraction(); hookShot.shooting = false;
        }
    }

    public void FinishLedgeClimb()
    {
        if(!hangingFromLedge){return;}
        hangingFromLedge = false;
        transform.position = ledgePos2;
        mainCollider.gameObject.SetActive(true);
        ledgeDetected = false;
        body.gravityScale = normGrav;
    }

    void GroundCollisionCheck()
    {
        if(body.velocity.y == 0 || pulka.state == Pulka.PulkaState.SITTING) //!if falling ( if you are not moving in y and if you just jumped and you arent on the ground)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround); //* make a collider on the feet and see what it hits

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.layer == Mathf.Log(whatIsGround.value,2)) //* check the things it hits,,, if its not the player itself, you collided maboi
                {
                    if (jumpTimer > 10 && !grounded) {Ground();} // if !grounded && jumptimer > 10 
                    grounded = true;
                    return;
                }
                else if(i == colliders.Length - 1) {playerAnimator.SetBool("falling", true); grounded = false;}
            }  
        }
        else
        {
            if(movingDirection == 0)
            {
                playerAnimator.SetBool("walking", false);
            }
            grounded = false;
        }
    }

    void PulkaRotate()
    {
        if(ducking && pulka.state != Pulka.PulkaState.NONE)
        {
            float rot = body.transform.localRotation.eulerAngles.z;
            rot = rot > 50 && rot < 90 ? 50 : rot < 310 && rot > 90 ? 310 : rot;
            body.transform.localRotation = Quaternion.Euler(0,0, rot);
        }
    }

    void TriggerSlide()
    {
        if(slideRequest && grounded) // ! and ducking maybe?
        {
            if(movementDebug.debugMessages){Debug.Log("Sliding");}

            body.AddForce( new Vector2(movingDirection * speed * 20, 0), ForceMode2D.Impulse);
           // Debug.Log("Force from slide: " + movingDirection * speed * 20);
            slideRequest = false;
        
        }
    }

    //TODO: This is probably redundant. Check later if it can be put into Pulka script.
    void TriggerDismount()
    {
        if(dismountRequest && grounded) //* triggers when on the pulka on ground, so that you get pushed up
        {
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            //Debug.Log("Force from discmount");
            grounded = false;
            dismountRequest = false;
            pulka.state = Pulka.PulkaState.NONE;
            pulka.Dismount();
            groundCheck.localPosition = caneGroundCheck;
            playerAnimator.SetBool("sitting", false);
        }
    }

    private void Ground() //! when you hit ground this should be called
    {
        if(movementDebug.debugMessages){Debug.Log("Ground");}

        grounded = true; //! ...
       // Debug.Log(grounded);
        playerAnimator.SetTrigger("land");
        playerAnimator.SetBool("falling", false);
        amntOfJumps = 0; //? reset jumps when you hit the floor
        jumping = false; // ? stop jump -||-
        jumpTimer = jumpLimit; //* you can jump now :)
        AudioManager.PlaySFX("LandOnLand");

        if (!ducking) {body.velocity = Vector2.zero;}
        else {body.AddForce( new Vector2(movingDirection * speed * 20, 0), ForceMode2D.Impulse);}
        body.gravityScale = normGrav;
        isFlung = false;
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

    private void TryJump()
    {
        if(actionBuffer) {return;} //! if hookshotting, don't try to jump
        if(jumpBufferTimer <= 0) {return;} //! you can't jump unless you've pressed jump
        if(jumpTimer < jumpLimit) {return;} // ! you can't jump unless it's been a while since you last jumped
        if(pulka.state == Pulka.PulkaState.SITTING) { return; }
        
        if(hangingFromLedge) //If ledge hanging and trying to jump away from it
        {
            hangingFromLedge = false;
            mainCollider.gameObject.SetActive(true);
            ledgeDetected = false;
            body.gravityScale = normGrav;
            grounded = true;
            amntOfJumps = 0;
            ledgeClimbTimer = 0;
            bodyTransform.localScale = new Vector3(-facingDirection,1,1);
        }
        
        if (grounded || amntOfJumps < amntOfJumpsMax) // * if you are on the ground OR you are double jumping :)
        {
            if(movementDebug.debugMessages){Debug.Log("Jump");}
            playerAnimator.SetTrigger("jump");
            AudioManager.PlaySFX("Jump");
            AudioManager.PlaySFX("VoiceJump");
            jumpBufferTimer = 0;
            jumping = true; grounded = false; //* if you jump, you are jumping, and you are not on the ground *taps head*
            if(amntOfJumps > 0)
            {
                Game.Instance.visualEffects.ChangePosition(doubleJumpVFX, transform.position);
                doubleJumpVFX.effect.Play();
            }
            StopVelocity();
            body.velocity = new Vector2(body.velocity.x, 0);
            body.AddForce( new Vector2(0, (amntOfJumps == 0 ? jumpForce : airJumpForce)), ForceMode2D.Impulse);
           // Debug.Log("Jump added force");
            amntOfJumps++; // * you jumped one more time :-o
            jumpTimer = 0; // ? reset timer!
            body.gravityScale = normGrav; // ? Otherwise gravity is still big when trying to double jump
        }
    }

    public void RequestJump()
    {
        //When you press the jump button, make a request for a jump
        if(movementDebug.debugMessages){Debug.Log("Jump pressed");}
        jumpBufferTimer = jumpBufferMax;
        //You will only see this count down in the inspector if you are jumping midair,
        //since it resets when you jump successfully
    }
    public void StopJump()
    {
        if(movementDebug.debugMessages){Debug.Log("Cancel jump");}

        if(jumpBufferTimer == 0)
        {
            movementDebug.buttonLiftPosition = transform.position;
            movementDebug.buttonLiftPositionSecond = transform.position;
        }
        else
        {
            movementDebug.buttonLiftPositionSecond = transform.position;
        }
        body.AddForce(new Vector2(0, -cancelJumpSpeed));
        body.gravityScale = normGrav * gravityModifier;
        playerAnimator.SetBool("falling", true);
        jumpTimer = jumpLimit; // TODO: should only happen if you have a jump left ( if jumps == maxjumps )
        jumpBufferTimer = 0;

    }
    public void ExitWater()
    {
        touchingWater = false;
        healthTimer = 0;
    }

    private void OnDrawGizmos()
    {
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
    }
}
    