using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<Level> _levelsList;
    [Inject] private PlayerController _player;
    [Inject] private ILevelService _levelService;
    [Inject] private GameStateController _gameStateController;
    [Inject] private PlayerSpawnService _playerSpawnService;

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
        _gameStateController.Init();
        _levelService.Init();
        _levelService.SetCurrentLevel();
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
    public Transform GetPlayer()
    {
        return _player.transform;
    }
}
