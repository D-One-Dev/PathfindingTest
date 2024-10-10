using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
public class ProjectInstaller : ScriptableObjectInstaller
{
    [SerializeField] private string savePath;
    public override void InstallBindings()
    {
        this.Container
            .Bind<string>()
            .WithId("SavePath")
            .FromInstance(savePath)
            .AsSingle();
        this.Container
            .Bind<JsonSerialization>()
            .AsSingle();
    }
}