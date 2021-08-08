using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
public class GridGenerator
{
    private const int randomFillPercent = 50;
    public NativeArray<int> GenerateWorld(int width, int height)
    {
        NativeArray<int> world = new NativeArray<int>(width * height, Allocator.Persistent);

        world = RandomFillMap(world, Time.realtimeSinceStartup.ToString(), width, height);

        world = SmoothMap(world, width, height);

        return world;

    }
    private NativeArray<int> RandomFillMap(NativeArray<int> world, string seed, int width, int height)
    {
        System.Random pseudoRandomGenerator = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                world[x * height + y] = FillArea(x, y, pseudoRandomGenerator);
            }
        }
        return world;
    }

    // ? : betyder om pseudorandomgeneratorn �r l�gre �n randomfillpercent s� �r det 0 annars 1
    private int FillArea(int x, int y, System.Random pseudoRandomGenerator)
    {
       
       return pseudoRandomGenerator.Next(0, 100) < randomFillPercent ? 1 : 0;
    }

    private NativeArray<int> SmoothMap(NativeArray<int> world, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y, world, width, height);

                if (neighbourWallTiles > 4)
                {
                    world[x * height + y] = 1;
                }

                else if (neighbourWallTiles < 4)
                {
                    world[x * height + y] = 0;
                }

            }
        }
        return world;
    }
    private int GetSurroundingWallCount(int gridX, int gridY, NativeArray<int> world, int width, int height)
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
                        wallCount += world[neighbourX * height + neighbourY];
                    }
                }

            }
        }

        return wallCount;
    }  
}
