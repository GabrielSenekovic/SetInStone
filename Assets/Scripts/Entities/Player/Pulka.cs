using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulka : MonoBehaviour
{
    public enum PulkaState
    {
        NONE,
        SHIELD,
        SITTING
    }
    public float pulkaAngle;
    public Vector2 pulkaDir;
    public float pulkaDistanceRad;
    [SerializeField] GameObject pulka;
    [SerializeField] GameObject cane;
    [SerializeField] Vector2 sittingPosition;

    public PulkaState state;

    void Start()
    {
        state = PulkaState.NONE;
        pulka.SetActive(false);
    }

    public void Aim(Vector2 mousePosition) //! input stuff
    {
        pulkaDir = mousePosition;
        if(mousePosition.x != 0) 
        {
            pulkaAngle = mousePosition.y < 0 ? -1 * Vector2.Angle(Vector2.right, mousePosition) 
                : Vector2.Angle(Vector2.right, mousePosition);
        }
        else {pulkaAngle = 90 * Mathf.Sign(mousePosition.y);}
    }

    public void Use()
    {
        pulka.SetActive(true);
        if(state == PulkaState.SITTING)
        {
            //Sit down in the Pulka
            pulka.transform.localPosition = sittingPosition;
            pulka.GetComponentInChildren<BoxCollider2D>().isTrigger = false;
            cane.SetActive(false);
            pulka.transform.localRotation = Quaternion.identity;
        }
        else
        {
            //Use shield
            pulka.GetComponentInChildren<BoxCollider2D>().isTrigger = true;
            pulka.transform.localPosition = (new Vector2(Mathf.Cos(pulkaAngle * Mathf.Deg2Rad), 
                Mathf.Sin(pulkaAngle * Mathf.Deg2Rad)) * pulkaDistanceRad);

            pulka.transform.localRotation = Quaternion.Euler(0, 0, pulkaAngle + 90);
        }
    }

    public void Dismount()
    {
        Debug.Log("Dismounting");
        //*If you stop holding the shield, it goes away. Dont set it riding pulka, because it should dismount only when you hit the ground
        state = PulkaState.NONE;
        cane.SetActive(true);
        pulka.SetActive(false);
    }
}
