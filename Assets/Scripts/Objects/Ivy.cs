using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class Ivy : MonoBehaviour
{
    public List<Ivy> adjacentIvy = new List<Ivy>();
    ParticleSystem fire;
    public bool onFire;

    int fireTimer = 0;
    int fireTimer_Max = 40;
    int spreadTimer_Max = 10;
    Tilemap map;

    RoomDoor door;
    private void Start() 
    {
        fireTimer_Max = Random.Range(30, 50);
        fire = GetComponentInChildren<ParticleSystem>();
        if(transform.parent.transform.parent.GetComponent<TilemapManager>())
        {
            map = transform.parent.transform.parent.GetComponent<TilemapManager>().GetTilemap(TilemapManager.TilemapType.IVY);
        }
        if(map != null)
        {
            Vector2Int[] directions = {new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(0,1), new Vector2Int(-1,1), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,-1), new Vector2Int(1,-1)};
            for(int i = 0; i < 8; i++)
            {
                GameObject temp = map.GetInstantiatedObject(new Vector3Int((int)(transform.localPosition.x) + directions[i].x, (int)(transform.localPosition.y - 0.5f) + directions[i].y, 0));
                if(temp)
                {
                    adjacentIvy.Add(temp.GetComponent<Ivy>());
                }
            }
        }
    }
    private void FixedUpdate() 
    {
        if(!onFire){return;}
        fireTimer++;
        if(fireTimer >= spreadTimer_Max)
        {
            spreadTimer_Max = Random.Range(fireTimer, fireTimer_Max);
            int amountToSpread = Random.Range(0, adjacentIvy.Count);
            int hasSpreadTo = 0;
            for(int i = 0; i < adjacentIvy.Count; i++)
            {
                if(hasSpreadTo > amountToSpread)
                {
                    break;
                }
                if(adjacentIvy[i].onFire)
                {
                    adjacentIvy.RemoveAt(i); i--; continue;
                }
                hasSpreadTo++;
                adjacentIvy[i].SetOnFire();
            }
        }
        if(fireTimer >= fireTimer_Max && map != null)
        {
            for(int i = 0; i < adjacentIvy.Count; i++)
            {
                if(!adjacentIvy[i].onFire)
                {
                    adjacentIvy[i].SetOnFire();
                }
            }
            map.SetColor(new Vector3Int((int)(transform.localPosition.x), (int)(transform.localPosition.y - 0.5f), 0), Color.clear);
            fire.Stop();
            if(door != null && door.jamCounter > 0)
            {
                door.UnJam();
                if(door.CanOpen())
                {
                    StartCoroutine("DoorOpenCutscene");
                }
            }
            else if(door == null)
            {
                if(fire.particleCount == 0)
                {
                    map.SetTile(new Vector3Int((int)(transform.localPosition.x), (int)(transform.localPosition.y - 0.5f), 0), null);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.transform.parent.GetComponent<RoomDoor>())
        {
            door = other.transform.parent.GetComponent<RoomDoor>();
            door.Jam();
        }
        if(other.CompareTag("Player") && other.transform.parent.GetComponent<Movement>().OnFire())
        {
            SetOnFire();
        }
    }
    public void SetOnFire()
    {
        onFire = true;
        fire.Play();
    }
    public IEnumerator DoorOpenCutscene()
    {
        Transform temp = Game.Instance.cinemachineVirtualCamera.Follow;
        yield return new WaitForSeconds(1.0f);
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = false;
        Game.Instance.cinemachineVirtualCamera.Follow = door.transform;
        yield return new WaitForSeconds(0.4f);
        door.GetComponent<Activatable>().Activate();
        AudioManager.PlaySFX("Discovery");
        yield return new WaitForSeconds(0.8f);
        Game.Instance.cinemachineVirtualCamera.Follow = temp;
        Game.Instance.cinemachineVirtualCamera.gameObject.GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = true;
        map.SetTile(new Vector3Int((int)(transform.localPosition.x), (int)(transform.localPosition.y - 0.5f), 0), null);
    }
}