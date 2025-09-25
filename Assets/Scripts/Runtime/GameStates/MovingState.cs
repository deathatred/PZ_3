using System.Diagnostics;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

public class MovingState : IGameState
{
    private PlayerShooting _playerShooting;
    private ILevelService _levelService;
    [Inject]
    public MovingState(PlayerShooting playerShooting, ILevelService levelService)
    {
        _playerShooting = playerShooting;
        _levelService = levelService;
    }
    public void Enter()
    {

        GameEventBus.SetNewMovingPoint(
           _levelService.CurrentLevel.GetMoveTarget(_levelService.MovePointIndex)
       );
        _levelService.AddMovePointIndex();
    }

    public void Exit()
    {
    }
}
