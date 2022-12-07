using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public Inventory inventory;
    public Inventory.Tools tool;
    bool canBeInteractedWith = true;
    public Inventory.Currency currency;

    public bool CanBeInteractedWith() => canBeInteractedWith;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void Interact()
    {
        if(currency != 0)
        {
            inventory.AddCurrency(Inventory.Currency.ACORN, 5);
        }
        else
        {
            inventory.UnlockTool(tool);
        }
        canBeInteractedWith = false;
    }
}
