using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private TileBase pathTile;
    private GameTilemap _gameTilemap;
    private VisualTilemapController _visualTilemapController;

    private void Start()
    {
        _gameTilemap = GameTilemap.GetInstance();
        _visualTilemapController = VisualTilemapController.GetInstance(terrainTilemap);
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3Int targetPos = GetTile();
            Vector2Int target = _gameTilemap.TilemapToArray(new Vector2Int(targetPos.x, targetPos.y));
            
            if(target.x >= 0 && target.y >= 0 && target.x < _gameTilemap.TilemapSize.x && target.y < _gameTilemap.TilemapSize.y)
            {
                List<GameTile> path = Pathfinding.FindPath(_gameTilemap.Tilemap[0,0], _gameTilemap.Tilemap[target.x, target.y]);
                if (path == null) Debug.LogWarning("Couldn't reach target tile");
                else VisualizePath(path);

                Debug.Log(target);
            }
        }
    }

    private Vector3Int GetTile()
    {
        return terrainTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void VisualizePath(List<GameTile> path)
    {
        _visualTilemapController.ClearAllVisualTiles();
        _gameTilemap.UpdateTilemap();

        foreach (GameTile tile in path)
        {
            Vector2Int pos = _gameTilemap.ArrayToTilemap(new Vector2Int(tile.position.x, tile.position.y));
            _visualTilemapController.SetTile(pos, pathTile);
        }

    }
}