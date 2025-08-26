using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PausedState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        Debug.Log("Entered Paused State");

    }

    public void Exit()
    {
        Debug.Log("Exited Paused State");
    }
}
