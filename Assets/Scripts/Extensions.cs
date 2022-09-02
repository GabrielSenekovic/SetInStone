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
}
