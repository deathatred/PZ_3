using UnityEngine;
using Zenject;

public class SpawningTargetsState : IGameState
{
    private ILevelService _levelService;
    [Inject]
    public SpawningTargetsState(ILevelService levelService)
    {
        _levelService = levelService;
    }
    public void Enter()
    {
        GameEventBus.SpawnTargets(_levelService.TargetSpawnPointIndex, _levelService.IsTargetSpawnPointLast());
        _levelService.AddTargetSpawnPointIndex();
    }

    public void Exit()
    {
    }
}
