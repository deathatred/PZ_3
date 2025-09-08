using System;
using UnityEngine;

public class GameStateController : IDisposable
{
    public IGameState CurrentGameState { get; private set; }
    private GameManager _gameManager;
    public GameStateController(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    public void Init()
    {
        Debug.Log("sub");
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
        CurrentGameState = StateFactory.Create(newState);
        CurrentGameState.Enter(_gameManager);
    }
    public void NextState()
    {
        ILevelService levelService = _gameManager.GetCurrentLevelService();
        levelService.ProgressLevel();
        ChangeCurrentGameState(levelService.GetCurrentLevelFlowState());
    }
    public void Dispose()
    {
        GameEventBus.OnMenuClicked -= GameEventBusOnMenuClicked;
        GameEventBus.OnPlayClicked -= GameEventBusOnPlayClicked;
        GameEventBus.OnFinishedSpawning -= GameEventBusGameNextState;
        GameEventBus.OnLevelFinishedLoading -= GameEventBusOnLevelFinishedLoading;
    }
}
