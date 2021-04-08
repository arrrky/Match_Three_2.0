using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject tileSpawnerPrefab;
    [SerializeField] private GameObject tileFactoryPrefab;
    [SerializeField] private GameObject playingFieldManagerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<TileSpawner>().FromComponentInNewPrefab(tileSpawnerPrefab).AsSingle().NonLazy();
        Container.Bind<TileFactory>().FromComponentInNewPrefab(tileFactoryPrefab).AsSingle().NonLazy();
        Container.Bind<PlayingFieldManager>().FromComponentInNewPrefab(playingFieldManagerPrefab).AsSingle().NonLazy();
    }
}