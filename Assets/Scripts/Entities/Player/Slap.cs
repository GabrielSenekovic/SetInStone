using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slap : MonoBehaviour, ITool
{
    int atkCooldown;
    public int atkCooldownMax;
    public Vector2 atkPos;
    Rigidbody2D playerBody;
    public float atkAngle;
    Vector2 mousePositionSaved;
    public bool mouse;

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
    public void Aim(Vector2 vector2)
    {
        atkPos = vector2 + (Vector2)transform.position;
        Vector2 clickPos = mousePositionSaved;

        if (vector2.x != 0)
        {
            atkAngle = vector2.y < 0 ? -1 * Vector2.Angle(Vector2.right, clickPos)
                : Vector2.Angle(Vector2.right, clickPos);
        }
        else { atkAngle = 90 * Mathf.Sign(vector2.y); }

        if (transform.localScale.x == -1)
        {
            atkAngle = -atkAngle + 180;
        }

        mousePositionSaved = vector2;
        mouse = true;
    }

    public void SetAngle(Vector2 direction)
    {
    }

    void MoveHitBox(Vector2 point)
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(point, 1.0f, LayerMask.GetMask("Entity"), playerBody.transform.position.z,
        playerBody.transform.position.z);

        foreach (Collider2D obj in hitObjects)
        {
            if (!obj.CompareTag("Player") && !obj.CompareTag("PassThrough"))
            {
                obj.gameObject.GetComponent<IAttackable>().OnBeAttacked(0, (atkPos - (Vector2)transform.position));
            }
        }

    }

    public bool Use()
    {
        if (atkCooldown < atkCooldownMax) { return false; }

        MoveHitBox((Vector3)(new Vector2(Mathf.Cos(atkAngle * Mathf.Deg2Rad),
              Mathf.Sin(atkAngle * Mathf.Deg2Rad))*2) + transform.position);

        atkCooldown = 0;
        return true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(Mathf.Cos(atkAngle * Mathf.Deg2Rad),
              Mathf.Sin(atkAngle * Mathf.Deg2Rad)) * 2 + (Vector2)transform.position, 1.0f);
    }
}
