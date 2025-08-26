using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class StartedState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        Debug.Log("Entered Started State");
        GameEventBus.LevelLoaded();
        _manager.NextState();
    }

    public void Exit()
    {
        Debug.Log("Exited Started State");
    }
}
