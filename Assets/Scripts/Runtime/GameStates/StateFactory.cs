using UnityEngine;
using Zenject;

public class StateFactory
{
    private readonly DiContainer _container;
   
    public StateFactory(DiContainer container)
    {
        _container = container;
    }

    public T CreateState<T>() where T : IGameState
    {
        return _container.Instantiate<T>();
    }
    public IGameState Create(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Started:
                return CreateState<StartedState>();
            case GameState.MovingToNextPoint:
                return CreateState<MovingState>();
            case GameState.SpawningTargets:
                return CreateState<SpawningTargetsState>();
            case GameState.Shooting:
                return CreateState<ShootingState>();
            case GameState.Paused:
                return CreateState<PausedState>();
            case GameState.Finished:
                return CreateState<FinishedState>();
            case GameState.Menu:
               return CreateState<MenuState>();
             
        }
        Debug.Log($"{gameState} State does not exist");
        return null;
    }
}