using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PuzzleSwitch : MonoBehaviour, Attackable
{
    [SerializeField] List<Activatable> activatables = new List<Activatable>();
    public bool isHit;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isHit = false;
    }

    public void SwitchHit()
    {
        isHit = true;
        AudioManager.PlaySFX("SwitchHit");
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = false;
        StartCoroutine("ActivationCutscene");
        anim.SetTrigger("Activate");
    }

    public void OnBeAttacked(int value)
    {
        if(!isHit)
        {
            SwitchHit();
        }
    }
    public IEnumerator ActivationCutscene()
    {
        Transform temp = Game.Instance.cinemachineVirtualCamera.Follow;
        yield return new WaitForSeconds(1.0f);
        for(int i = 0; i < activatables.Count; i++)
        {
            Game.Instance.cinemachineVirtualCamera.Follow = activatables[i].transform;
            yield return new WaitForSeconds(0.4f);
            activatables[i].Activate();
            if(i == activatables.Count - 1)
            {
                AudioManager.PlaySFX("Discovery");
            }
            yield return new WaitForSeconds(0.4f);
        }
        Game.Instance.cinemachineVirtualCamera.Follow = temp;
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = true;
    }
}
