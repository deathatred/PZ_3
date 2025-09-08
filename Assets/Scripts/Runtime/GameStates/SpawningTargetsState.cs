using UnityEngine;

public class SpawningTargetsState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        ILevelService leverServise = manager.GetCurrentLevelService();

        GameEventBus.SpawnTargets(leverServise.TargetSpawnPointIndex, _manager.GetCurrentLevelService().IsTargetSpawnPointLast());
        leverServise.AddTargetSpawnPointIndex();
     
    }

    public void Exit()
    {
    }
}
