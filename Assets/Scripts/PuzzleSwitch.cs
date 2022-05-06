using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitch : MonoBehaviour, Attackable
{
    [SerializeField] GameObject door;
    RoomDoor doorScript;
    public bool isHit;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isHit = false;
        doorScript = door.GetComponent<RoomDoor>();
    }

    public void switchHit()
    {
        isHit = true;
        AudioManager.PlaySFX("SwitchHit");
        if(doorScript)
        {
            doorScript.doorOpening = true;
            AudioManager.PlaySFX("DoorOpen");
        }
        anim.SetTrigger("Activate");
    }

    public void OnBeAttacked(int value)
    {
        if(!isHit)
        {
            Debug.Log("Switch is being hit");
            switchHit();
        }
    }
}
