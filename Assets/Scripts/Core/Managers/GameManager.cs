using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Level> _levelsList;
    public static GameManager Instance { get; private set; }
    public int MovePointIndex { get; private set; } = 0;
    public int TargetSpawnPointIndex { get; private set; } = 0;
    public int CurrentLevelIndex { get; private set; } = 0;
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
        _currentLevel = _levelsList[CurrentLevelIndex];
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Start()
    {
        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();
        _currentLevel.SetPlayer(_player);
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
        if (TargetSpawnPointIndex == _currentLevel.GetTargetSpawnPointsCount()-1)
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
    private void SubscribeToEvents()
    {
        GameEventBus.OnAllTargetsDestroyed += GameEventBusGameNextState;
        GameEventBus.OnFinishedMoving += GameEventBusGameNextState;
        GameEventBus.OnFinishedSpawning += GameEventBusGameNextState;
        GameEventBus.OnShootingEnded += GameEventBusOnShootingEnded;
        GameEventBus.OnPlayClicked += GameEventBusOnPlayClicked;
        GameEventBus.OnMenuClicked += GameEventBusOnMenuClicked;
    }

    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnAllTargetsDestroyed -= GameEventBusGameNextState;
        GameEventBus.OnFinishedMoving -= GameEventBusGameNextState;
        GameEventBus.OnFinishedSpawning -= GameEventBusGameNextState;
        GameEventBus.OnShootingEnded -= GameEventBusOnShootingEnded;
        GameEventBus.OnPlayClicked -= GameEventBusOnPlayClicked;
        GameEventBus.OnMenuClicked -= GameEventBusOnMenuClicked;
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
        print(TargetSpawnPointIndex - listOffset);
        if ((TargetSpawnPointIndex - listOffset) >= 0)
        {
            _currentLevel.DestroyObstacles(TargetSpawnPointIndex - listOffset);
        }
    }
    private void GameEventBusGameNextState()
    {
        NextState();
    }
    public List<Level> GetLevelsList()
    {
        return _levelsList;
    }
    private void ResetLevel()
    {
        MovePointIndex = 0;
        TargetSpawnPointIndex = 0;
        _currentLevelProgress = 0;
        _currentLevel = _levelsList[CurrentLevelIndex];
        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();
        _player.position = _playerDefaultSpawnPoint;
        _player.rotation = Quaternion.Euler(0f, 90f, 0f);
        _currentLevel.ResetLevel();
        ChangeCurrentGameState(GameState.Menu);
    }
}
