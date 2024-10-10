using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private Rigidbody2D cameraRigidbody;
    [SerializeField] private TileBase pathTile;
    [SerializeField] private TMP_Text currentFPSText;
    [SerializeField] private TMP_Text minFPSText;
    public override void InstallBindings()
    {
        this.Container
            .Bind<Controls>()
            .To<Controls>()
            .FromNew()
            .AsTransient();
        this.Container
            .Bind<Tilemap>()
            .WithId("TerrainTilemap")
            .FromInstance(terrainTilemap)
            .AsCached();
        this.Container
            .Bind<VisualTilemapController>()
            .To<VisualTilemapController>()
            .AsSingle();
        this.Container
            .Bind<GameTilemap>()
            .To<GameTilemap>()
            .AsSingle();
        this.Container
            .Bind<GameObject>()
            .WithId("Camera")
            .FromInstance(cameraGameObject)
            .AsCached();
        this.Container
            .Bind<Rigidbody2D>()
            .WithId("Camera")
            .FromInstance(cameraRigidbody)
            .AsCached();
        this.Container
            .Bind<TileBase>()
            .WithId("PathTile")
            .FromInstance(pathTile)
            .AsCached();
        this.Container
            .Bind<TMP_Text>()
            .WithId("CurrentFPSText")
            .FromInstance(currentFPSText)
            .AsCached();
        this.Container
            .Bind<TMP_Text>()
            .WithId("MinFPSText")
            .FromInstance(minFPSText)
            .AsCached();
    }
}