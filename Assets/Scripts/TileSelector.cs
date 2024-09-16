using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private TileBase pathTile;
    [SerializeField] private TileBase test;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3Int targetPos = GetTile();
            Vector2Int target = DataTilemap.Instance.TilemapToDataTilemap(new Vector2Int(targetPos.x, targetPos.y));
            
            List<DataTile> path = Pathfinding.FindPath(DataTilemap.Instance.dataTilemap[0,0], DataTilemap.Instance.dataTilemap[target.x, target.y]);
            if (path == null) Debug.LogWarning(34234423);
            else VisualizePath(path);
            

            Debug.Log(target);
        }
    }

    private Vector3Int GetTile()
    {
        return terrainTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void VisualizePath(List<DataTile> path)
    {
        DataTilemap.Instance.RefreshTilemap(terrainTilemap);

        foreach (DataTile tile in Pathfinding.Test())
        {
            Vector2Int pos = DataTilemap.Instance.DataTilemapToTilemap(new Vector2Int(tile.position.x, tile.position.y));
            terrainTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), test);
        }

        foreach (DataTile tile in path)
        {
            Vector2Int pos = DataTilemap.Instance.DataTilemapToTilemap(new Vector2Int(tile.position.x, tile.position.y));
            terrainTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), pathTile);
        }

    }
}
