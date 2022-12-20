using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System.Linq;

public class HookProjectile : MonoBehaviour
{
    [System.NonSerialized]public Rigidbody2D body;
    public HookShot hookScript;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] ParticleSystem ivyParticles;
    public Rigidbody2D playerRb;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] GameObject seaWeed;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerRb = hookScript.gameObject.GetComponent<Rigidbody2D>();
        SetVisible(false);
    }

    public bool IsVisible()
    {
        return renderer.color != Color.clear;
    }
    public void SetVisible(bool value)
    {
        renderer.color = value?Color.white:Color.clear;
        seaWeed.SetActive(value);
    }
    public void ResetHookshot()
    {
        body.velocity = Vector3.zero;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(hookScript.state != HookShot.HookShotState.Shooting) { return; }
        if(!hookScript.hit 
            && !(other.CompareTag("Player") || other.CompareTag("PassThrough") ||  other.CompareTag("Water")) 
            && other.gameObject.layer != LayerMask.NameToLayer("Pickup")
            && !other.GetComponent<PlatformEffector2D>())
        {
            body.gravityScale = 0; body.velocity = Vector2.zero; //Stop the hook
            hookScript.state = HookShot.HookShotState.Retracting;

            if (Physics2D.OverlapCircleAll(transform.position, 0.25f).Any(h => h.GetComponent<Ivy>()))
            {
                ivyParticles.Play();
                hookScript.StopPull();
                return;
            }
            if(other.TryGetComponent(out IAttackable attackable))
            {
                attackable.OnBeAttacked(1, body.velocity);
                hookScript.StopPull();
                return;
            }
            hookScript.hit = true;

            playerRb.gravityScale = 0;

            hookScript.hitPoint = other.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);
            hookScript.hitPoint = new Vector2(Mathf.RoundToInt(hookScript.hitPoint.x), Mathf.CeilToInt(hookScript.hitPoint.y));

            Vector2 Dir = (hookScript.hitPoint - (Vector2)hookScript.transform.position).normalized;
            playerRb.velocity = Dir * hookScript.hookSpeed;

            hitParticles.Play();
            hookScript.OnHit();

            CheckHitPoints();
        }
        else if(other.CompareTag("PassThrough"))
        {
            ivyParticles.Play();
        }
    }
    void HitTarget()
    {

    }
    void CheckHitPoints() //Debug
    {
        int dir = (int)Mathf.Sign(hookScript.hitPoint.x - transform.position.x);
        int j = 0;
        for (int i = 0; i < 3; i++)
        {
            hookScript.colliderCheckPoints[i] = (Vector2)hookScript.hitPoint + new Vector2(dir, 0.5f + i);
            Collider2D[] hitList = Physics2D.OverlapCircleAll(hookScript.hitPoint + new Vector2(dir, 0.5f + i), 0.4f);
            if (hitList.Length == 0)
            {
                hookScript.colliderCheckPoint_colors[i] = Color.green;
                j++;
            }
            else
            {
                hookScript.colliderCheckPoint_colors[i] = Color.red;
            }
        }
        if (j >= 2)
        {
            hookScript.hitPoint_color = Color.green;
            hookScript.ClimbLedge();
        }
        else
        {
            hookScript.hitPoint_color = Color.red;
        }
    }
}
