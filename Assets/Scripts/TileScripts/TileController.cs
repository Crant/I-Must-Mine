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
    public void UpdateCells()
    {
        for (int x = 0; x < World.GetWidth(); x++)
        {
            for (int y = 0; y < World.GetHeight(); y++)
            {
                if (World.GetTile(World.Index(x, y)) == TileType.Dirt && y > 0 && y < World.GetHeight() && x > 0 && x < World.GetWidth() - 1)
                {
                    if (World.GetTile(World.Index(x, y - 1)) == 0)
                    {
                        World.SetTile(World.Index(x, y), 0);
                        World.SetTile(World.Index(x, y - 1), TileType.Dirt);
                        
                    }
                    else if (World.GetTile(World.Index(x + 1, y - 1)) == 0)
                    {
                        World.SetTile(World.Index(x, y), 0);
                        World.SetTile(World.Index(x + 1, y - 1), TileType.Dirt);
                    }
                    else if (World.GetTile(World.Index(x - 1, y - 1)) == 0)
                    {
                        World.SetTile(World.Index(x, y), 0);
                        World.SetTile(World.Index(x - 1, y - 1), TileType.Dirt);
                    }
                }

            }

        }
    }
    public void MineBlock(Vector2 pos, float miningSpeed)
    {
        if (!World.IsBlock(pos))
            return;

        if (!World.GetActiveTiles().ContainsKey(World.Index(pos)))
        {
            World.ActivateTileData(World.Index(pos), Tiles.Dirt());
        }
        World.GetActiveTiles().TryGetValue(World.Index(pos), out TileData tile);

        tile.miningHp -= miningSpeed;

        World.GetActiveTiles()[World.Index(pos)] = tile;
    }
    public void CreateBlock(Vector2 pos, TileType tile)
    {
        if (!World.IsBlock(pos))
        {
            World.SetTile(pos, tile);
        }
    }
}
