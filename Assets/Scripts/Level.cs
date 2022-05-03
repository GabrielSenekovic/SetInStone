using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]List<Room> rooms = new List<Room>();
    void Awake()
    {
        rooms = GetComponentsInChildren<Room>().ToList();
    }
    private void Start() 
    {
        /*CollectAllTilePositions();*/
    }

    void CollectAllTilePositions()
    {
        List<Map.RoomData> levelData = new List<Map.RoomData>();
        Vector2Int verticalBounds = Vector2Int.zero; // x = bottom, y = top
        Vector2Int horizontalBounds = Vector2Int.zero; // x = left, y = right

        for (int i = 0; i < rooms.Count; i++) //rooms.Count
        {
            levelData.Add(new Map.RoomData(new List<Map.TileData>(), rooms[i].discovered, rooms[i].gameObject.name)); //Add this room to the level list
            List<BoxCollider2D> tilesInRoom = rooms[i].GetComponentsInChildren<BoxCollider2D>().ToList();
            for(int j = 0; j < tilesInRoom.Count; j++)
            {
                if(tilesInRoom[j].gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Entity") || tilesInRoom[j].gameObject.CompareTag("DontDraw")){continue;}
                Color color = GetTileColor(tilesInRoom[j].gameObject.layer);
                
                if (color == Color.red)
                {
                    Vector2 temp = (Vector2)tilesInRoom[j].gameObject.transform.position + tilesInRoom[j].GetComponent<BoxCollider2D>().offset;
                    levelData[i].doorPosition = Vector2Int.FloorToInt(temp); //Door position does not draw the door. It only draws the background
                }

                if (tilesInRoom[j].transform.position.x < horizontalBounds.x) { horizontalBounds.x = (int)tilesInRoom[j].transform.position.x; }
                if (tilesInRoom[j].transform.position.x > horizontalBounds.y) { horizontalBounds.y = (int)tilesInRoom[j].transform.position.x; }
                if (tilesInRoom[j].transform.position.y < verticalBounds.x) { verticalBounds.x = (int)tilesInRoom[j].transform.position.y; }
                if (tilesInRoom[j].transform.position.y > verticalBounds.y) { verticalBounds.y = (int)tilesInRoom[j].transform.position.y; }

                for(int k = 0; k < tilesInRoom[j].size.y; k++)
                {
                    levelData[i].tiles.Add(new Map.TileData(Vector2Int.FloorToInt(new Vector2(tilesInRoom[j].transform.position.x, tilesInRoom[j].transform.position.y - k)), color));
                }
            }
        }
        AdjustTilePositions(levelData, verticalBounds, horizontalBounds);
    }
    Color GetTileColor(int value)
    {
        //Use the material to get the color later
        return value == 8 ? Color.yellow : Color.red;
    }

    void AdjustTilePositions(List<Map.RoomData> list, Vector2 verticalBounds, Vector2 horizontalBounds)
    {
        //Script to make sure all Vector2 are above 0

        float xMod = horizontalBounds.x < 0 ? Mathf.Abs(horizontalBounds.x) : 0; //If the tiles start at a negative position, push it up if the tiles are at -16, add 16 to them
        float yMod = verticalBounds.y > 0 ? Mathf.Abs(verticalBounds.y) : 0;

        Vector2 oldMinBounds = new Vector2(horizontalBounds.x, verticalBounds.y);

        horizontalBounds = new Vector2(0,
                                       horizontalBounds.y + Mathf.Abs(horizontalBounds.x));
        verticalBounds = new Vector2(verticalBounds.x + Mathf.Abs(verticalBounds.y),
                                     0);

        Vector2 middlePoint = new Vector2(horizontalBounds.y / 2, verticalBounds.x / 2); //Middle point of stage

        Vector2 vectorToMiddleOfMap = new Vector2(500 - middlePoint.x, 500 - middlePoint.y);

        xMod += vectorToMiddleOfMap.x;
        yMod += vectorToMiddleOfMap.y;

        for (int i = 0; i < list.Count; i++)
        {
            for(int j = 0; j < list[i].tiles.Count; j++)
            {
                list[i].tiles[j].position = new Vector2Int((int)((float)list[i].tiles[j].position.x + xMod), (int)((float)list[i].tiles[j].position.y - yMod));
            }
            list[i].doorPosition.Set((int)((float)list[i].doorPosition.x + xMod), (int)((float)list[i].doorPosition.y - yMod));
        }

        Game.CreateMapTexture(list);
    }
}
