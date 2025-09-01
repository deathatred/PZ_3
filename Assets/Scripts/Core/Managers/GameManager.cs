using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Level> _levelsList;
    public static GameManager Instance { get; private set; }
    public int MovePointIndex { get; private set; } = 0;
    public int TargetSpawnPointIndex { get; private set; } = 0;
    public int CurrentLevelIndex
    {
        get { return _currentLevelIndex + 1; }
        private set { _currentLevelIndex = value; }
    }
    private int _currentLevelIndex = 0;
    private Level _currentLevel;
    public Level CurrentLevel => _currentLevel;

    public IGameState CurrentGameState { get; private set; }
    private LevelFlowSO _currentLevelFlowSO;
    private int _currentLevelProgress = 0;
    private Vector3 _playerDefaultSpawnPoint = new Vector3(0f, 0.6f, 0f);
    private Transform _player;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        InitSingleton();
        GetPlayer();
        SetCurrentLevel(_player);
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    public void ChangeCurrentGameState(GameState newState)
    {
        CurrentGameState?.Exit();
        CurrentGameState = StateFactory.Create(newState);
        CurrentGameState.Enter(this);
    }
    public void NextState()
    {
        _currentLevelProgress++;
        ChangeCurrentGameState(_currentLevelFlowSO.GameStateList[_currentLevelProgress]);
    }
    public void AddMovePointIndex()
    {
        MovePointIndex++;
    }
    public void AddTargetSpawnPointIndex()
    {
        TargetSpawnPointIndex++;
    }
    public bool IsTargetSpawnPointLast()
    {
        if (TargetSpawnPointIndex == _currentLevel.GetTargetSpawnPointsCount() - 1)
        {
            return true;
        }
        return false;
    }
    private void InitSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void GetPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void SubscribeToEvents()
    {
        GameEventBus.OnAllTargetsDestroyed += GameEventBusGameNextState;
        GameEventBus.OnFinishedMoving += GameEventBusGameNextState;
        GameEventBus.OnFinishedSpawning += GameEventBusGameNextState;
        GameEventBus.OnShootingEnded += GameEventBusOnShootingEnded;
        GameEventBus.OnPlayClicked += GameEventBusOnPlayClicked;
        GameEventBus.OnMenuClicked += GameEventBusOnMenuClicked;
        GameEventBus.OnNextClicked += GameEventBusOnNextClicked;
    }
    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnAllTargetsDestroyed -= GameEventBusGameNextState;
        GameEventBus.OnFinishedMoving -= GameEventBusGameNextState;
        GameEventBus.OnFinishedSpawning -= GameEventBusGameNextState;
        GameEventBus.OnShootingEnded -= GameEventBusOnShootingEnded;
        GameEventBus.OnPlayClicked -= GameEventBusOnPlayClicked;
        GameEventBus.OnMenuClicked -= GameEventBusOnMenuClicked;
        GameEventBus.OnNextClicked -= GameEventBusOnNextClicked;
    }
    private void GameEventBusOnMenuClicked()
    {
        ResetLevel();
    }
    private void GameEventBusOnPlayClicked()
    {
        ChangeCurrentGameState(GameState.Started);
    }

    private void GameEventBusOnShootingEnded()
    {
        int listOffset = 1;
        if ((TargetSpawnPointIndex - listOffset) >= 0)
        {
            _currentLevel.DestroyObstacles(TargetSpawnPointIndex - listOffset);
        }
    }
    private void GameEventBusGameNextState()
    {
        NextState();
    }

    private void GameEventBusOnNextClicked()
    {
        StartNextLevelWithDelayAsync().Forget();
    }
    private async UniTask StartNextLevelWithDelayAsync()
    {
        print(CurrentLevelIndex + " " + _levelsList.Count);
        if (CurrentLevelIndex < _levelsList.Count)
        {
            GameEventBus.LoadNextLevel();
            await UniTask.Delay(3000);
            NextLevelAsync().Forget();
        }
    }
    public List<Level> GetLevelsList()
    {
        return _levelsList;
    }

    public async UniTask ChangeLevel(int index)
    {
        int indexNormalized = index - 1;
        if (indexNormalized == _currentLevelIndex)
        {
            print("reseting");
            ResetLevel();
            return;
        }
        _currentLevel.gameObject.SetActive(false);
        _currentLevelIndex = indexNormalized;
        await LoadLevel(_currentLevelIndex);
    }

    public async UniTask NextLevelAsync()
    {
        _currentLevel.gameObject.SetActive(false);
        _currentLevelIndex++;
        await LoadLevel(_currentLevelIndex);
    }
    private async UniTask LoadLevel(int index)
    {
        MovePointIndex = 0;
        TargetSpawnPointIndex = 0;
        _currentLevelProgress = 0;

        _currentLevel = _levelsList[index];
        _currentLevel.gameObject.SetActive(true);

        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();

        ResetPlayerTransform();
        SetCurrentLevel(_player);

        await UniTask.Delay(2000);
        ChangeCurrentGameState(GameState.Started);
    }

    private void ResetPlayerTransform()
    {
        _player.position = _playerDefaultSpawnPoint;
        _player.rotation = Quaternion.Euler(0f, 90f, 0f);
    }

    private void ResetLevel()
    {
        MovePointIndex = 0;
        TargetSpawnPointIndex = 0;
        _currentLevelProgress = 0;
        _currentLevel = _levelsList[_currentLevelIndex];
        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();
        _player.position = _playerDefaultSpawnPoint;
        _player.rotation = Quaternion.Euler(0f, 90f, 0f);
        _currentLevel.ResetLevel();
        ChangeCurrentGameState(GameState.Menu);
    }
    private void SetCurrentLevel(Transform player)
    {
        _currentLevel = _levelsList[_currentLevelIndex];
        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();
        _currentLevel.SetPlayer(player);
    }
    public int GetRemainingBullets()
    {
        var playerShooting = _player.GetComponent<PlayerShooting>();
        return playerShooting.GetBullets();
    }
}
