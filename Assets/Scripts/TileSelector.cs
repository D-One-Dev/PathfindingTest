using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private TileBase pathTile;
    private DataTilemap dataTilemap;

    private void Start()
    {
        dataTilemap = DataTilemap.GetInstance();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3Int targetPos = GetTile();
            Vector2Int target = dataTilemap.TilemapToDataTilemap(new Vector2Int(targetPos.x, targetPos.y));
            
            if(target.x >= 0 && target.y >= 0 && target.x < dataTilemap.tilemapSize.x && target.y < dataTilemap.tilemapSize.y)
            {
                List<DataTile> path = Pathfinding.FindPath(dataTilemap.dataTilemap[0,0], dataTilemap.dataTilemap[target.x, target.y]);
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

    private void VisualizePath(List<DataTile> path)
    {
        dataTilemap.RefreshTilemap(terrainTilemap);

        foreach (DataTile tile in path)
        {
            Vector2Int pos = dataTilemap.DataTilemapToTilemap(new Vector2Int(tile.position.x, tile.position.y));
            terrainTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), pathTile);
        }
    }
}