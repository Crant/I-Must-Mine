using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public static class TileHelp
{

    //public static int GetWidth()
    //{
    //    return width;
    //}
    //public static int GetHeight()
    //{
    //    return height;
    //}
    //public static int Index(float x, float y)
    //{
    //    return Mathf.Clamp(height * Mathf.FloorToInt(x) + Mathf.FloorToInt(y), 0, (width * height) - 1);
    //}
    //public static int Index(Vector2 pos)
    //{
    //    return Mathf.Clamp(height * Mathf.FloorToInt(pos.x) + Mathf.FloorToInt(pos.y), 0, (width * height) - 1);
    //}
    //public static int Index(Vector3 pos)
    //{
    //    return Mathf.Clamp(height * Mathf.FloorToInt(pos.x) + Mathf.FloorToInt(pos.y), 0, (width * height) - 1);
    //}
    //public static Vector2Int Coordinates(int flatIndex)
    //{
    //    return new Vector2Int(flatIndex / height, flatIndex % height);
    //}
    //public static bool IsInside(Vector2 tile, Vector2 check)
    //{
    //    return check.x >= tile.x && check.y >= tile.y && check.x < tile.x + 1 && check.y < tile.y + 1;
    //}
    //public static bool IsInside(int tileIndex, Vector2 check)
    //{
    //    Vector2Int tile = Coordinates(tileIndex);
    //    return check.x >= tile.x && check.y >= tile.y && check.x < tile.x + 1 && check.y < tile.y + 1;
    //}
    //public static bool IsOverlap(Vector2 tileOne, Vector2 tileTwo)
    //{
    //    return tileOne.x < tileTwo.x + 1 && tileOne.x + 1 > tileTwo.x && tileOne.y < tileTwo.y + 1 && tileOne.y + 1 > tileTwo.y;
    //}
    //public static bool IsBlock(Vector2 pos)
    //{
    //    return world[Index(pos)] >= 1;
    //}
    //public static bool IsBlock(Vector3 pos)
    //{
    //    return world[Index(pos)] >= 1;
    //}
    //public static bool IsBlock(float x, float y)
    //{
    //    return world[Index(x, y)] >= 1;
    //}
}
