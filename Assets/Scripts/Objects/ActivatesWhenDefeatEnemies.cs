using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActivatesWhenDefeatEnemies : MonoBehaviour
{
    [SerializeField] List<Activatable> activatables = new List<Activatable>();
    [SerializeField] List<EnemyHealth> enemies = new List<EnemyHealth>();
    [SerializeField] bool closeDoorBehindPlayer;

    private void Awake()
    {
        Debug.Assert(activatables.Count > 0);
        Debug.Assert(enemies.Count > 0);
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].InsertRoom(this);
        }
    }
    public void RemoveEnemy(EnemyHealth enemy)
    {
        enemies.Remove(enemy);
        if(enemies.Count == 0)
        {
            for (int i = 0; i < activatables.Count; i++)
            {
                activatables[i].Activate();
            }
            StartCoroutine(ActivationCutscene());
        }
    }

    public IEnumerator ActivationCutscene() //The same as puzzleswitch
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
}
