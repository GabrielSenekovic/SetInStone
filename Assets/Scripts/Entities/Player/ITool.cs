using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITool
{
    public bool Use();
    public void Aim(Vector2 vector2);
    public void SetAngle(Vector2 direction);
}
