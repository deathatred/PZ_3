using System;
using UnityEngine;
using Zenject;

public class GameStateController : IDisposable
{
    public IGameState CurrentGameState { get; private set; }
    private StateFactory _stateFactory;
    private ILevelService _levelService;
    [Inject]
    public GameStateController(StateFactory stateFactory, ILevelService levelService)
    {
        _levelService = levelService;
         _stateFactory = stateFactory; 
    }
    public void Init()
    {
        GameEventBus.OnAllTargetsDestroyed += GameEventBusGameNextState;
        GameEventBus.OnFinishedMoving += GameEventBusGameNextState;
        GameEventBus.OnFinishedSpawning += GameEventBusGameNextState;
        GameEventBus.OnMenuClicked += GameEventBusOnMenuClicked;
        GameEventBus.OnPlayClicked += GameEventBusOnPlayClicked;
        GameEventBus.OnLevelFinishedLoading += GameEventBusOnLevelFinishedLoading;
    }
    private void GameEventBusOnMenuClicked()
    {
        Debug.Log("sub");
        ChangeCurrentGameState(GameState.Menu);
    }
    private void GameEventBusOnPlayClicked()
    {
        ChangeCurrentGameState(GameState.Started);
    }
    private void GameEventBusGameNextState()
    {
        NextState();
    }
    private void GameEventBusOnLevelFinishedLoading()
    {
        ChangeCurrentGameState(GameState.Started);
    }
    public void ChangeCurrentGameState(GameState newState)
    {
        CurrentGameState?.Exit();
        CurrentGameState = _stateFactory.Create(newState);
        CurrentGameState.Enter();
    }
    public void NextState()
    {
        _levelService.ProgressLevel();
        ChangeCurrentGameState(_levelService.GetCurrentLevelFlowState());
    }
    public void Dispose()
    {
        GameEventBus.OnMenuClicked -= GameEventBusOnMenuClicked;
        GameEventBus.OnPlayClicked -= GameEventBusOnPlayClicked;
        GameEventBus.OnFinishedSpawning -= GameEventBusGameNextState;
        GameEventBus.OnLevelFinishedLoading -= GameEventBusOnLevelFinishedLoading;
    }
}
