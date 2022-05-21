using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HookProjectile : MonoBehaviour
{
    [System.NonSerialized]public Rigidbody2D body;
    public HookShot hookScript;
    public ParticleSystem particles;
    public Rigidbody2D playerRb;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerRb = hookScript.gameObject.GetComponent<Rigidbody2D>();
        Debug.Log("bababuey");
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!hookScript.hit && !(other.CompareTag("Player") || other.CompareTag("PassThrough") ||  other.CompareTag("Water")) && other.gameObject.layer != LayerMask.NameToLayer("Pickup"))
        {
            body.gravityScale = 0; body.velocity = Vector2.zero; //Stop the hook
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.25f);
            for(int i = 0; i < hits.Length; i++)
            {
                if(hits[i].GetComponent<Ivy>())
                {
                    hookScript.StopPull();
                    hookScript.retract = true;
                    return;
                }
            }
            hookScript.hit = true;

            playerRb.gravityScale = 0;
            playerRb.velocity = Vector2.zero;

            hookScript.hitPoint = other.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);
            hookScript.hitPoint = new Vector2(Mathf.RoundToInt(hookScript.hitPoint.x), Mathf.CeilToInt(hookScript.hitPoint.y));

            int dir = (int)Mathf.Sign(hookScript.hitPoint.x - transform.position.x);

            int j = 0;
            for(int i = 0; i < 3; i++)
            {
                hookScript.colliderCheckPoints[i] = (Vector2)hookScript.hitPoint + new Vector2(dir, 0.5f + i);
                Collider2D[] hitList = Physics2D.OverlapCircleAll(hookScript.hitPoint + new Vector2(dir, 0.5f + i), 0.4f);
                if(hitList.Length == 0)
                {
                    hookScript.colliderCheckPoint_colors[i] = Color.green;
                    j++;
                }
                else
                {
                    hookScript.colliderCheckPoint_colors[i] = Color.red;
                }
            }
            if(j >= 2)
            {
                hookScript.hitPoint_color = Color.green;
                hookScript.ClimbLedge();
            }
            else
            {
                hookScript.hitPoint_color = Color.red;
            }
            particles.Play();
            if(!hookScript.shooting)
            {
                hookScript.PullPlayer(body.position);
                hookScript.retract = true;
            }
        }
    }
}
