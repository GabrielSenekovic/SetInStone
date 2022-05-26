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
    Vector2 hookForce;
    Rigidbody2D body;
    Pulka pulka;
    public Animator playerAnimator;

    public Movement movement;
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
        //hook = Instantiate(hookPrefab, transform.position, Quaternion.identity).GetComponent<HookProjectile>();
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
        hookForce = (posIn - body.position).normalized * hookStrength; // * the vector from player to hook hit position normalized and scaled
        body.AddForce(hookForce, ForceMode2D.Impulse); // * push player towards the hook hit position
        //Cant do this, it has to be continuous
        GetComponent<Movement>().isFlung = true;
        AudioManager.PlaySFX("HookHit");
        AudioManager.PlaySFX("HookReel");

        movement.actionBuffer = false;
        body.gravityScale = movement.normGrav;
        movement.amntOfJumps = 0;
    }
    public void StopPull()
    {
        if(!hook.gameObject.activeSelf){return;}
        shooting = false;
        movement.forceLedgeClimb = false;
        if(hit)
        {
            PullPlayer(hook.body.position);
            retract = true;
        }
    }
    public void PullIn()
    {
        if((hitPoint - (Vector2)transform.position).magnitude < 0.2f)
        {
            body.velocity = Vector2.zero;
            shooting = false;
            return;
        }
        Vector2 Dir = (hitPoint - (Vector2)transform.position).normalized;
        body.velocity = Dir * hookSpeed;

        /*if((transform.position - playerMov.gameObject.transform.position).magnitude < 1)
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            retract = false;
            playerMov.actionBuffer = false;
            playerRb.gravityScale = playerMov.normGrav;
        }*/
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
        if(hook.gameObject.activeSelf) {return false;}
        if(GetComponent<Movement>().ducking){return false;}
        if(pulka.state != Pulka.PulkaState.NONE) {return false;}
        hook.gameObject.SetActive(true);
        hit = false;
        shooting = true;
        hook.transform.position = (Vector3)hook.body.position + hookOrigin.localPosition;
        hook.transform.rotation = Quaternion.Euler(0, 0, hookAngle); // * rotate the hook in the direction of the stick and...
        hook.body.velocity = hookDir.normalized * hookSpeed; //* give it velocity in that direction
        AdjustSeaweed();
       
        body.gravityScale = 0;
        GetComponent<Movement>().actionBuffer = true;
        body.velocity = Vector2.zero;
        playerAnimator.SetTrigger("throwhook");
        AudioManager.PlaySFX("HookThrow");
        return true;
    }
    public void Retract()
    {
        if(movement.actionBuffer) //When it starts retracting, give player their movement back
        {
            movement.actionBuffer = false;
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
        hook.gameObject.SetActive(false);
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
        //seaweed.localScale = new Vector3(vectorToTarget.magnitude * 2 * (1 / hook.transform.localScale.x), seaweed.localScale.y, 1);
        //seaweed.GetComponent<MeshRenderer>().sharedMaterials[0].mainTextureScale = new Vector2(seaweed.localScale.x/2, 1);
    }
    public void ClimbLedge()
    {
        movement.forceLedgeClimb = true;
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
