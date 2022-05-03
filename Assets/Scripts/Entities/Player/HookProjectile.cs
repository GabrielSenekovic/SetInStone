using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HookProjectile : MonoBehaviour
{
    [System.NonSerialized] public float shootDistanceMax = 50;
    Transform playerTrans;
    [System.NonSerialized] public int wallLinger; //current seconds stayed in wall
    public int wallLingerMax; //max seconds for staying in wall
    public Rigidbody2D body;
    [System.NonSerialized] public bool hit;
    [System.NonSerialized] public HookShot hookScript;
    [SerializeField] Transform seaweed;
    public VisualEffect hitEffect;
    public Movement playerMov;
    public Rigidbody2D playerRb;
    public Rigidbody2D rb;
    Animator playerAnimator;

    public bool retract;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerTrans = hookScript.gameObject.transform;
        playerMov = hookScript.gameObject.GetComponent<Movement>();
        playerRb = hookScript.gameObject.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = hookScript.playerAnimator;
        retract = false;
    }

    private void Update() 
    {
        AdjustSeaweed();
    }

    void FixedUpdate()
    {
        if(hit){wallLinger++;}

        float hookDistance = (playerTrans.position - transform.position).magnitude;
        if(!retract && hookDistance >= shootDistanceMax /*|| wallLinger >= wallLingerMax*/)
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
    void Retract()
    {
        if(playerMov.actionBuffer) //When it starts retracting, give player their movement back
        {
            playerMov.actionBuffer = false;
            playerRb.gravityScale = playerMov.normGrav;
            hitEffect.Stop();
        }

        Vector2 Dir = (playerMov.gameObject.transform.position - transform.position).normalized;
        rb.velocity = Dir * 30 * 2; //* give it velocity in that direction (hookspeed * 2)
        AdjustSeaweed();

        if((transform.position - playerMov.gameObject.transform.position).magnitude < 1)
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            retract = false;
        }
    }

    public void Shoot(Vector2 hookDir, float hookSpeed, float hookAngle, Vector2 bodyPosition)
    {
        rb = GetComponent<Rigidbody2D>();
        hit = false;
        wallLinger = 0;
        transform.position = bodyPosition;
        transform.rotation = Quaternion.Euler(0, 0, hookAngle); // * rotate the hook in the direction of the stick and...
        rb.velocity = hookDir.normalized * hookSpeed; //* give it velocity in that direction
        AdjustSeaweed();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!hit && !(other.CompareTag("Player") || other.CompareTag("PassThrough") ) && other.gameObject.layer != LayerMask.NameToLayer("Pickup"))
        {
            Debug.Log(other.name);
            hit = true;
            body.gravityScale = 0; body.velocity = Vector2.zero; //Stop the hook

            playerRb.gravityScale = 0;
            playerRb.velocity = Vector2.zero;

            hitEffect.Play();
        }
    }

    public void AdjustSeaweed()
    {
        seaweed.transform.position = new Vector2((transform.position.x + hookScript.transform.position.x)/2, 
            (transform.position.y + hookScript.transform.position.y)/2);
        
        Vector2 vectorToTarget = seaweed.transform.position - hookScript.gameObject.transform.position;
        float seaweedAngle = 90 * Mathf.Deg2Rad;
        
        if(vectorToTarget.x != 0) 
            {seaweedAngle = Mathf.Atan(vectorToTarget.y / vectorToTarget.x);}

        seaweed.rotation = Quaternion.Euler(0, 0, seaweedAngle * Mathf.Rad2Deg);
        seaweed.localScale = new Vector3(vectorToTarget.magnitude * 2 * (1 / transform.localScale.x), seaweed.localScale.y, 1);
        seaweed.GetComponent<MeshRenderer>().sharedMaterials[0].mainTextureScale = new Vector2(seaweed.localScale.x/2, 1);
    }
}
