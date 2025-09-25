using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class LevelService : ILevelService
{
    private List<Level> _levelsList;
    private int _currentLevelIndex = 0;
    private Level _currentLevel;
    private int _currentLevelProgress = 0;
    private Transform _player;
    private CancellationTokenSource _cts;
    private LevelFlowSO _currentLevelFlowSO;
    private PlayerSpawnService _playerSpawnService;
    //private GameManager _gameManager;

    public int MovePointIndex { get; private set; } = 0;
    public int TargetSpawnPointIndex { get; private set; } = 0;
    public int CurrentLevelIndex
    {
        get { return _currentLevelIndex + 1; }
        private set { _currentLevelIndex = value; }
    }
    public Level CurrentLevel {get => _currentLevel; }

    public LevelService(List<Level> levels, PlayerSpawnService spawn, Transform player)
    {
        _player = player;
        _playerSpawnService = spawn;
        _levelsList = levels;
    }
    public void Init()
    {
        GameEventBus.OnMenuClicked += GameEventBusOnMenuClicked;
        GameEventBus.OnShootingEnded += GameEventBusOnShootingEnded;
        GameEventBus.OnNextClicked += GameEventBusOnNextClicked;
    }
    private void GameEventBusOnShootingEnded()
    {
        int listOffset = 1;
        if ((TargetSpawnPointIndex - listOffset) >= 0)
        {
            CurrentLevel.DestroyObstacles(TargetSpawnPointIndex - listOffset);
        }
    }
    private void GameEventBusOnMenuClicked()
    {
        ResetLevel();
    }
    private void GameEventBusOnNextClicked()
    {
        StartNextLevelWithDelayAsync().Forget();
    }
    public async UniTask ChangeLevelAsync(int level)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        int indexNormalized = level - 1;
        if (indexNormalized == _currentLevelIndex)
        {
            ResetLevel();
            return;
        }
        _currentLevel.gameObject.SetActive(false);
        _currentLevelIndex = indexNormalized;
        await LoadLevelAsync(_currentLevelIndex, _cts.Token);
    }

    public void SetCurrentLevel()
    {
        _currentLevel = _levelsList[_currentLevelIndex];
        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();
        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();
    }

    public async UniTask LoadLevelAsync(int index, CancellationToken token)
    {
        MovePointIndex = 0;
        TargetSpawnPointIndex = 0;
        _currentLevelProgress = 0;

        _currentLevel = _levelsList[index];
      
        _currentLevel.gameObject.SetActive(true);

        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();

        _playerSpawnService.ResetPlayerTransform();
        SetCurrentLevel();
        ResetLevel();
        await UniTask.Delay(2000).AttachExternalCancellation(token);
        GameEventBus.LevelFinishedLoading();
    }

    public async UniTask StartNextLevelWithDelayAsync()
    {
        CreateNewToken();
        var token = _cts.Token;

        if (CurrentLevelIndex < _levelsList.Count)
        {
            GameEventBus.LoadNextLevel();
            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: token);
            NextLevelAsync(token).Forget();
        }
    }

    public async UniTask NextLevelAsync(CancellationToken token)
    {
        _currentLevel.gameObject.SetActive(false);
        _currentLevelIndex++;
        await LoadLevelAsync(_currentLevelIndex, token);
    }

    public void ResetLevel()
    { 
        MovePointIndex = 0;
        TargetSpawnPointIndex = 0;
        _currentLevelProgress = 0;

        _currentLevel = _levelsList[_currentLevelIndex];
        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();
        _playerSpawnService.ResetPlayerTransform();
        _currentLevel.ResetLevel();
    }

    public List<Level> GetLevelsList()
    {
        return _levelsList;
    }

    public void ProgressLevel()
    {
        _currentLevelProgress++;
    }
    private void CreateNewToken()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
    }
    public GameState GetCurrentLevelFlowState()
    {
        return _currentLevelFlowSO.GameStateList[_currentLevelProgress];
    }
    public void AddMovePointIndex()
    {
        MovePointIndex++;
    }
    public void AddTargetSpawnPointIndex()
    {
        TargetSpawnPointIndex++;
    }
    public void Dispose()
    {
        _cts?.Cancel();
        GameEventBus.OnMenuClicked -= GameEventBusOnMenuClicked;
        GameEventBus.OnShootingEnded -= GameEventBusOnShootingEnded;
        GameEventBus.OnNextClicked -= GameEventBusOnNextClicked;
    }
    public bool IsTargetSpawnPointLast()
    {
        if (TargetSpawnPointIndex == CurrentLevel.GetTargetSpawnPointsCount() - 1)
        {
            return true;
        }
        return false;
    }
}
