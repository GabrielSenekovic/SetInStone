using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public enum TilemapType
    {
        IVY, //For checking for adjacent ivies
        DECORATION //For waterfalls to make their source disappear
    }
    [System.Serializable]public struct TilemapEntry
    {
        public Tilemap tilemap;
        public TilemapType type;
    }
    public List<TilemapEntry> tileMaps;

    public Tilemap GetTilemap(TilemapType type)
    {
        for(int i = 0; i < tileMaps.Count;i++)
        {
            if(tileMaps[i].type == type)
            {
                return tileMaps[i].tilemap;
            }
        }
        return null;
    }
}
