using UnityEngine;

public class GameStateController
{
    public IGameState CurrentGameState { get; private set; }
    private GameManager _gameManager;
    public GameStateController(GameManager gameManager)
    {
        _gameManager = gameManager;
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
}
