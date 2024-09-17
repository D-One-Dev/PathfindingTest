using UnityEngine;
using UnityEngine.Tilemaps;

public class DataTilemap
{
    public DataTile[,] dataTilemap;
    public Vector2Int tilemapSize;
    private static DataTilemap Instance;
    public void InitializeTilemap(Vector2Int tilemapSize)
    {
        this.tilemapSize = tilemapSize;

        dataTilemap = new DataTile[tilemapSize.x, tilemapSize.y];
        for (int i = 0; i < tilemapSize.x; i++)
        {
            for (int j = 0; j < tilemapSize.y; j++)
            {
                InitializeTile(new Vector2Int(i, j));
            }
        }

        foreach(DataTile tile in dataTilemap) SetNeighbors(tile);
    }

    public void SetTerrainTile(Vector2Int position, TerrainTile tile)
    {
        dataTilemap[position.x, position.y].terrainTile = tile;
    }

    private void InitializeTile(Vector2Int position)
    {
        dataTilemap[position.x, position.y] = new DataTile(position);
    }

    private void SetNeighbors(DataTile tile)
    {
        int x = tile.position.x;
        int y = tile.position.y;
        Vector2Int[] offsets;

        if(!IsTilemapMirrored())
        {
            if (y % 2 == 0) offsets = new Vector2Int[] {new (1, -1), new(1, 0), new(1, 1), new(0, -1), new(-1, 0), new(0, 1)};
            else offsets = new Vector2Int[] { new(0, -1), new(1, 0), new(0, 1), new(-1, -1), new(-1, 0), new(-1, 1) };

            foreach (Vector2Int offset in offsets)
            {
                if (x + offset.x >= 0 && y + offset.y >= 0 && x + offset.x < tilemapSize.x && y + offset.y < tilemapSize.y && dataTilemap[x + offset.x, y + offset.y] != null)
                    tile.neighbors.Add(dataTilemap[x + offset.x, y + offset.y]);
            }
        }
        else
        {
            if (y % 2 != 0) offsets = new Vector2Int[] { new(1, -1), new(1, 0), new(1, 1), new(0, -1), new(-1, 0), new(0, 1) };
            else offsets = new Vector2Int[] { new(0, -1), new(1, 0), new(0, 1), new(-1, -1), new(-1, 0), new(-1, 1) };

            foreach (Vector2Int offset in offsets)
            {
                if (x + offset.x >= 0 && y + offset.y >= 0 && x + offset.x < tilemapSize.x && y + offset.y < tilemapSize.y && dataTilemap[x + offset.x, y + offset.y] != null)
                    tile.neighbors.Add(dataTilemap[x + offset.x, y + offset.y]);
            }
        }
    }

    public void RefreshTilemap(Tilemap terrainTilemap)
    {
        int startPosX = -tilemapSize.x / 2;
        int startPosY = -tilemapSize.y / 2;

        for (int i = 0; i < tilemapSize.x; i++)
        {
            for (int j = 0; j < tilemapSize.y; j++)
            {
                terrainTilemap.SetTile(new Vector3Int(startPosX + i, startPosY + j, 0), dataTilemap[i, j].terrainTile.Tile);
            }
        }
    }

    public Vector2Int TilemapToDataTilemap(Vector2Int tilemapPos)
    {
        int startPosX = -tilemapSize.x / 2;
        int startPosY = -tilemapSize.y / 2;
        return tilemapPos - new Vector2Int(startPosX, startPosY);
    }

    public Vector2Int DataTilemapToTilemap(Vector2Int dataTilemapPos)
    {
        int startPosX = -tilemapSize.x / 2;
        int startPosY = -tilemapSize.y / 2;
        return dataTilemapPos + new Vector2Int(startPosX, startPosY);
    }

    private bool IsTilemapMirrored()
    {
        int remainder = tilemapSize.x % 4;

        return remainder == 0 || remainder == 1;
    }

    public static DataTilemap GetInstance()
    {
        if (Instance == null)
        {
            Instance = new DataTilemap();
        }
        return Instance;
    }
}
