using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PausedState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;

    }

    public void Exit()
    {
    }
}
