using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System.Linq;

public class Lemmy : MonoBehaviour
{
    public float targetRadius;
    public float atkRadius;
    public GameObject armPivot;
    public GameObject arm;

    Transform target;

    float atkAngle;

    public float armDistanceRad; //The distance radius of the arm from the Lemmy main body

    Vector2 nextPosition;
    float currentAngle;
    float currentRadius;

    [Range(1, 2)]public float armMovementSpeed;

    bool attacking;
    bool charging;

    public float atkTimer;
    public int atkTimerMax;

    public float atkPush;

    public int damageDealt;

    public bool canHitDown;
    Animator anim;

    public Transform side1;
    public Transform side2;
    public LayerMask whatIsGround;

    public float speed;

   // Vector3 pos1;
    //Vector3 pos2;

    private void Start() 
    {
        target = null;
        attacking = false;
        currentAngle = 0;
        charging = false;
        currentRadius = 0;
        anim = GetComponent<Animator>();
        anim.speed = speed;
    }
    private void Update() 
    {
        if(target != null && !attacking)
        {
            Vector2 atkDir = target.position - transform.position;
            if(CheckIfOutOfRange()){target = null; return;}
            else
            {
                atkAngle = atkDir.y < 0 ? -1 * Vector2.Angle(Vector2.right, atkDir) //If directon is lower than transform y, then go down instead of up
                : Vector2.Angle(Vector2.right, atkDir);
            }

            nextPosition = (new Vector2(Mathf.Cos(atkAngle * Mathf.Deg2Rad), 
                    Mathf.Sin(atkAngle * Mathf.Deg2Rad)) * armDistanceRad);
        }
    }
    private void FixedUpdate() 
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, targetRadius, LayerMask.GetMask("Entity"), 
            transform.position.z, transform.position.z);

        foreach(Collider2D obj in hitObjects)
        {
            if(obj.CompareTag("Player"))
            {
                target = obj.transform;
            }
        }
        if(hitObjects.Length == 0)
        {
            target = null;
        }
        if(target != null && !attacking && !charging && !CheckIfOutOfRange())
        {
            MoveArm();

            hitObjects = Physics2D.OverlapCircleAll(armPivot.transform.position, atkRadius, LayerMask.GetMask("Entity"), 
            transform.position.z, transform.position.z);

            foreach(Collider2D obj in hitObjects)
            {
                if(obj.CompareTag("Player"))
                {
                    attacking = true;
                    anim.SetTrigger("Attack");
                }
            }
        }
        if(attacking){Attack();}
       // if(charging){Charge();}
    }
    bool CheckIfOutOfRange()
    {
        Vector2 atkDir = target.position - transform.position;
        return !canHitDown && atkDir.y < 0;
    }
    void Attack()
    {
        atkTimer++;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(arm.transform.position, 1.2f, LayerMask.GetMask("Entity"), 
            transform.position.z, transform.position.z);

        foreach(Collider2D obj in hitObjects)
        {
            if(obj.CompareTag("Shield"))
            {
                charging = true;
                attacking = false;
                atkTimer = 0;
                obj.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody2D>().AddForce
                (armPivot.transform.localPosition.normalized * atkPush, ForceMode2D.Impulse);
                AudioManager.PlaySFX("LemmyBounce");

               // Game.Instance.visualEffects.ChangePosition(shieldHit, arm.transform.position);
               // shieldHit.effect.Play();

                return;
            }
            if(obj.CompareTag("Player") && obj.transform.parent.GetComponent<HealthModel>())
            {
                charging = true;
                attacking = false;
                atkTimer = 0;
                obj.transform.parent.GetComponent<Rigidbody2D>().AddForce
                (armPivot.transform.localPosition.normalized * atkPush / 1.2f, ForceMode2D.Impulse);
                obj.transform.parent.GetComponent<HealthModel>().TakeDamage(damageDealt);

                //Game.Instance.visualEffects.ChangePosition(shieldHit, arm.transform.position);

                return;
            }
            if(atkTimer >= atkTimerMax)
            {
                attacking = false;
                atkTimer = 0;
                charging = true;
            }
        }
    }
    public void FinishCharge() //Called from animation
    {
        attacking = false;
        charging = false; atkTimer = 0;
    }
    void MoveArm()
    {
        float angleBetween = Vector2.Angle(armPivot.transform.localPosition, nextPosition) / armMovementSpeed; //Only works from right to left, not left to right

        angleBetween = armPivot.transform.localPosition.x < nextPosition.x ? -angleBetween : angleBetween;
        //If anglebetween is positive, it goes counterclockwise, which would be side1
        Transform transformToUse = angleBetween > 0 ? side1:side2;

        Vector3 previousPositionOfTransform = transformToUse.position;
        Vector3 previousPositionOfPivot = armPivot.transform.position;

        armPivot.transform.localPosition = (new Vector2(Mathf.Cos(atkAngle* Mathf.Deg2Rad - angleBetween * Mathf.Deg2Rad), 
                    Mathf.Sin(atkAngle* Mathf.Deg2Rad - angleBetween * Mathf.Deg2Rad)) * armDistanceRad);

        if(Physics2D.LinecastAll(transformToUse.position, previousPositionOfTransform).Any(e => e.collider.gameObject.layer == Mathf.Log(whatIsGround.value,2)) || //If there is ground between the two movement points
            Physics2D.OverlapCircleAll(transformToUse.position, 1.0f/16.0f*2.0f).Any(e => e.gameObject.layer == Mathf.Log(whatIsGround.value,2))) //Or if the chosen point collides with ground
        {
            armPivot.transform.position = previousPositionOfPivot;
        }

        //armPivot.transform.localPosition = Vector2.zero;

        armPivot.transform.localRotation = Quaternion.Euler(0, 0, atkAngle - angleBetween);
       // shieldHit.effect.transform.localRotation = Quaternion.Euler(0, 0, atkAngle - angleBetween + 90);

        currentAngle = atkAngle* Mathf.Deg2Rad - angleBetween * Mathf.Deg2Rad;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
        Gizmos.color = attacking ? Color.red : charging ? (Color)new Color32(255, 125, 0, 255) : Color.cyan;
        Gizmos.DrawWireSphere(armPivot.transform.position, atkRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.2f);
        //Gizmos.color = Color.green;

       // Gizmos.DrawLine(pos1, pos2);
    }
}
