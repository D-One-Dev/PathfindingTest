using System;
using UnityEngine;

[Serializable]
public class GameSave
{
    public Vector2Int tilemapSize;
    public GameTile[] Tilemap;

    public GameSave(GameTile[,] tilemap, Vector2Int tilemapSize)
    {
        this.tilemapSize = tilemapSize;
        int arraySize = tilemapSize.x * tilemapSize.y;
        Tilemap = new GameTile[arraySize];

        int i = 0;
        foreach (GameTile tile in tilemap)
        {
            Tilemap[i] = tile;
            i++;
        }
    }

    public GameTile[,] GetTilemap()
    {
        GameTile[,] tilemap = new GameTile[tilemapSize.x, tilemapSize.y];
        int arraySize = tilemapSize.x * tilemapSize.y;

        for (int i = 0; i < tilemapSize.x; i++)
        {
            for (int j = 0; j < tilemapSize.y; j++)
            {
                tilemap[j, i] = Tilemap[tilemapSize.x * j + i];
            }
        }
        return tilemap;
    }
}