using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartCoroutine("ActivationCutscene");
        anim.SetTrigger("Activate");
    }

    public void OnBeAttacked(int value)
    {
        if(!isHit)
        {
            Debug.Log("Switch is being hit");
            SwitchHit();
        }
    }
    public IEnumerator ActivationCutscene()
    {
        Debug.Log("ACTIVATION");
        Transform temp = Game.Instance.cinemachineVirtualCamera.Follow;
        yield return new WaitForSeconds(1.0f);
        for(int i = 0; i < activatables.Count; i++)
        {
            Game.Instance.cinemachineVirtualCamera.Follow = activatables[i].transform;
            yield return new WaitForSeconds(0.4f);
            activatables[i].Activate();
            yield return new WaitForSeconds(0.4f);
        }
        Game.Instance.cinemachineVirtualCamera.Follow = temp;
    }
}
