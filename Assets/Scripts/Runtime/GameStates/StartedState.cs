using System.Diagnostics;
using UnityEngine;
using Zenject;

public class StartedState : IGameState
{
    private GameStateController _stateController;
    private ILevelService _levelService;
    [Inject]
    public StartedState(GameStateController controller, ILevelService levelService)
    {
        _stateController = controller;
        _levelService = levelService;
    }
    public void Enter()
    {
        GameEventBus.LevelLoaded(_levelService.CurrentLevel);
        _stateController.NextState();
    }

    public void Exit()
    {
    }
}
