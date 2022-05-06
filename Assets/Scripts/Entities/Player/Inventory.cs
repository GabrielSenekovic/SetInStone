using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    bool hasPulka = false;
    bool hasAxe = false;
    bool hasHookshot = true;

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
}
