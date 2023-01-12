using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cane : MonoBehaviour, ITool
{
    int atkCooldown;
    public int atkCooldownMax;
    Vector2 atkPos;
    float atkAngle;

    Vector2 originalCanePosition;

    public int baseAtkDamage;
    Rigidbody2D playerBody;
    Movement movement;

    [SerializeField] GameObject cane;
    [SerializeField] GameObject caneProjectile;
    [SerializeField] float projectileSpeed;

    Vector2 mousePositionSaved;
    bool mouse;

    public float attackRange = 2;

    int elapsedFrames;

    public Animator caneAnimator;

    void Start()
    {
        movement = GetComponent<Movement>();
        playerBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (atkCooldown < atkCooldownMax) 
        {
            atkCooldown++; 
        }
    }

    public void Aim(Vector2 mousePosition) //! input stuff
    {
        atkPos = mousePosition + (Vector2)transform.position;
        Vector2 clickPos = mousePositionSaved;

        if (mousePosition.x != 0)
        {
            atkAngle = mousePosition.y < 0 ? -1 * Vector2.Angle(Vector2.right, clickPos)
                : Vector2.Angle(Vector2.right, clickPos);
        }
        else { atkAngle = 90 * Mathf.Sign(mousePosition.y); }

        if(transform.localScale.x == -1)
        {
            atkAngle = -atkAngle + 180;
        }

        mousePositionSaved = mousePosition;
        mouse = true;
    }
    public void SetAngle(Vector2 direction)
    {
        atkAngle = Vector2.Angle(Vector2.right, direction);
        mouse = false;
    }

    void Hit(Vector2 point)
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(point, 1.0f, LayerMask.GetMask("Entity"), playerBody.transform.position.z,
        playerBody.transform.position.z);

        foreach (Collider2D obj in hitObjects)
        {
            if (!obj.CompareTag("Player") && !obj.CompareTag("PassThrough"))
            {
                AudioManager.PlaySFX("CaneHit");
                if (obj.gameObject.GetComponent<PuzzleSwitch>() && !obj.gameObject.GetComponent<PuzzleSwitch>().isHit)
                {
                    obj.gameObject.GetComponent<PuzzleSwitch>().ActivateSwitch();
                }
                obj.gameObject.GetComponent<IAttackable>().OnBeAttacked(baseAtkDamage, Vector2.zero);
            }
        }

    }

    private void OnDrawGizmos()
    {
        /*Vector3 mPos = Mouse.current.position.ReadValue();
        mPos.z = 18; // cam z *-1

        Vector2 lookdir = mPos - transform.position;

        float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        Vector2 mpostemp = mPos;
        mPos = new Vector2(mpostemp.y, mpostemp.x);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cane.transform.position, 0.3f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + mousePositionSaved, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position - new Vector3(0,0,5), 0.2f);*/
    }

    public bool Use()
    {
        if (atkCooldown < atkCooldownMax) { return false; }
        AudioManager.PlaySFX("Attack");

        cane.transform.localPosition = (new Vector2(Mathf.Cos(atkAngle * Mathf.Deg2Rad),
              Mathf.Sin(atkAngle * Mathf.Deg2Rad)) * attackRange);

        cane.transform.rotation = Quaternion.Euler(0, 0, atkAngle);

        caneAnimator.SetTrigger("Attack");
        Hit(cane.transform.position + (Vector3)(new Vector2(Mathf.Cos(atkAngle * Mathf.Deg2Rad),
              Mathf.Sin(atkAngle * Mathf.Deg2Rad))));

        atkCooldown = 0; // * is on cooldown
        elapsedFrames = 0;
        if(movement.IsSubmerged())
        {
            Instantiate(caneProjectile, cane.transform.position, cane.transform.rotation).TryGetComponent(out Rigidbody2D projectile);
            projectile.velocity = (cane.transform.position - transform.position).normalized * projectileSpeed;
        }
        return true;
    }
}
