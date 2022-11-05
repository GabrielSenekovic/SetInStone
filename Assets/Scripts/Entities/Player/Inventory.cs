using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [Flags]
    public enum Tools
    {
        NONE = 0,
        CANE = 1,
        SQUIDHOOK = 1 << 1,
        PULKA = 1 << 2,
        SPEAR = 1 << 3,
        AXE = 1 << 4,
        LIGHTNINGBOW = 1 << 5,
        BALL = 1 << 6,
        REVERSALROD = 1 << 7,
        PLACEMENTROD = 1 << 8,
        FISHINGROD = 1 << 9,
        WHEEL = 1 << 10,
        CLOCK = 1 << 11,
        SHOVEL = 1 << 12,
        PERSISTENCESTONE = 1 << 13,
        TENACITYSTONE = 1 << 14,
        VIGOURSTONE = 1 << 15, 
        MERCYSTONE = 1 << 16,
        AQUEOUSSTONE = 1 << 17
    }
    bool hasPulka = false;
    bool hasAxe = false;
    public bool hasHookshot = true;

    public bool HasAxe()
    {
        return hasAxe;
    }

    public bool HasHookshot()
    {
        return hasHookshot;
    }

    public bool HasPulka()
    {
        return hasPulka;
    }
    public void UnlockTool(Tools tool)
    {
        switch(tool)
        {
            case Tools.SQUIDHOOK: hasHookshot = true; break;
            default: break;
        }
    }
}
