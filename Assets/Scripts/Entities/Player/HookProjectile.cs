using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HookProjectile : MonoBehaviour
{
    public Rigidbody2D body;
    public HookShot hookScript;
    public VisualEffect hitEffect;
    public Rigidbody2D playerRb;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerRb = hookScript.gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!hookScript.hit && !(other.CompareTag("Player") || other.CompareTag("PassThrough") ) && other.gameObject.layer != LayerMask.NameToLayer("Pickup"))
        {
            Debug.Log(other.name);
            hookScript.hit = true;
            body.gravityScale = 0; body.velocity = Vector2.zero; //Stop the hook

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
            }
            else
            {
                hookScript.hitPoint_color = Color.red;
            }

            hitEffect.Play();
            if(!hookScript.shooting)
            {
                hookScript.PullPlayer(body.position);
                hookScript.retract = true;
            }
        }
    }
}
