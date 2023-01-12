using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Fireplace : MonoBehaviour, IFlammable, ISwitch
{
    [SerializeField] List<Activatable> activatables = new List<Activatable>();
    [SerializeField] bool onFire;
    Animator anim;
    Fire fire;
    void Start()
    {
        fire = GetComponentInChildren<Fire>();
        anim = fire.gameObject.GetComponent<Animator>();
        fire.gameObject.SetActive(onFire);
    }
    public void ActivateSwitch()
    {
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = false;
        StartCoroutine("ActivationCutscene");
        anim.SetTrigger("Activate");
    }

    public void SetOnFire()
    {
        onFire = true;
        fire.gameObject.SetActive(onFire);
        ActivateSwitch();
    }
    public IEnumerator ActivationCutscene() //The same as activate when defeat enemies
    {
        Transform temp = Game.Instance.cinemachineVirtualCamera.Follow;
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < activatables.Count; i++)
        {
            Game.Instance.cinemachineVirtualCamera.Follow = activatables[i].transform;
            yield return new WaitForSeconds(0.4f);
            activatables[i].Activate();
            if (i == activatables.Count - 1)
            {
                AudioManager.PlaySFX("Discovery");
            }
            yield return new WaitForSeconds(0.4f);
        }
        Game.Instance.cinemachineVirtualCamera.Follow = temp;
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = true;
    }

    public bool OnFire() => onFire;
}
