using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class TileSelector : MonoBehaviour
{
    [Inject(Id = "TerrainTilemap")]
    private readonly Tilemap terrainTilemap;
    [Inject(Id = "PathTile")]
    private readonly TileBase pathTile;

    private GameTilemap _gameTilemap;
    private VisualTilemapController _visualTilemapController;

    private Controls _controls;

    [Inject]
    public void Construct(Controls controls, VisualTilemapController visualTilemapController, GameTilemap gameTilemap)
    {
        _controls = controls;
        _visualTilemapController = visualTilemapController;
        _gameTilemap = gameTilemap;
    }

    private void Awake()
    {
        _controls.Gameplay.MainClick.performed += ctx => OnMainClick();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
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

    private void OnMainClick()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        Vector3Int targetPos = GetTile();
        Vector2Int target = _gameTilemap.TilemapToArray(new Vector2Int(targetPos.x, targetPos.y));

        if (target.x >= 0 && target.y >= 0 && target.x < _gameTilemap.TilemapSize.x && target.y < _gameTilemap.TilemapSize.y)
        {
            List<GameTile> path = Pathfinding.FindPath(_gameTilemap.Tilemap[0, 0], _gameTilemap.Tilemap[target.x, target.y]);
            if (path == null)
            {
                Debug.LogWarning("Couldn't reach target tile");
                watch.Stop();
                Debug.LogWarningFormat("Total time: " + watch.ElapsedMilliseconds + " msec.");
            }
            else
            {
                watch.Stop();
                Debug.LogWarningFormat("Total time: " + watch.ElapsedMilliseconds + " msec.");
                VisualizePath(path);
            }
        }
    }
}