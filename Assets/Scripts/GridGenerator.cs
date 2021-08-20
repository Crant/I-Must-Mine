using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
public class GridGenerator
{
    private const int randomFillPercent = 50;
    public NativeArray<TileType> GenerateWorld(int width, int height)
    {
        NativeArray<TileType> world = new NativeArray<TileType>(width * height, Allocator.Persistent);

        world = RandomFillMap(world, Time.realtimeSinceStartup.ToString(), width, height);

        world = SmoothMap(world, width, height);

        return world;

    }
    private NativeArray<TileType> RandomFillMap(NativeArray<TileType> world, string seed, int width, int height)
    {
        System.Random pseudoRandomGenerator = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                world[x * height + y] = (TileType)FillArea(x, y, pseudoRandomGenerator);
            }
        }
        return world;
    }

    // ? : betyder om pseudorandomgeneratorn är lägre än randomfillpercent så är det 0 annars 1
    private int FillArea(int x, int y, System.Random pseudoRandomGenerator)
    {
       
       return pseudoRandomGenerator.Next(0, 100) < randomFillPercent ? 1 : 0;
    }

    private NativeArray<TileType> SmoothMap(NativeArray<TileType> world, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y, world, width, height);

                if (neighbourWallTiles > 4)
                {
                    world[x * height + y] = TileType.Dirt;
                }

                else if (neighbourWallTiles < 4)
                {
                    world[x * height + y] = 0;
                }

            }
        }
        return world;
    }
    private int GetSurroundingWallCount(int gridX, int gridY, NativeArray<TileType> world, int width, int height)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += (int)world[neighbourX * height + neighbourY];
                    }
                }

            }
        }

        return wallCount;
    }  
}
