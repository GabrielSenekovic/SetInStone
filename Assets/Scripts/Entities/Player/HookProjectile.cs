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

            hitEffect.Play();
            if(!hookScript.shooting)
            {
                hookScript.PullPlayer(body.position);
                hookScript.retract = true;
            }
        }
    }
}
