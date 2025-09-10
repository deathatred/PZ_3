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
    [SerializeField] private Transform _player;
    private ILevelService _levelService;
    private GameStateController _gameStateController;
    private PlayerSpawnService _playerSpawnService;

    private async void Awake()
    {
        InitSingleton();
        Init();
        await FirebaseFacade.Init();
        Debug.Log("Firebase Ready!");
    }
    private void OnDisable()
    {
        DispoceServises();
    }
    private void Init()
    {
        _gameStateController = new GameStateController(this);
        _gameStateController.Init();
        _levelService = new LevelService(_levelsList, this, _player);
        _levelService.Init();
        _levelService.SetCurrentLevel();
        _playerSpawnService = new PlayerSpawnService(_player);
    }
    private void DispoceServises()
    {
        _levelService.Dispose();
        _gameStateController.Dispose();
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
    public Transform GetPlayer()
    {
        return _player;
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

}
