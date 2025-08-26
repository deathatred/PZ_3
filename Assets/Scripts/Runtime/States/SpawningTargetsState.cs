using UnityEngine;

public class SpawningTargetsState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        Debug.Log("Entered Spawning Targets State");
        GameEventBus.SpawnTargets(_manager.TargetSpawnPointIndex,_manager.IsTargetSpawnPointLast());
        _manager.AddTargetSpawnPointIndex();
        
    }

    public void Exit()
    {
        Debug.Log("Exited Spawning Targets State");
    }
}
