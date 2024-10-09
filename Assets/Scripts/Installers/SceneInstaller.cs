using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Tilemap terrainTilemap;
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private Rigidbody2D cameraRigidbody;
    [SerializeField] private TileBase pathTile;
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
            .AsSingle();
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
            .AsSingle();
        this.Container
            .Bind<Rigidbody2D>()
            .WithId("Camera")
            .FromInstance(cameraRigidbody)
            .AsSingle();
        this.Container
            .Bind<TileBase>()
            .WithId("PathTile")
            .FromInstance(pathTile)
            .AsSingle();
    }
}