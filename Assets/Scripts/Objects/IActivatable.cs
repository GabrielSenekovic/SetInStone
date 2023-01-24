using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable
{
    public void Activate();

    public Transform Transform();
    public bool IsActivated();
    public IEnumerator ActivationCutscene();
}
