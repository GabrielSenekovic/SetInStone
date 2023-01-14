using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyProjectile : MonoBehaviour
{
    public bool hit;
    public Snail snailScript;
    Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 atkDir, float atkSpeed, float atkAngle, Vector2 bodyPosition, bool gravity = false)
    {
        hit = false;
        transform.position = bodyPosition;
        transform.rotation = Quaternion.Euler(0, 0, atkAngle * Mathf.Rad2Deg);
        body.gravityScale = 0;
        body.velocity = atkDir.normalized * atkSpeed; 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("PassThrough")){return;}

        if(!hit && (other.CompareTag("Shield") || other.CompareTag("Player")))
        {
            //AudioManager.PlaySFX("Block");
        }
        AudioManager.PlaySFX("SnailShotHit");
        gameObject.SetActive(false);
    }
}
