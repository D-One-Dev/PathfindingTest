using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class VisualTilemapController
{
    [Inject(Id = "TerrainTilemap")]
    private readonly Tilemap _visualTilemap;

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