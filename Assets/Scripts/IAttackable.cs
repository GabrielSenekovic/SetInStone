using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void OnBeAttacked(int value, Vector2 dir);
}
