using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Room transitionTo; //The room it transitions to
    private BoxCollider2D myCollider;
    [SerializeField] int roomValue;
    [SerializeField] IntTile intTile;
    private void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        if(transform.parent.transform.parent.TryGetComponent(out TilemapManager manager))
        {
            Tilemap utilityMap = manager.GetTilemap(TilemapManager.TilemapType.UTILITY);
            Tilemap modifierMap = manager.GetTilemap(TilemapManager.TilemapType.MODIFIER);
            if(GetDirection(utilityMap, out Vector2Int direction))
            {
                RemoveOtherDoors(utilityMap, direction);
            }
            Vector3Int position = new Vector3Int().AddSameValue(transform.localPosition, -0.5f);
            utilityMap.SetColor(position, Color.clear);

            roomValue = (modifierMap.GetTile(position * 2) as IntTile).value;
            Room transitionFrom = manager.transform.parent.GetComponent<Room>();
            transitionTo = transitionFrom.FetchLinkedRoom(roomValue);
            modifierMap.SetTile(position * 2, null);
        }
    }
    bool GetDirection(Tilemap map, out Vector2Int direction)
    {
        //Returns false if there's no other doors in any direction
        //Doors can be either to the left or up
        int x = map.GetTile(new Vector3Int().SetByDirection(transform.localPosition, -0.5f, 1, new Vector2Int(1, 0))) ? 1 : 0;
        int y = map.GetTile(new Vector3Int().SetByDirection(transform.localPosition, -0.5f, 1, new Vector2Int(0, 1))) ? 1 : 0;
        direction = new Vector2Int(x, y);
        return !(x == 0 && y == 0);
    }
    void RemoveOtherDoors(Tilemap map, Vector2Int direction) //Remove the door tiles and size up the collider
    {
        int w = 1;
        Vector3Int nextPosition = new Vector3Int().SetByDirection(transform.localPosition, -0.5f, w, direction);
        while (map.GetTile(nextPosition) != null)
        {
            map.SetTile(nextPosition, null); w++;
            nextPosition = new Vector3Int().SetByDirection(transform.localPosition, -0.5f, w, direction);
        }
        myCollider.size = new Vector2(1 * direction.y + w * direction.x, 1 * direction.x + w * direction.y);
        myCollider.offset = new Vector2(((w - 1) * 0.5f) * direction.x, ((w - 1) * 0.5f) * direction.y);
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
