using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public static class World
{
    private static bool worldCreated = false;
    private static NativeArray<TileType> world;
    private static Dictionary<int, TileData> activeTiles;
    private static int worldWidth;
    private static int worldHeight;

    public const float gravity = 0.7f;

    public static void CreateWorld(int width, int height, NativeArray<TileType> newWorld)
    {
        if (!worldCreated)
        {
            world = newWorld;
            worldWidth = width;
            worldHeight = height;
            activeTiles = new Dictionary<int, TileData>();
            worldCreated = true;
        }
    }
    public static void ResetWorld()
    {
        worldCreated = false;
    }
    public static void SetTile(int index, TileType value)
    {
        world[index] = value;
    }
    public static void SetTile(Vector2 pos, TileType value)
    {
        world[Index(pos)] = value;
    }
    public static TileType GetTile(int index)
    {
        return world[index];
    }
    public static TileType GetTile(int x, int y)
    {
        return world[Index(x, y)];
    }
    public static TileType GetTile(Vector2 pos)
    {
        return world[Index(pos)];
    }
    public static int GetWidth()
    {
        return worldWidth;
    }
    public static int GetHeight()
    {
        return worldHeight;
    }
    public static int Index(float x, float y)
    {
        return Mathf.Clamp(worldHeight * Mathf.FloorToInt(x) + Mathf.FloorToInt(y), 0, (worldWidth * worldHeight) - 1);
    }
    public static int Index(Vector2 pos)
    {
        return Mathf.Clamp(worldHeight * Mathf.FloorToInt(pos.x) + Mathf.FloorToInt(pos.y), 0, (worldWidth * worldHeight) - 1);
    }
    public static int Index(Vector3 pos)
    {
        return Mathf.Clamp(worldHeight * Mathf.FloorToInt(pos.x) + Mathf.FloorToInt(pos.y), 0, (worldWidth * worldHeight) - 1);
    }
    public static Vector2Int Coordinates(int flatIndex)
    {
        return new Vector2Int(flatIndex / worldHeight, flatIndex % worldHeight);
    }
    public static bool IsInside(Vector2 tile, Vector2 check)
    {
        return check.x >= tile.x && check.y >= tile.y && check.x < tile.x + 1 && check.y < tile.y + 1;
    }
    public static bool IsInside(int tileIndex, Vector2 check)
    {
        Vector2Int tile = Coordinates(tileIndex);
        return check.x >= tile.x && check.y >= tile.y && check.x < tile.x + 1 && check.y < tile.y + 1;
    }
    public static bool IsOverlap(Vector2 tileOne, Vector2 tileTwo)
    {
        return tileOne.x < tileTwo.x + 1 && tileOne.x + 1 > tileTwo.x && tileOne.y < tileTwo.y + 1 && tileOne.y + 1 > tileTwo.y;
    }
    public static bool IsBlock(Vector2 pos)
    {
        return (int)world[Index(pos)] >= 1;
    }
    public static bool IsBlock(Vector3 pos)
    {
        return (int)world[Index(pos)] >= 1;
    }
    public static bool IsBlock(float x, float y)
    {
        return (int)world[Index(x, y)] >= 1;
    }
    public static IEnumerable<int> GetActiveKeys()
    {
        return activeTiles.Keys;
    }
    public static TileData GetTileData(int key)
    {
        return activeTiles[key];
    }
    public static void SetTileData(int key, TileData tile)
    {
        activeTiles[key] = tile;
    }
    public static void DeactivateTileData(int key)
    {
        activeTiles.Remove(key);
    }
    public static void ActivateTileData(int key, TileData tile)
    {
        activeTiles.Add(key, tile);
    }
    public static Dictionary<int, TileData> GetActiveTiles()
    {
        return activeTiles;
    }
}
