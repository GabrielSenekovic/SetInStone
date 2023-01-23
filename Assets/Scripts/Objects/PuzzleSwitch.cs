﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PuzzleSwitch : MonoBehaviour, IAttackable, ISwitch
{
    [SerializeField] List<Activatable> activatables = new List<Activatable>();
    bool isHit;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isHit = false;
    }

    public void ActivateSwitch()
    {
        isHit = true;
        AudioManager.PlaySFX("SwitchHit");
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = false;
        StartCoroutine("ActivationCutscene");
        anim.SetTrigger("Activate");
    }

    public void OnBeAttacked(int value, Vector2 dir)
    {
        if(!isHit)
        {
            ActivateSwitch(); //This dowes noting atm the switch is activated in playerattack
        }
    }
    public IEnumerator ActivationCutscene() //The same as activate when defeat enemies
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

    public bool IsActivated() => isHit;
}
