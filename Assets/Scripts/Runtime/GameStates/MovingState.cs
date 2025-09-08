using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MovingState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        ILevelService currentLevelService = manager.GetCurrentLevelService();
        GameEventBus.SetNewMovingPoint(
           currentLevelService.CurrentLevel.GetMoveTarget(currentLevelService.MovePointIndex)
       );
        currentLevelService.AddMovePointIndex();
    }

    public void Exit()
    {
    }
}
