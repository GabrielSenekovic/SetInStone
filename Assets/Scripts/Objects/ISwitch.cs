using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitch
{
    public void ActivateSwitch();
    public IEnumerator ActivationCutscene();
}
