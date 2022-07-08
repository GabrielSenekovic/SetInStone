using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.VFX;
using System.Linq;
public class Movement : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator bubbleAnimator;
    bool grounded = true;
    public bool ducking;
    public float cancelJumpSpeed = 5.0f;

    Rigidbody2D body;
    [System.NonSerialized] public HookShot hookShot;
    [System.NonSerialized] public Pulka pulka;

    [System.NonSerialized] public const float groundedRadius = .05f;
    [System.NonSerialized] public float gravityVelocity;
    public float movingDirection = 0; //The direction you're currently moving in. If not moving, the direction is 0
    public float verticalDirection = 0; //This is only used when swimming! If not swimming, vertical directon is 0
    [System.NonSerialized] public float facingDirection = 0; //The direction you last looked in. It's set to something even if not moving
    
    [System.NonSerialized] public int amntOfJumps = 0;
    [System.NonSerialized] public int jumpTimer = 0;
    [System.NonSerialized] public int jumpBufferTimer = 0;
    
    [System.NonSerialized] public bool jumping = false;
    [System.NonSerialized] public bool dismountRequest = false;
    [System.NonSerialized] public bool slideRequest = false;

    [System.NonSerialized] public float normGrav;
    [System.NonSerialized] public bool actionBuffer;

    Vector2 caneGroundCheck = new Vector2(0, -0.2f);
    [SerializeField] public float gravityModifier; //Used to increase the gravity when falling
    [SerializeField] public float jumpForce;
    [SerializeField] public float airJumpForce;
    [SerializeField] public float speed;
    [SerializeField] public float swimmingSpeed;
    [SerializeField] public int jumpBufferMax; //This timer should count down to 0
    [SerializeField] public int jumpLimit;
    [SerializeField] public int amntOfJumpsMax;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public LayerMask whatIsWater;

    public MovementDebug movementDebug;

    public Transform bodyTransform;

    public ParticleSystem doubleJump;
    public ParticleSystem caneVFX;

    public Vector2 currentVelocity;

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

    public int ledgeClimbTimer;
    int ledgeClimbTimer_max = 2;

    public bool forceLedgeClimb = false;
    public bool hangingFromLedge;
    public bool ledgeDetected;
    public bool submerged = false;
    public bool touchingWater = false;
    public bool touchingSurface = false;
    public bool isFlung; //Used for hookshotting. If moving in the opposite direction of velocity you break it but if you move in the same direction nothing happens

    int healthTimer;
    int healthTimer_max = 50;

    [SerializeField] bool onFire = false;

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
    }

    private void Update()
    {
        currentVelocity = body.velocity;

        if(!submerged) //Underwater you cant walk on the ground nor ledgeclimb
        {
            GroundCollisionCheck();
            CheckLedgeClimb();
            Fall();
            if(touchingWater && Physics2D.OverlapCircleAll(surfaceCheck.transform.position, 0.5f).Any(e => e.gameObject.layer == LayerMask.NameToLayer("Water")))
            {
                submerged = true;
                playerAnimator.SetBool("swimming", true);
            }
        }
        else if(submerged && !touchingSurface)
        {
            CheckSurfaceClose();
        }
        else if(submerged && touchingSurface)
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
        movementDebug.Update(transform.position);

        if(submerged)
        {
            healthTimer++;
            if(healthTimer>=healthTimer_max)
            {
                healthTimer = 0;
                health.Heal(1);
            }
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
            Move(speedMod);
        }
        else if(hangingFromLedge && (facingDirection == movingDirection || forceLedgeClimb || touchingSurface))
        {
            if(ledgeClimbTimer >= ledgeClimbTimer_max || forceLedgeClimb || touchingSurface)
            {
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
    void Move(float speedMod)
    {
        if(!submerged)
        {
            transform.position +=  new Vector3(movingDirection * speed * speedMod,0,0); //The normal movement
        }
        else
        {
            if(touchingSurface && submerged && verticalDirection == 1){verticalDirection = 0;}
            Vector2 swimmingDirection = (new Vector2(movingDirection, verticalDirection)).normalized;
            transform.position +=  new Vector3(swimmingDirection.x * swimmingSpeed * speedMod,swimmingDirection.y * swimmingSpeed * speedMod,0); //The normal movement
        }
    }
    void Fall()
    {
        if(!grounded && !hangingFromLedge && Mathf.Abs(body.velocity.y) < 1 && jumpTimer > 0 && actionBuffer == false) //!If the down velocity or the up velocity is less than a threshold
        {
            body.gravityScale = normGrav * gravityModifier;
        }
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
        ledgeClimbTimer = 0;
        playerAnimator.ResetTrigger("ledgeClimb");
    }
    public void CheckSurfaceClose()
    {
        if(body.gravityScale > 0){return;} //If still going down from water enter gravity, then dont check for surface
        touchingSurface = !Physics2D.Raycast(surfaceCheck.position, new Vector2(0, 1), surfaceCheckDistance, whatIsGround) &&
        !Physics2D.Raycast(surfaceCheck.position, new Vector2(0, 1), surfaceCheckDistance, whatIsWater);
        if(!touchingSurface)
        { //This is a patch. Previously, when jumping out of water flush against a wall for a bit, swimming would be set to false and you wouldnt be far out of the water enough to exit it and reenter it
        //So when you fell back into the water, you were in a constant state of falling animation
            playerAnimator.SetBool("swimming", true);
        }
    }

    void GroundCollisionCheck()
    {
        if(hangingFromLedge){return;}
        if(body.velocity.y == 0 || pulka.GetState() == Pulka.PulkaState.SITTING) //!if falling ( if you are not moving in y and if you just jumped and you arent on the ground)
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
        if(ducking && pulka.GetState() != Pulka.PulkaState.NONE)
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
            slideRequest = false;
        
        }
    }

    private void Ground() //! when you hit ground this should be called
    {
        if(movementDebug.debugMessages){Debug.Log("Ground");}

        grounded = true; //! ...
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

    public void TryJump()
    {
        if(jumpBufferTimer <= 0) {return;} //! you can't jump unless you've pressed jump
        if(submerged && !touchingSurface) {return;} //! cant jump if you're swimming, unless you're at the surface
        if(actionBuffer) {return;} //! if hookshotting, don't try to jump
        if(jumpTimer < jumpLimit) {return;} // ! you can't jump unless it's been a while since you last jumped
        if(pulka.GetState() == Pulka.PulkaState.SITTING) { return; }
        
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
        
        if (grounded || amntOfJumps < amntOfJumpsMax || touchingSurface) // * if you are on the ground OR you are double jumping :) OR at the surface of the water
        {
            if(movementDebug.debugMessages){Debug.Log("Jump");}
            if(amntOfJumps == 0){caneVFX.Play();}
            playerAnimator.SetBool("swimming", false);
            playerAnimator.SetTrigger("jump");
            AudioManager.PlaySFX("Jump");
            AudioManager.PlaySFX("VoiceJump");
            jumpBufferTimer = 0;
            jumping = true; grounded = false; //* if you jump, you are jumping, and you are not on the ground *taps head*
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
        if(submerged && !touchingSurface){return;}
        //When you press the jump button, make a request for a jump
        if(movementDebug.debugMessages){Debug.Log("Jump pressed");}
        jumpBufferTimer = jumpBufferMax;
        //You will only see this count down in the inspector if you are jumping midair,
        //since it resets when you jump successfully
    }
    public void StopJump()
    {
        if((submerged && !touchingSurface) || hangingFromLedge){return;}
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
    public void PlayCaneVFX()
    {
        caneVFX.Play();   
    }
    public void EnterFire()
    {
        onFire = true;
        bubbleAnimator.SetBool("Fire", true);
    }
    public bool OnFire()
    {
        return onFire;
    }
    public bool GetGrounded()
    {
        return grounded;
    }
    public void SetGrounded(bool value)
    {
        grounded = value;
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

    public void EnterWater()
    {
        touchingWater = true;
        grounded = false;
        onFire = false;
        bubbleAnimator.SetBool("Fire", false);
        if(body.velocity.y > -5)
        {
            body.velocity = new Vector2(body.velocity.x,-5);
        }
    }
    public void ExitWater()
    {
        submerged = false;
        touchingWater = false;
        touchingSurface = false;
        healthTimer = 0;
        amntOfJumps = 0;
        playerAnimator.SetBool("swimming", false);
        if(!hangingFromLedge){body.gravityScale = normGrav;}
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

        Gizmos.color = ledgeDetected ? Color.green : Color.red;
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + new Vector3(wallCheckDistance, 0,0));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(wallCheckDistance, 0,0));

        Gizmos.color = touchingSurface ? Color.green : Color.red;
        Gizmos.DrawLine(surfaceCheck.position, surfaceCheck.position + new Vector3(0, surfaceCheckDistance,0));
    }
}
    