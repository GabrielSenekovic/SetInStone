using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Room transitionTo; //The room it transitions to
    private BoxCollider2D myCollider;
    [SerializeField] int roomValue;
    private void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        if(transform.parent.transform.parent.TryGetComponent(out TilemapManager manager))
        {
            Tilemap utilityMap = manager.GetTilemap(TilemapManager.TilemapType.UTILITY);
            Tilemap modifierMap = manager.GetTilemap(TilemapManager.TilemapType.MODIFIER);
            RemoveOtherDoors(utilityMap);
            Vector3Int position = new Vector3Int((int)(transform.localPosition.x - 0.5f), (int)(transform.localPosition.y - 0.5f), 0);
            utilityMap.SetColor(position, Color.clear);
            IntTile intTile = modifierMap.GetTile(position * 2) as IntTile;
            roomValue = intTile.value;
            Room transitionFrom = manager.transform.parent.GetComponent<Room>();
            transitionTo = transitionFrom.FetchLinkedRoom(roomValue);
        }
    }
    private void RemoveOtherDoors(Tilemap map)
    {
        int y = 1;
        Vector3Int nextPosition = new Vector3Int((int)(transform.localPosition.x - 0.5f), (int)(transform.localPosition.y - 0.5f) + y,0);
        while (map.GetTile(nextPosition) != null)
        {
            map.SetTile(nextPosition, null); y++;
            nextPosition = new Vector3Int((int)(transform.localPosition.x - 0.5f), (int)(transform.localPosition.y - 0.5f) + y, 0);
        }
        myCollider.size = new Vector2(1, y);
        myCollider.offset = new Vector2(0, (y - 1) * 0.5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.transform.parent.GetComponent<Movement>())
        {
            collision.GetComponent<HealthModel>().safePos = collision.transform.position;
            if (Game.GetCurrentCameraCollider() != transitionTo.GetCollider())
            {
                Game.SetCameraCollider(transitionTo.GetCollider());
            }
        }
    }
}
