using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public enum SetValue
    {
        OFF,
        ON,
        TOGGLE
    }
    public static void SetCanvasGroupAs(this CanvasGroup cg, SetValue val)
    {
       switch(val)
        {
            case SetValue.OFF:
                cg.alpha = 0;
                cg.blocksRaycasts = false;
                cg.interactable = false;
                break;
            case SetValue.ON:
                cg.alpha = 1;
                cg.blocksRaycasts = true;
                cg.interactable = true;
                break;
            case SetValue.TOGGLE:
                cg.alpha = cg.alpha == 1 ? 0 : 1;
                cg.blocksRaycasts = cg.alpha == 1 ? true : false;
                cg.interactable = cg.alpha == 1 ? true : false;
                break;
        }
    }
    public static Vector3Int AddSameValue(this Vector3Int self, Vector3 origin, float value)
    {
        return new Vector3Int((int)(origin.x + value), (int)(origin.y + value), 0);
    }
    public static Vector3Int SetByDirection(this Vector3Int self, Vector3 origin, float value, int value2, Vector2Int dir)
    {
        //A function that uses the same value for x and y, but uses dir to determine if it's only horizontal or only vertical
        //value is a value that is added to the vector before the check
        //value2 is a value that is multiplied before added to the vector
        return new Vector3Int((int)(origin.x + value) + value2 * dir.x, (int)(origin.y + value) + value2 * dir.y, 0);
    }
}
