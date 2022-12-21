using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    public void PickUp(Collider2D collision);
}
