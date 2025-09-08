using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class StartedState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        ILevelService levelService = manager.GetCurrentLevelService();
        GameEventBus.LevelLoaded(levelService.CurrentLevel);
        _manager.GetGameStateController().NextState();
    }

    public void Exit()
    {
    }
}
