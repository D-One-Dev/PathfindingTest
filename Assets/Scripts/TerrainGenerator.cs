using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private TerrainTile[] terrainTiles;

    [Header("Terrain generation settings")]
    [SerializeField] private Vector2Int terrainSize;
    [SerializeField] private float terrainHeightOffset;
    [SerializeField] private Vector2 NoiseScale;

    [SerializeField] private TileBase test;
    void Start()
    {
        DataTilemap.Instance.InitializeTilemap(terrainSize);
        GenerateTerrain();

        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if ((i == 1 && j == 1) || (i == 1 && j == -1)) continue;
                terrainTilemap.SetTile(new Vector3Int(i, j, 0), test);
            }
        }
    }
    private void GenerateTerrain()
    {
        terrainTilemap.ClearAllTiles();
        float seedX = Random.Range(-10000f, 10000f);
        float seedY = Random.Range(-10000f, 10000f);

        int startPosX = -terrainSize.x/2;
        int startPosY = -terrainSize.y/2;
        for(int i = 0; i < terrainSize.x; i++)
        {
            for(int j = 0; j < terrainSize.y; j++)
            {
                TerrainTile tile = SelectRandomTile(Mathf.PerlinNoise((seedX + startPosX + i) * NoiseScale.x, (seedY + startPosY + j)) * NoiseScale.y);
                terrainTilemap.SetTile(new Vector3Int(startPosX + i, startPosY + j, 0), tile.Tile);
                DataTilemap.Instance.SetTerrainTile(new Vector2Int(i, j), tile);
            }
        }
    }

    private TerrainTile SelectRandomTile(float height)
    {
        if(height > 1f) height = 1f;
        else if (height < 0f) height = 0f;
        height = Mathf.Pow(height, terrainHeightOffset);
        List<TerrainTile> tiles = new List<TerrainTile>();
        foreach (TerrainTile tile in terrainTiles)
        {
            if (tile.minHeight <= height && tile.maxHeight >= height) tiles.Add(tile);
        }

        if (tiles.Count == 0)
        {
            Debug.LogWarningFormat("Tile corresponding to height " + height + " not found");
            return null;
        }
        else
        {
            return tiles[Random.Range(0, tiles.Count)];
        }
    }
}