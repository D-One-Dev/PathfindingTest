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

    private DataTilemap dataTilemap;
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
                TerrainTile tile = SelectRandomTile(GetHeight(new Vector2(seedX + startPosX + i, seedY + startPosY + j)));
                terrainTilemap.SetTile(new Vector3Int(startPosX + i, startPosY + j, 0), tile.Tile);
                dataTilemap.SetTerrainTile(new Vector2Int(i, j), tile);
            }
        }
    }

    private TerrainTile SelectRandomTile(float height)
    {
        height = Mathf.Pow(height, -terrainHeightOffset);
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

    private void InitializeTerrain()
    {
        dataTilemap = DataTilemap.GetInstance();

        dataTilemap.InitializeTilemap(terrainSize);
        GenerateTerrain();
    }

    private float GetHeight(Vector2 pos)
    {
        float xCoord = pos.x * NoiseScale.x;
        float yCoord = pos.y * NoiseScale.y;

        float height = Mathf.PerlinNoise(xCoord, yCoord);
        if (height < 0f) return 0f;
        else if (height > 1f) return 1f;
        return height;
    }

    private void OnValidate()
    {
        InitializeTerrain();
    }
}