using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum SurfaceType
{
    None = 0,
    Ground = 1,
    Water = 2
}
public class GameTile
{
    public Vector2Int position;
    public List<GameTile> neighbors;
    public SurfaceType surfaceType;
    public int pathWeight;
    public Building building;
    public Resource resource;
    public Fraction fraction;
    public Unit unit;
    public TileBase tileVisual;

    public GameTile(Vector2Int position)
    {
        this.position = position;
        neighbors = new List<GameTile>();
    }
}