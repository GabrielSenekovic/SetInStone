using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    int atkCooldown;
    public int atkCooldownMax;
    public Vector2 atkPos;
    float atkAngle;

    public Vector2 originalCanePosition;

    public int baseAtkDamage;
    Rigidbody2D playerBody;

    [SerializeField] GameObject cane;

    Vector2 caneNextPosition;

    Vector2 mousePositionSaved;

    public float attackRange = 2;

    int elapsedFrames;

    public Animator caneAnimator;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (atkCooldown < atkCooldownMax) 
        {
            atkCooldown++; 
        }
    }

    public void AimAttack(Vector2 mousePosition) //! input stuff
    {
        atkPos = mousePosition + (Vector2)transform.position;
        Vector2 clickPos = mousePositionSaved;
        Vector2 headPos = originalCanePosition;
        Vector2 headToClick = clickPos - headPos;

        if (mousePosition.x != 0)
        {
            atkAngle = mousePosition.y < 0 ? -1 * Vector2.Angle(Vector2.right, headToClick)
                : Vector2.Angle(Vector2.right, headToClick);
        }
        else { atkAngle = 90 * Mathf.Sign(mousePosition.y); }

        if(transform.localScale.x == -1)
        {
            atkAngle = -atkAngle + 180;
        }

        mousePositionSaved = mousePosition;
    }

    void MoveHitBox(Vector2 point)
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(point, 1.5f, LayerMask.GetMask("Entity"), playerBody.transform.position.z,
       playerBody.transform.position.z);

        foreach (Collider2D obj in hitObjects)
        {
            if (!obj.CompareTag("Player") && !obj.CompareTag("PassThrough"))
            {
                AudioManager.PlaySFX("CaneHit");
                if (obj.gameObject.GetComponent<PuzzleSwitch>() && !obj.gameObject.GetComponent<PuzzleSwitch>().isHit)
                {
                    obj.gameObject.GetComponent<PuzzleSwitch>().SwitchHit();
                }
                obj.gameObject.GetComponent<Attackable>().OnBeAttacked(baseAtkDamage, Vector2.zero);
            }
        }

    }

    public bool Attack()
    {
        if(atkCooldown < atkCooldownMax) {return false;}
        AudioManager.PlaySFX("Attack");

        Vector2 clickPos = mousePositionSaved + (Vector2)transform.position;
        Vector2 headPos =(Vector2)transform.position + originalCanePosition;
        caneNextPosition = headPos + (clickPos - headPos).normalized * attackRange;

        cane.transform.localPosition = (new Vector2(Mathf.Cos(atkAngle * Mathf.Deg2Rad),
              Mathf.Sin(atkAngle * Mathf.Deg2Rad)) * attackRange);

        float angle = Vector2.Angle(Vector2.up, mousePositionSaved + originalCanePosition); // *2

        if(caneNextPosition.x > transform.position.x)
        {
            angle *= -1;
        }
        cane.transform.rotation = Quaternion.Euler(0, 0, atkAngle);

        caneAnimator.SetTrigger("Attack");
        MoveHitBox(cane.transform.position + (Vector3)(new Vector2(Mathf.Cos(atkAngle * Mathf.Deg2Rad),
              Mathf.Sin(atkAngle * Mathf.Deg2Rad))));

        atkCooldown = 0; // * is on cooldown
        elapsedFrames = 0;
        return true;
    }

    private void OnDrawGizmos()
    {
        Vector3 mPos = Mouse.current.position.ReadValue();
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
        Gizmos.DrawSphere(transform.position + (Vector3)originalCanePosition, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position - new Vector3(0,0,5), 0.2f);
    }
}
