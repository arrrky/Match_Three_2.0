using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject tileSpawnerPrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<TileSpawner>().FromComponentInNewPrefab(tileSpawnerPrefab).AsSingle().NonLazy();
    }
}