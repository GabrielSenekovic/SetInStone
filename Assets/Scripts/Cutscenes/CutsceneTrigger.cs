using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutsceneTrigger : MonoBehaviour, ISwitch
{
    [SerializeField] List<GameObject> activatables = new List<GameObject>();
    bool activated = false;
    public void ActivateSwitch()
    {
        activated = true;
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = false;
        StartCoroutine("ActivationCutscene");
    }

    public IEnumerator ActivationCutscene()
    {
        Transform temp = Game.Instance.cinemachineVirtualCamera.Follow;
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < activatables.Count; i++)
        {
            IActivatable activatable = activatables[i].GetComponent<IActivatable>();
            Game.Instance.cinemachineVirtualCamera.Follow = activatable.Transform();
            yield return new WaitForSeconds(0.4f);
            activatable.Activate();
            if (i == activatables.Count - 1)
            {
                AudioManager.PlaySFX("Discovery");
            }
            yield return new WaitForSeconds(0.4f);
        }
        Game.Instance.cinemachineVirtualCamera.Follow = temp;
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = true;
    }

    public bool IsActivated() => activated;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsActivated())
        {
            ActivateSwitch();
        }
    }
}
