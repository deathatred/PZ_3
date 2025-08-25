using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Level _currentLevel;

    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; }

    private LevelFlowSO _currentLevelFlowSO;
    private int _currentLevelProgress = 0;
    private int _currentMovePointIndex = 0;
    private int _currentTargetSpawnPointIndex = 0;
    private Transform _player;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        InitSingleton();
        ChangeCurrentGameState(GameState.Started);

        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Start()
    {
        ChangeCurrentGameState(GameState.Started);
        _currentLevelFlowSO = _currentLevel.GetLevelFlowSO();
    }
    private void Update()
    {

    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void ChangeCurrentGameState(GameState state)
    {
        CurrentGameState = state;
        switch (CurrentGameState)
        {
            case GameState.Started:
                GameEventBus.LevelLoaded();
                break;
            case GameState.MovingToNextPoint:
                GameEventBus.SetNewMovingPoint(_currentLevel.GetMoveTarget(_currentMovePointIndex));
                _currentMovePointIndex++;
                break;
            case GameState.SpawningTargets:
                GameEventBus.SpawnTargets(_currentLevel.GetTargetSpawnPoint(_currentTargetSpawnPointIndex));
                _currentTargetSpawnPointIndex++;
                break;
            case GameState.Shooting:

                break;
            case GameState.Paused:
                break;
            case GameState.Finished:
                break;
        }
        _currentLevelProgress++;
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
        //OnLevelLoaded
        //OnFinishedMoving
        //Every action that changes state must be an event to trigger state change
        GameEventBus.OnAllTargetsDestroyed += GameEventBusOnAllTargetsDestroyed;
    }
    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnAllTargetsDestroyed -= GameEventBusOnAllTargetsDestroyed;
    }
    private void GameEventBusOnAllTargetsDestroyed()
    {
        ChangeCurrentGameState(_currentLevelFlowSO.GameStateList[_currentLevelProgress]);
    }
}
