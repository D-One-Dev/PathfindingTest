using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTilemap
{
    private GameTile[,] _tilemap;
    private Tilemap _visualTilemap;
    private Vector2Int _tilemapSize;
    public GameTile[,] Tilemap {get => _tilemap;}
    public static GameTilemap Instance;

    public void InitializeTilemap(Tilemap tilemap, Vector2Int tilemapSize)
    {
        _visualTilemap = tilemap;
        _tilemapSize = tilemapSize;
        _tilemap = new GameTile[_tilemapSize.x, _tilemapSize.y];

        ClearAllVisualTiles();

        for(int i = 0; i < _tilemapSize.x; i++)
        {
            for(int j = 0; j < _tilemapSize.y; j++)
            {
                InitializeTile(new Vector2Int(i, j));
            }
        }
    }

    private void InitializeTile(Vector2Int position)
    {
        _tilemap[position.x, position.y] = new GameTile(position);
    }

    public static GameTilemap GetInstance()
    {
        if(Instance == null) Instance = new GameTilemap();
        return Instance;
    }

    public void ClearAllTiles()
    {
        ClearAllVisualTiles();
        for (int i = 0; i < _tilemapSize.x; i++)
        {
            for (int j = 0; j < _tilemapSize.y; j++)
            {
                _tilemap[i,j] = null;
            }
        }
    }

    public void ClearAllVisualTiles()
    {
        _visualTilemap.ClearAllTiles();
    }

    public GameTile GetTile(Vector2Int position)
    {
        return _tilemap[position.x, position.y];
    }

    public void UpdateTilemap()
    {
        for (int i = 0; i < _tilemapSize.x; i++)
        {
            for (int j = 0; j < _tilemapSize.y; j++)
            {
                Vector2Int position = ArrayToTilemap(new Vector2Int(i, j));
                _visualTilemap.SetTile(new Vector3Int(position.x, position.y, 0), GetTile(new Vector2Int(i, j)).tileVisual);
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
}