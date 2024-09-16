using System.Collections.Generic;
using UnityEngine;

public class DataTile
{
    public Vector2Int position;

    public DataTile parent;

    public List<DataTile> neighbors;

    public TerrainTile terrainTile;

    public DataTile(Vector2Int position)
    {
        this.position = position;
        neighbors = new List<DataTile>();
    }
}