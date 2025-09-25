using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameContext : MonoInstaller
{
    [SerializeField] private List<Level> _levelsList;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerShooting _playerShooting;

    public override void InstallBindings()
    {
        Container.Bind<PlayerController>().FromInstance(_playerController).AsSingle().NonLazy();
        Container.Bind<PlayerShooting>().FromInstance(_playerShooting).AsSingle().NonLazy();
        Container.Bind<ILevelService>().To<LevelService>().AsSingle().WithArguments(_levelsList, _playerController.transform);
        Container.Bind<GameStateController>().AsSingle();
        Container.Bind<PlayerSpawnService>().AsSingle().WithArguments(_playerController.transform);
        Container.Bind<StateFactory>().AsSingle();
    }
}
