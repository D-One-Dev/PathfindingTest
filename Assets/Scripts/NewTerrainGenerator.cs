using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class NewTerrainGenerator : MonoBehaviour
{
    private GameTilemap _gameTilemap;
    private VisualTilemapController _visualTilemapController;

    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private TerrainTile[] terrainTiles;

    [Header("Terrain generation settings")]
    [SerializeField] private Vector2Int terrainSize;
    [SerializeField] private float terrainHeightOffset;
    [SerializeField] private Vector2 noiseScale;

    [Inject]
    public void Construct(VisualTilemapController visualTilemapController, GameTilemap gameTilemap)
    {
        _visualTilemapController = visualTilemapController;
        _gameTilemap = gameTilemap;
    }

    private void Start()
    {
        GameSave save = JsonSerialization.ReadSave();
        if (save == null) GenerateTerrain();
        else
        {
            _gameTilemap.InitializeTilemap(terrainSize);
            _gameTilemap.SetTilemap(save.GetTilemap());
        }
    }

    public void GenerateTerrain()
    {
        _gameTilemap.InitializeTilemap(terrainSize);

        float seedX = Random.Range(-10000f, 10000f);
        float seedY = Random.Range(-10000f, 10000f);

        int startPosX = -terrainSize.x / 2;
        int startPosY = -terrainSize.y / 2;
        for (int i = 0; i < terrainSize.x; i++)
        {
            for (int j = 0; j < terrainSize.y; j++)
            {
                TerrainTile terrainTile = SelectRandomTile(GetHeight(new Vector2(seedX + startPosX + i, seedY + startPosY + j)));

                GameTile tile = _gameTilemap.GetTile(new Vector2Int(i, j));
                tile.surfaceType = terrainTile.surfaceType;
                tile.pathWeight = terrainTile.pathWeight;
                tile.tileVisual = terrainTile.Tile;
            }
        }

        _gameTilemap.UpdateTilemap();

        JsonSerialization.WriteSave(new GameSave(_gameTilemap.Tilemap, terrainSize));
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

    private float GetHeight(Vector2 pos)
    {
        float xCoord = pos.x * noiseScale.x;
        float yCoord = pos.y * noiseScale.y;

        float height = Mathf.PerlinNoise(xCoord, yCoord);
        if (height < 0f) return 0f;
        else if (height > 1f) return 1f;
        return height;
    }

    private void OnValidate()
    {
        //_gameTilemap = GameTilemap.GetInstance();
        //_visualTilemapController = VisualTilemapController.GetInstance(terrainTilemap);
        //GenerateTerrain();
    }
}