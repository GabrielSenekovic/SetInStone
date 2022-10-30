using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public Inventory inventory;
    public Inventory.Tools tool;
    public void Interact()
    {
        inventory.UnlockTool(tool);
    }
}
