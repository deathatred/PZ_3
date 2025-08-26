using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Transform> _movePoint = new List<Transform>();
    [SerializeField] private List<Transform> _targetSpawnPoints = new List<Transform>();
    [SerializeField] private Transform _targetPrefab;
    [SerializeField] private Transform _chestPrefab;
    [SerializeField] private LevelFlowSO _levelFlowSO;

    private Transform _player;
    private List<Transform> _targets = new List<Transform>();
    private int _targetsAmount = 10;
    private CancellationTokenSource _cts;
    private float _targetOffset = 0f; // скільки в сумі треба зміститися вниз

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    public Transform GetMoveTarget(int index)
    {
        return _movePoint[index];
    }
    public Transform GetTargetSpawnPoint(int index)
    {
        return _targetSpawnPoints[index];
    }
    public LevelFlowSO GetLevelFlowSO()
    {
        return _levelFlowSO;
    }
    public int GetTargetSpawnPointsCount()
    {
        return _targetSpawnPoints.Count;
    }
    private void SpawnTargets(int index, bool last = false)
    {
        //TODO: make animation
        Vector3 currentTargetPos = _targetSpawnPoints[index].position;
        float offset = 0.5f;
        float offsetAmount = 1f;

        for (int i = 0; i < _targetsAmount; i++)
        {
            Transform target = GameObject.Instantiate(_targetPrefab,
                new Vector3(currentTargetPos.x, currentTargetPos.y + offset, currentTargetPos.z),
                Quaternion.identity);
            Vector3 targetPos = new Vector3(_player.position.x, target.position.y, _player.position.z);
            target.LookAt(targetPos);
            _targets.Add(target);

            offset += offsetAmount;
            if (last && i == _targetsAmount - 1)
            {
                float chestPivotOffset = 0.5f;
                Transform chest = GameObject.Instantiate(_chestPrefab,
                    new Vector3(target.position.x - chestPivotOffset,
                    target.position.y + chestPivotOffset, target.position.z),
                    Quaternion.identity);
                _targets.Add(chest);
                chest.LookAt(targetPos);
            }
        }
        GameEventBus.FinishedSpawning();
    }
    private void TargetDestroyed(Transform target)
    {
        if (_targets.Remove(target)) 
        {
            LowerAllTargets().Forget();
            if (_targets.Count == 0)
            {
                print("all target destroyed");
                GameEventBus.AllTargetsDestroyed();
            }
        }
    }
    private async UniTask LowerAllTargets()
    {
        float amountToLower = 1f;
        float duration = 0.1f;
        float time = 0f;

        Dictionary<Transform, Vector3> startPositions = new Dictionary<Transform, Vector3>();
        foreach (Transform target in _targets)
        {
            startPositions[target] = target.position;
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            foreach (Transform target in _targets)
            {
                Vector3 start = startPositions[target];
                Vector3 end = start + Vector3.down * amountToLower;
                target.position = Vector3.Lerp(start, end, t);
            }


            await UniTask.NextFrame();
        }
    }

    private void SubscribeToEvents()
    {
        GameEventBus.OnSpawningTargets += GameEventBus_OnSpawningTargets;
        GameEventBus.OnTargetDestroyed += TargetDestroyed;
    }

    private void GameEventBus_OnSpawningTargets(int index, bool isLast)
    {
        SpawnTargets(index, isLast);
    }

    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnTargetDestroyed -= TargetDestroyed;
    }
    public void SetPlayer(Transform player)
    {
        _player = player;
    }
}
