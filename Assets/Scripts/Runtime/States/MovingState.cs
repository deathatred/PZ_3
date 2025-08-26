using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MovingState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        Debug.Log("Entered Moving State");

        GameEventBus.SetNewMovingPoint(
           manager.CurrentLevel.GetMoveTarget(manager.MovePointIndex)
       );
        manager.AddMovePointIndex();
    }

    public void Exit()
    {
        Debug.Log("Exited Moving State");
    }
}
