using UnityEngine;

public static class StateFactory
{
    public static IGameState Create(GameState state)
    {
        return state switch
        {
            GameState.Started => new StartedState(),
            GameState.MovingToNextPoint => new MovingState(),
            GameState.SpawningTargets => new SpawningTargetsState(),
            GameState.Shooting => new ShootingState(),
            GameState.Finished => new FinishedState(),
            GameState.Paused => new FinishedState() //Paused
        };

    }
}
