using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitch : MonoBehaviour, Attackable
{
    [SerializeField] GameObject door;
    RoomDoor doorScript;
    public bool isHit;

    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        doorScript = door.GetComponent<RoomDoor>();
    }

    public void switchHit()
    {
        isHit = true;
        AudioManager.PlaySFX("SwitchHit");
        doorScript.doorOpening = true;
        AudioManager.PlaySFX("DoorOpen");
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
