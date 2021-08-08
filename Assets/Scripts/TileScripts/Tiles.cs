using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Empty,
    Dirt,
    Stone
}
public struct TileData
{
    public float hitPoints;
    public float miningHp;
    public float timeAlive;

    public TileData(float hitPoints, float miningHp)
    {
        this.hitPoints = hitPoints;
        this.miningHp = miningHp;
        timeAlive = 0;
    }
}
public class Tiles : MonoBehaviour
{
    private static Tile[] tiles;
    [SerializeField] private Tile[] serializedTiles;

    private void Awake()
    {
        tiles = serializedTiles;
    }
    public static TileData Dirt()
    {
        return GetTile(TileType.Dirt);
    }
    public static TileData Stone()
    {
        return GetTile(TileType.Stone);
    }

    public static TileData GetTile(TileType tile)
    {
        for (int i = 0; i < tiles.Length; i++)
        {

            if (tile.ToString() == tiles[i].name)
            {
                return new TileData(tiles[i].hitPoints, tiles[i].miningStr);
            }

        }
        return new TileData(tiles[0].hitPoints, tiles[0].miningStr) ;
    }
}
