using UnityEngine;

public class GameTilemap
{
    private GameTile[,] _tilemap;
    private Vector2Int _tilemapSize;
    private VisualTilemapController _visualTilemapController;
    public GameTile[,] Tilemap {get => _tilemap;}
    public Vector2Int TilemapSize {get => _tilemapSize;}
    private static GameTilemap Instance;

    public void InitializeTilemap(VisualTilemapController visualTilemapController, Vector2Int tilemapSize)
    {
        _tilemapSize = tilemapSize;
        _tilemap = new GameTile[_tilemapSize.x, _tilemapSize.y];
        _visualTilemapController = visualTilemapController;

        _visualTilemapController.ClearAllVisualTiles();

        for(int i = 0; i < _tilemapSize.x; i++)
        {
            for(int j = 0; j < _tilemapSize.y; j++)
            {
                InitializeTile(new Vector2Int(i, j));
            }
        }

        foreach (GameTile tile in Tilemap) SetNeighbors(tile);
    }

    private void InitializeTile(Vector2Int position)
    {
        Tilemap[position.x, position.y] = new GameTile(position);
    }

    public static GameTilemap GetInstance()
    {
        if(Instance == null) Instance = new GameTilemap();
        return Instance;
    }

    public void ClearAllTiles()
    {
        _visualTilemapController.ClearAllVisualTiles();
        for (int i = 0; i < _tilemapSize.x; i++)
        {
            for (int j = 0; j < _tilemapSize.y; j++)
            {
                Tilemap[i,j] = null;
            }
        }
    }

    public GameTile GetTile(Vector2Int position)
    {
        return Tilemap[position.x, position.y];
    }

    public void UpdateTilemap()
    {
        for (int i = 0; i < _tilemapSize.x; i++)
        {
            for (int j = 0; j < _tilemapSize.y; j++)
            {
                Vector2Int position = ArrayToTilemap(new Vector2Int(i, j));
                _visualTilemapController.SetTile(position, GetTile(new Vector2Int(i, j)).tileVisual);
            }
        }
    }

    public Vector2Int ArrayToTilemap(Vector2Int position)
    {
        Vector2Int start = -_tilemapSize / 2;
        return position + start;
    }

    public Vector2Int TilemapToArray(Vector2Int position)
    {
        Vector2Int start = -_tilemapSize / 2;
        return position - start;
    }

    private void SetNeighbors(GameTile tile)
    {
        int x = tile.position.x;
        int y = tile.position.y;
        Vector2Int[] offsets;

        if ((!IsTilemapMirrored() && y % 2 == 0) || (IsTilemapMirrored() && y % 2 != 0))
        {
            offsets = new Vector2Int[] { new(1, -1), new(1, 0), new(1, 1), new(0, -1), new(-1, 0), new(0, 1) };
        }
        else
        {
            offsets = new Vector2Int[] { new(0, -1), new(1, 0), new(0, 1), new(-1, -1), new(-1, 0), new(-1, 1) };
        }

        foreach (Vector2Int offset in offsets)
        {
            if (x + offset.x >= 0 && y + offset.y >= 0 && x + offset.x < _tilemapSize.x && y + offset.y < _tilemapSize.y && Tilemap[x + offset.x, y + offset.y] != null)
            {
                tile.neighbors.Add(Tilemap[x + offset.x, y + offset.y]);
            }
        }
    }

    private bool IsTilemapMirrored()
    {
        int remainder = _tilemapSize.x % 4;

        return remainder == 0 || remainder == 1;
    }
}