using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class HookShot : MonoBehaviour
{
    public float hookStrength; // amount it joinks you
    public float hookSpeed; // speed of the hook projectile
    public float hookRange; // range of the hook
    [System.NonSerialized] public float hookAngle;
    public HookProjectile hook;
    public Vector2 hookDir;
    public float rayLength = 2.0f;
    [SerializeField] public LayerMask whatIsGround;
    Vector2 hookForce;
    Rigidbody2D body;
    Pulka pulka;
    public Animator playerAnimator;

    Movement movement;
    [SerializeField] Transform seaweed;
    public Transform hookOrigin;
    public bool retract;
    public bool shooting;
    [System.NonSerialized] public bool hit;
    public Vector2 hitPoint; //DEBUG
    public Vector2[] colliderCheckPoints = new Vector2[3];
    public Color[] colliderCheckPoint_colors = new Color[3];
    public Color hitPoint_color;

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
        movement = gameObject.GetComponent<Movement>();
        hook.hookScript = this;
        pulka = GetComponent<Pulka>();
        retract = false;
    }
    private void Update() 
    {
        AdjustSeaweed();
    }
    void FixedUpdate()
    {
        float hookDistance = (transform.position - hook.transform.position).magnitude;
        if(hit && !retract && shooting)
        {
            PullIn();
        }
        if(!retract && hookDistance >= hookRange)
        {
            Retract();
            retract = true;
            //swoop back
        }
        else if(retract)
        {
            Retract();
        }
    }

    public void PullPlayer(Vector2 posIn) //This is when the player gets yeeted 
    {
        playerAnimator.SetBool("hookpulling", true);
        body.velocity = Vector2.zero;
        if(!HasPulledInFully())
        {
            hookForce = (posIn - body.position).normalized * hookStrength; // * the vector from player to hook hit position normalized and scaled
            body.AddForce(hookForce, ForceMode2D.Impulse); // * push player towards the hook hit position
            movement.Fling();
        }
        else
        {
            movement.UnCollideWithWalls();
        }
        AudioManager.PlaySFX("HookHit");
        AudioManager.PlaySFX("HookReel");

        movement.RemoveFlag(Movement.NiyoMovementState.ACTIONBUFFER);
        body.gravityScale = movement.normGrav;
        movement.amntOfJumps = 0;
    }
    public void StopPull()
    {
        if(!hook.IsVisible()){return;}
        shooting = false;
        movement.RemoveFlag(Movement.NiyoMovementState.FORCE_LEDGE_CLIMB);
        if(hit)
        {
            PullPlayer(hook.body.position);
            retract = true;
        }
    }
    public void PullIn()
    {
        if (HasPulledInFully())
        {
            shooting = false;
            return;
        }
        Vector2 Dir = (hitPoint - (Vector2)transform.position).normalized;
        body.velocity = Dir * hookSpeed;
    }
    public bool HasPulledInFully()
    {
        return (hitPoint - (Vector2)transform.position).magnitude < 0.5f; //0.5f is the current margin
    }

    public void Aim(Vector2 mousePosition) //! input stuff
    {
        mousePosition -= (Vector2)hookOrigin.localPosition;
        hookDir = mousePosition.normalized;

        if (mousePosition.x != 0)
        {
            hookAngle = mousePosition.y < 0 ? -1 * Vector2.Angle(Vector2.right, mousePosition)
                : Vector2.Angle(Vector2.right, mousePosition);
        }
        else { hookAngle = 90 * Mathf.Sign(mousePosition.y); }
        hookAngle -= 90;
        if(hookDir == Vector2.zero)
        {
            hookDir = new Vector2(0,1);
        }
    }

    public bool Shoot()
    {
        if(hook.IsVisible()) {return false;}
        if(movement.IsDucking()){return false;}
        if(pulka.GetState() != Pulka.PulkaState.NONE) {return false;}
        RaycastHit2D closeRangeHit = Physics2D.Raycast(transform.position, hookDir, rayLength, whatIsGround);
        if(closeRangeHit.collider != null) { return false; }
        hook.SetVisible(true);
        hit = false;
        shooting = true;
        hook.ResetHookshot();
        hook.transform.localPosition = hookOrigin.localPosition;
        hook.transform.rotation = Quaternion.Euler(0, 0, hookAngle); // * rotate the hook in the direction of the stick and...
        hook.body.velocity = hookDir.normalized * hookSpeed; //* give it velocity in that direction
        movement.facingDirection = Mathf.Sign(Vector2.Dot(Vector2.right, hookDir.normalized));
        AdjustSeaweed();
       
        body.gravityScale = 0;
        movement.AddFlag(Movement.NiyoMovementState.ACTIONBUFFER);
        body.velocity = Vector2.zero;
        playerAnimator.SetTrigger("throwhook");
        AudioManager.PlaySFX("HookThrow");
        return true;
    }
    public void Retract()
    {
        if (!hook.IsVisible()) { return; }
        if (movement.HasFlag(Movement.NiyoMovementState.ACTIONBUFFER)) //When it starts retracting, give player their movement back
        {
            movement.RemoveFlag(Movement.NiyoMovementState.ACTIONBUFFER);
            body.gravityScale = movement.normGrav;
        }

        Vector2 Dir = (movement.gameObject.transform.position - hook.transform.position).normalized;
        hook.body.velocity = Dir * 30.0f * 2.0f; //* give it velocity in that direction (hookspeed * 2)
        AdjustSeaweed();

        if((hook.transform.position - movement.gameObject.transform.position).magnitude < 1.0f)
        {
            FinishRetraction();
        }
    }
    public void FinishRetraction()
    {
        hook.SetVisible(false);
        hook.transform.localPosition = Vector3.zero;
        hook.body.velocity = Vector3.zero;
        hook.transform.rotation = Quaternion.identity;
        retract = false;
    }

    public void AdjustSeaweed()
    {
        seaweed.transform.position = new Vector2((hook.transform.position.x + transform.position.x + hookOrigin.localPosition.x)/2, 
            (hook.transform.position.y + transform.position.y + hookOrigin.localPosition.y)/2);
        
        Vector2 vectorToTarget = seaweed.transform.position - transform.position - hookOrigin.localPosition;
        float seaweedAngle = 90 * Mathf.Deg2Rad;
        
        if(vectorToTarget.x != 0) 
            {seaweedAngle = Mathf.Atan(vectorToTarget.y / vectorToTarget.x);}

        seaweed.rotation = Quaternion.Euler(0, 0, seaweedAngle * Mathf.Rad2Deg);
        seaweed.GetComponent<SpriteRenderer>().size = new Vector2(vectorToTarget.magnitude * 2 * (1 / hook.transform.localScale.x), seaweed.localScale.y);
    }
    public void ClimbLedge()
    {
        movement.AddFlag(Movement.NiyoMovementState.FORCE_LEDGE_CLIMB);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + hookDir.normalized * 3);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(hookDir + (Vector2)transform.position, 0.2f);
        
        Gizmos.color = new Color32(255,125,0,255);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + hookForce);

        Gizmos.color = hitPoint_color;
        Gizmos.DrawSphere(hitPoint, 0.2f);

        for(int i = 0; i < 3; i++)
        {
            Gizmos.color = colliderCheckPoint_colors[i];
            Gizmos.DrawSphere(colliderCheckPoints[i], 0.4f);
        }
    }
}
