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
    public GameObject hookPrefab;
    [System.NonSerialized] public HookProjectile hook;
    public Vector2 hookDir;
    Vector2 hookForce;
    Rigidbody2D body;
    Pulka pulka;
    public Animator playerAnimator;

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
        hook = Instantiate(hookPrefab, transform.position, Quaternion.identity).GetComponent<HookProjectile>();
        hook.hookScript = this;
        hook.shootDistanceMax = hookRange;
        hook.gameObject.SetActive(false);
        pulka = GetComponent<Pulka>();
    }

    public void PullPlayer(Vector2 posIn)
    {
        playerAnimator.SetBool("hookpulling", true);
        hookForce = (posIn - body.position).normalized * hookStrength; // * the vector from player to hook hit position normalized and scaled
        body.AddForce(hookForce, ForceMode2D.Impulse); // * push player towards the hook hit position
        //Cant do this, it has to be continuous
        GetComponent<Movement>().isFlung = true;
        AudioManager.PlaySFX("HookHit");
        AudioManager.PlaySFX("HookReel");
        Debug.Log("Added force in direction: " + (posIn - body.position).normalized);

        hook.playerMov.actionBuffer = false;
        hook.playerRb.gravityScale = hook.playerMov.normGrav;
        hook.playerMov.amntOfJumps = 0;
    }
    public void StopPull()
    {
        if(hook.hit)
        {
            PullPlayer(hook.body.position);
        }
        hook.retract = true;
    }

    public void Aim(Vector2 mousePosition) //! input stuff
    {
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
          Debug.Log("Shooting on cooldown");
        if(GetComponent<Movement>().ducking){return false;}
        if(pulka.state != Pulka.PulkaState.NONE) {return false;}
        Debug.Log("Shooting");
        hook.gameObject.SetActive(true);
        hook.GetComponent<HookProjectile>().Shoot(hookDir, hookSpeed, hookAngle, body.position);
       
        body.gravityScale = 0;
        GetComponent<Movement>().actionBuffer = true;
        body.velocity = Vector2.zero;
        playerAnimator.SetTrigger("throwhook");
        AudioManager.PlaySFX("HookThrow");

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + hookDir.normalized * 3);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(hookDir + (Vector2)transform.position, 0.2f);
        
        Gizmos.color = new Color32(255,125,0,255);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + hookForce);
    }
}
