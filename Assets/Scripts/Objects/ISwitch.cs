using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitch
{
    public bool IsActivated();
    public void ActivateSwitch();
    public IEnumerator ActivationCutscene();
}
