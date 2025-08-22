using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Transform _player;
    private Transform _moveTarget;
    public GameState CurrentGameState { get; private set; }

    private void Awake()
    {
        InitSingleton();
        ChangeCurrentGameState(GameState.Started);

        _player = GameObject.FindGameObjectWithTag("Player").transform;     
    }
    private void ChangeCurrentGameState(GameState state)
    {
        CurrentGameState = state;
    }
    public void SetMoveTarget(Transform target)
    {
        _moveTarget = target;
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
    
}
