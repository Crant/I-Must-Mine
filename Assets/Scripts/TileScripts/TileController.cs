using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController
{
    private List<int> keys = new List<int>();
    private TileData tile;

    private const float resetTimer = 3f;
    public void CheckActiveTiles()
    {
        keys.AddRange(World.GetActiveKeys());

        for (int i = 0; i < keys.Count; i++)
        {
            //Debug.Log(keys[i] + " key at index " + i);
            tile = World.GetTileData(keys[i]);
            tile.timeAlive += Time.deltaTime;
            World.SetTileData(keys[i], tile);
            if (World.GetTileData(keys[i]).timeAlive >= resetTimer)
            {
                World.DeactivateTileData(keys[i]);
            }
            else if (World.GetTileData(keys[i]).miningHp <= 0 || World.GetTileData(keys[i]).hitPoints <= 0)
            {
                World.DeactivateTileData(keys[i]);
                World.SetTile(keys[i], (int)TileType.Empty);
            }
        }
        keys.Clear();
    }
}
