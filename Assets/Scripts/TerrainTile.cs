using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Terrain Tile")]
public class TerrainTile : ScriptableObject
{
    public TileBase Tile;
    public float minHeight, maxHeight;
    public int pathWeight;
    public bool walkable;
    public SurfaceType surfaceType;
}