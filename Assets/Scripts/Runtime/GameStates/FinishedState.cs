using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class FinishedState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        Debug.Log("Entered Finished State");
        GameEventBus.LevelFinished();
    }

    public void Exit()
    {
        Debug.Log("Exited Finished State");
    }
}
