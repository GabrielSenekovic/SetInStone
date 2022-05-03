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
    [SerializeField] GameObject animatedCane;
    public GameObject caneStartPos;

    Vector2 caneNextPosition;

    Vector2 mousePositionSaved;

    Vector2 temp;

    public float attackRange = 2;

    int elapsedFrames;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        originalCanePosition = caneStartPos.transform.localPosition;
    }

    void FixedUpdate()
    {
        if (atkCooldown < atkCooldownMax) 
        {
            MoveCane();
            atkCooldown++; 
        }
        else
        {
            cane.SetActive(false);
        }
    }

    public void AimAttack(Vector2 mousePosition) //! input stuff
    {
        atkPos = mousePosition + (Vector2)transform.position;
        //Vector2 offsetMouesPos = (mousePosition);
        //save magnitude
        Vector2 clickPos = mousePositionSaved;
        Vector2 headPos = originalCanePosition;
        Vector2 headToClick = clickPos - headPos;

        if (mousePosition.x != 0)
        {
            atkAngle = mousePosition.y < 0 ? -1 * Vector2.Angle(Vector2.right, headToClick)
                : Vector2.Angle(Vector2.right, headToClick);
        }
        else { atkAngle = 90 * Mathf.Sign(mousePosition.y); }

        //if (mousePosition.x != 0)
        //{
        //    atkAngle = Mathf.Atan2(mousePosition.y + originalCanePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        //}
        //else { atkAngle = 90 * Mathf.Sign(mousePosition.y); }

        //cane.transform.position = mousePosition + (Vector2)transform.position;

        mousePositionSaved = mousePosition;
    }

    void MoveCane()
    {
        float stepAmount = (float)elapsedFrames / (atkCooldownMax/2);// atkCooldownMax;
        stepAmount = stepAmount * stepAmount * (3f - 2f * stepAmount);
        Vector2 startPos = (Vector2)transform.position + originalCanePosition;
        Vector2 endPos = caneNextPosition;

        Vector3 interpolatedPosition = Vector3.Lerp(startPos,endPos , stepAmount);
        cane.transform.position = interpolatedPosition;
        elapsedFrames += (atkCooldown > atkCooldownMax / 2) ? -1 : 1;   // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)
        MoveHitBox(interpolatedPosition);
    }

    void MoveHitBox(Vector2 point)
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(point, 1.0f, LayerMask.GetMask("Entity"), playerBody.transform.position.z,
       playerBody.transform.position.z);

        foreach (Collider2D obj in hitObjects)
        {
            if (!obj.CompareTag("Player"))
            {
                Debug.Log("Hit object " + obj.ToString());
                AudioManager.PlaySFX("CaneHit");
                if (obj.gameObject.GetComponent<PuzzleSwitch>() && !obj.gameObject.GetComponent<PuzzleSwitch>().isHit)
                {
                    obj.gameObject.GetComponent<PuzzleSwitch>().switchHit();
                }
                obj.gameObject.GetComponent<Attackable>().OnBeAttacked(baseAtkDamage);
            }
        }

    }

    public bool Attack()
    {
        if(atkCooldown < atkCooldownMax) {return false;}
        Debug.Log("Attacking");
        AudioManager.PlaySFX("Attack");

        //Debug.Log(mousePositionSaved.normalized);

        //caneNextPosition = (new Vector2(Mathf.Cos(atkAngle * Mathf.Deg2Rad),
        //      Mathf.Sin(atkAngle * Mathf.Deg2Rad)) * attackRange); // mouse pos is 0,0 over the player pos. so you have to ofset it. then we dont want the full length. we want the attack range to be the magnitute 
        Vector2 clickPos = mousePositionSaved + (Vector2)transform.position;
        Vector2 headPos =(Vector2)transform.position + originalCanePosition;
        caneNextPosition = headPos + (clickPos - headPos).normalized * attackRange;

        //cane.transform.localPosition = (new Vector2(Mathf.Cos(atkAngle * Mathf.Deg2Rad),
        //       Mathf.Sin(atkAngle * Mathf.Deg2Rad)) * attackRange);

        cane.transform.localPosition = originalCanePosition;

        float angle = Vector2.Angle(Vector2.up, mousePositionSaved + originalCanePosition); // *2

        if(caneNextPosition.x > transform.position.x)
        {
            angle *= -1;
        }

        //if (mousePositionSaved.x != 0)
        //{
        //    atkAngle = mousePositionSaved.y < 0 ? -1 * Vector2.Angle(Vector2.right, mousePositionSaved)
        //        : Vector2.Angle(Vector2.right, mousePositionSaved);
        //}
        //else { atkAngle = 90 * Mathf.Sign(mousePositionSaved.y); }
        cane.transform.rotation = Quaternion.Euler(0, 0, atkAngle - 90.0f);

        cane.SetActive(true);

        atkCooldown = 0; // * is on cooldown
        elapsedFrames = 0;
        return true;
    }

    private void OnDrawGizmos()
    {
        Vector3 mPos = Mouse.current.position.ReadValue();
        mPos.z = 18; // cam z *-1

        //mPos = Game.Instance.mouseCamera.ScreenToWorldPoint(mPos);

        Vector2 lookdir = mPos - transform.position;

        float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        Vector2 mpostemp = mPos;
        mPos = new Vector2(mpostemp.y, mpostemp.x);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cane.transform.position, 0.3f);
        //Gizmos.DrawLine(cane.transform.position, (Vector2)cane.transform.position + caneNextPosition);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + mousePositionSaved, 0.1f);
        //Gizmos.DrawSphere(cane.transform.localPosition + transform.position, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + (Vector3)originalCanePosition, 0.2f);
        //Gizmos.DrawWireSphere(lookdir, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position - new Vector3(0,0,5), 0.2f);
        //Gizmos.DrawWireSphere(temp + (Vector2)transform.position, 0.2f);
        //Gizmos.DrawLine(transform.position, Vector3.up * 1000 + transform.position);
        //Gizmos.DrawLine(transform.position, mousePositionSaved - originalCanePosition);
    }
}
