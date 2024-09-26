using UnityEngine;
using UnityEngine.Tilemaps;

public class VisualTilemapController
{
    private static VisualTilemapController Instance;
    private Tilemap _visualTilemap;

    public static VisualTilemapController GetInstance(Tilemap visualTilemap)
    {
        if (Instance == null)
        {
            Instance = new VisualTilemapController();
            Instance._visualTilemap = visualTilemap;
        }
        return Instance;
    }

    public void ClearAllVisualTiles()
    {
        _visualTilemap.ClearAllTiles();
    }

    public void SetTile(Vector2Int position, TileBase tileVisual)
    {
        Vector3Int pos = new Vector3Int(position.x, position.y, 0);
        _visualTilemap.SetTile(pos, tileVisual);
    }
}