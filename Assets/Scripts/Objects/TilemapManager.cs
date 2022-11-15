using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TilemapManager : MonoBehaviour
{
    public enum TilemapType
    {
        IVY, //For checking for adjacent ivies
        DECORATION, //For waterfalls to make their source disappear
        WATER,
        GROUND,
        UTILITY,
        MODIFIER
    }
    [System.Serializable]public struct TilemapEntry
    {
        public Tilemap tilemap;
        public TilemapType type;
    }
    public List<TilemapEntry> tileMaps;

    public Tilemap GetTilemap(TilemapType type) => tileMaps.Where(t => t.type == type).First().tilemap;

    private void Awake()
    {
        TilemapCollider2D collider;
        for (int i = 0; i < tileMaps.Count; i++)
        {
            switch(tileMaps[i].type)
            {
                case TilemapType.DECORATION: break;
                case TilemapType.GROUND:
                    tileMaps[i].tilemap.gameObject.layer = LayerMask.NameToLayer("Ground");
                    if (!tileMaps[i].tilemap.TryGetComponent<TilemapCollider2D>(out collider))
                    {
                        collider = tileMaps[i].tilemap.gameObject.AddComponent<TilemapCollider2D>();
                    }
                    break;
                case TilemapType.IVY: break;
                case TilemapType.WATER:
                    tileMaps[i].tilemap.color = new Color32(255, 255,255,128);
                    tileMaps[i].tilemap.tag = "Water";
                    tileMaps[i].tilemap.gameObject.layer = LayerMask.NameToLayer("Water");
                    if(!tileMaps[i].tilemap.TryGetComponent<TilemapCollider2D>(out collider))
                    {
                        collider = tileMaps[i].tilemap.gameObject.AddComponent<TilemapCollider2D>();
                    }
                    collider.isTrigger = true;
                    break;
            }
        }
    }
}
