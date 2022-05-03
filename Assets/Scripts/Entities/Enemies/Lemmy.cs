using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Lemmy : MonoBehaviour
{
    public float targetRadius;
    public float atkRadius;
    public GameObject arm;

    Transform target;

    float atkAngle;

    public float armDistanceRad;

    Vector2 nextPosition;
    float currentAngle;
    float currentRadius;

    [Range(1, 2)]public float armMovementSpeed;

    bool attacking;
    bool charging;

    float atkTimer;
    public int atkTimerMax;

    public float atkPush;

    public float chargeDuration;
    [SerializeField]VisualEffectEntry shieldHit;

    public VisualEffect VFX_prefab;

    public int damageDealt;

    private void Start() 
    {
        target = null;
        attacking = false;
        currentAngle = 0;
        charging = false;
        currentRadius = 0;
        if(chargeDuration == 0)
        {
            chargeDuration = 1; //otherwise it will divide by 0
        }
        shieldHit.effect = Instantiate(VFX_prefab, transform.position, Quaternion.identity, transform);
        Game.Instance.visualEffects.Add(shieldHit, false);
    }
    private void Update() 
    {
        if(target != null && !attacking)
        {
            Vector2 atkDir = target.position - transform.position;
            atkAngle = Vector2.Angle(Vector2.up, atkDir);
            atkAngle = atkDir.y < transform.position.y ? -1 * Vector2.Angle(Vector2.right, atkDir) 
                : Vector2.Angle(Vector2.right, atkDir);

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
        if(target != null && !attacking && !charging)
        {
            MoveArm();

            hitObjects = Physics2D.OverlapCircleAll(arm.transform.position, atkRadius, LayerMask.GetMask("Entity"), 
            transform.position.z, transform.position.z);

            foreach(Collider2D obj in hitObjects)
            {
                if(obj.CompareTag("Player"))
                {
                    attacking = true;
                }
            }
        }
        if(attacking)
        {
            atkTimer++;
            Attack();
            if(atkTimer >= atkTimerMax)
            {
                attacking = false;
                atkTimer = 0;
                charging = true;
            }
        }
        if(charging)
        {
            Charge();
        }
    }
    void Attack()
    {
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
                (arm.transform.localPosition.normalized * atkPush, ForceMode2D.Impulse);
                Debug.Log("Lemmy added force");
                AudioManager.PlaySFX("LemmyBounce");

                Game.Instance.visualEffects.ChangePosition(shieldHit, arm.transform.position);
                shieldHit.effect.Play();

                return;
            }
            if(obj.CompareTag("Player") && obj.GetComponent<HealthModel>())
            {
                charging = true;
                attacking = false;
                atkTimer = 0;
                obj.GetComponent<Rigidbody2D>().AddForce
                (arm.transform.localPosition.normalized * atkPush / 1.2f, ForceMode2D.Impulse);
                obj.GetComponent<HealthModel>().TakeDamage(damageDealt);

                Game.Instance.visualEffects.ChangePosition(shieldHit, arm.transform.position);

                return;
            }
        }

        float speed = 10;
        currentRadius = armDistanceRad + Mathf.Tan(atkTimer/speed) - Mathf.Sin(atkTimer/speed) * 1.5f;

        arm.transform.localPosition = (new Vector2(Mathf.Cos(currentAngle), 
                    Mathf.Sin(currentAngle)) * currentRadius);
    }
    void Charge()
    {
        atkTimer++; //It's now being used as a chargeTimer
        
        float differenceToInterpolate = currentRadius - armDistanceRad;
        float increment = differenceToInterpolate / chargeDuration;
        float totalIncrement = increment * atkTimer;
        float middleRadius = currentRadius - totalIncrement;

        if(middleRadius <= armDistanceRad)
        {
            middleRadius = armDistanceRad;
            atkTimer = 0;
            charging = false;
        }
        arm.transform.localPosition = (new Vector2(Mathf.Cos(currentAngle), 
                    Mathf.Sin(currentAngle)) * middleRadius);
    }
    void MoveArm()
    {
        float angleBetween = Vector2.Angle(arm.transform.localPosition, nextPosition) / armMovementSpeed; //Only works from right to left, not left to right

        angleBetween = arm.transform.localPosition.x < nextPosition.x ? -angleBetween : angleBetween;

        arm.transform.localPosition = (new Vector2(Mathf.Cos(atkAngle* Mathf.Deg2Rad - angleBetween * Mathf.Deg2Rad), 
                    Mathf.Sin(atkAngle* Mathf.Deg2Rad - angleBetween * Mathf.Deg2Rad)) * armDistanceRad);

        arm.transform.localRotation = Quaternion.Euler(0, 0, atkAngle - angleBetween);
        shieldHit.effect.transform.localRotation = Quaternion.Euler(0, 0, atkAngle - angleBetween + 90);

        currentAngle = atkAngle* Mathf.Deg2Rad - angleBetween * Mathf.Deg2Rad;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
        Gizmos.color = attacking ? Color.red : charging ? (Color)new Color32(255, 125, 0, 255) : Color.cyan;
        Gizmos.DrawWireSphere(arm.transform.position, atkRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
