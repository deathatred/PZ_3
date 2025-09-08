using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<Level> _levelsList;
    private ILevelService _levelService;
    private GameStateController _gameStateController;
    private PlayerSpawnService _playerSpawnService;
    private Transform _player;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        InitSingleton();
        GetPlayer();
        Init();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void Init()
    {
        _gameStateController = new GameStateController(this);
        _levelService = new LevelService(_levelsList,this, _player);
        _levelService.SetCurrentLevel();
        _playerSpawnService = new PlayerSpawnService(_player);
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
        GameEventBus.OnLevelFinishedLoading += GameEventBus_OnLevelFinishedLoading;
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
        GameEventBus.OnLevelFinishedLoading -= GameEventBus_OnLevelFinishedLoading;
    }
    private void GameEventBusOnMenuClicked()
    {
        _levelService.ResetLevel();
        _gameStateController.ChangeCurrentGameState(GameState.Menu);
    }
    private void GameEventBusOnPlayClicked()
    {
        _gameStateController.ChangeCurrentGameState(GameState.Started);
    }
    private void GameEventBusOnShootingEnded()
    {
        int listOffset = 1;
        if ((_levelService.TargetSpawnPointIndex - listOffset) >= 0)
        {
            _levelService.CurrentLevel.DestroyObstacles(_levelService.TargetSpawnPointIndex - listOffset);
        }
    }
    private void GameEventBusGameNextState()
    {
        _gameStateController.NextState();
    }
    private void GameEventBusOnNextClicked()
    {
        _levelService.StartNextLevelWithDelayAsync().Forget();
    }
    private void GameEventBus_OnLevelFinishedLoading()
    {
        _gameStateController.ChangeCurrentGameState(GameState.Started);
    }
    public int GetRemainingBullets()
    {
        var playerShooting = _player.GetComponent<PlayerShooting>();
        return playerShooting.GetBullets();
    }
    public ILevelService GetCurrentLevelService()
    {
        return _levelService;
    }
    public GameStateController GetGameStateController()
    {
        return _gameStateController;
    }
    public PlayerSpawnService GetPlayerSpawnService()
    {
        return _playerSpawnService;
    }
    public bool IsTargetSpawnPointLast()
    {
        if (_levelService.TargetSpawnPointIndex == _levelService.CurrentLevel.GetTargetSpawnPointsCount() - 1)
        {
            return true;
        }
        return false;
    }
}
