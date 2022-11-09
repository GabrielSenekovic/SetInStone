using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    public enum ACTIVATABLETYPE //This is kind of a really stupid way of doing this lol. This should be done with interfaces instead
    {
        DOOR,
        LAMP,
        PLATFORM
    }
    public ACTIVATABLETYPE type;
    // Start is called before the first frame update
    public void Activate()
    {
        switch(type)
        {
            case ACTIVATABLETYPE.DOOR: 
            GetComponent<RoomDoor>().doorOpening = true;
            AudioManager.PlaySFX("DoorOpen");
            break;
            case ACTIVATABLETYPE.LAMP:
            GetComponent<Animator>().SetTrigger("Activate");
            break;
            case ACTIVATABLETYPE.PLATFORM:
            GetComponent<MovingPlatform>().isOn = true;
            break;
        }
    }
}
