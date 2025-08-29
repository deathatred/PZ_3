using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Transform> _movePoint = new List<Transform>();
    [SerializeField] private List<Transform> _targetSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> _obstacleSets = new List<Transform>();
    [SerializeField] private InfoLevelSO _infoLevelSO;
    [SerializeField] private ColorsSO _colorScheme;
    [SerializeField] private Transform _targetPrefab;
    [SerializeField] private Transform _chestPrefab;
    [SerializeField] private LevelFlowSO _levelFlowSO;

    private Transform _player;
    private List<Transform> _targets = new List<Transform>();
    private int _targetsAmount = 10;
    private CancellationTokenSource _targetCts;

    private void OnEnable()
    {
        SubscribeToEvents();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Start()
    {
        _targetCts = new CancellationTokenSource();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private async void SpawnTargets(int index, bool last = false)
    {
        var token = _targetCts.Token;

        Vector3 spawnBasePos = _targetSpawnPoints[index].position;
        float baseOffset = 0.5f;
        float offsetStep = 1f;
        float delayBetweenTargets = 0.1f;
        List<Transform> spawnedTargets = new List<Transform>();
        if (last)
        {
            Transform chest = SpawnChest(_targetSpawnPoints[index].transform, new Vector3(_player.position.x, spawnBasePos.y, _player.position.z));
            spawnedTargets.Add(chest);
            _targets.Add(chest);
            await AnimateTargetRiseAsync(chest, baseOffset, 0.3f, token);
            try
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(delayBetweenTargets), cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
        for (int i = 0; i < _targetsAmount; i++)
        {
            token.ThrowIfCancellationRequested();
            float yOffset = 1.5f;
            Vector3 spawnPos = new Vector3(spawnBasePos.x, spawnBasePos.y - yOffset, spawnBasePos.z);
            Transform target = Instantiate(_targetPrefab, spawnPos, Quaternion.identity);
            spawnedTargets.Add(target);
            _targets.Add(target);

            AssignTargetColor(target, i);

            Vector3 lookPos = new Vector3(_player.position.x, target.position.y, _player.position.z);
            target.LookAt(lookPos);

            List<UniTask> riseTasks = new List<UniTask>();
            for (int j = 0; j < spawnedTargets.Count; j++)
            {
                float targetHeight = baseOffset + (spawnedTargets.Count - j - 1) * offsetStep;
                riseTasks.Add(AnimateTargetRiseAsync(spawnedTargets[j], targetHeight, 0.1f, token));
            }

            await UniTask.WhenAll(riseTasks);
            try
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(delayBetweenTargets), cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return; 
            }
        }    
        GameEventBus.FinishedSpawning();
    }
    private async UniTask AnimateTargetRiseAsync(Transform target, float finalY, float duration, CancellationToken token)
    {
        Vector3 startPos = target.position;
        Vector3 endPos = new Vector3(startPos.x, finalY, startPos.z);
        float time = 0f;

        while (time < duration)
        {
            token.ThrowIfCancellationRequested();

            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            target.position = Vector3.Lerp(startPos, endPos, t);
            await UniTask.NextFrame();
        }

        target.position = endPos;
    }
    private void AssignTargetColor(Transform target, int index)
    {
        var renderer = target.GetComponent<Renderer>();
        var color = (index % 2 != 0) ? _colorScheme.ColorsList[0] : _colorScheme.ColorsList[1];

        renderer.material.color = color;
        renderer.material.EnableKeyword("_EMISSION");
        float emissionAmount = 0.5f;
        renderer.material.SetColor("_EmissionColor", color * emissionAmount);

        target.GetComponent<Target>().SetMainColor(color);
    }

    private Transform SpawnChest(Transform lastTarget, Vector3 lookAtPos)
    {
        float chestPivotOffset = 0.5f;

        Vector3 spawnPos = new Vector3(
            lastTarget.position.x,
            lastTarget.position.y - 1.5f + chestPivotOffset,
            lastTarget.position.z
        );

        Transform chest = Instantiate(_chestPrefab, spawnPos, Quaternion.identity);
        Vector3 targetLookPos = new Vector3(lookAtPos.x, chest.position.y, lookAtPos.z);
        chest.LookAt(targetLookPos);

        return chest;
    }
    private void TargetDestroyed(Transform target)
    {
        if (target.CompareTag("Chest"))
        {
            if (_targets.Count == 1)
            {
                _targets.Remove(target);
                GameEventBus.AllTargetsDestroyed();
            }
            else
            {
                _targets.Remove(target);
            }
            return;
        }
        if (_targets.Remove(target))
        {
            LowerAllTargetsAsync().Forget();
            if (_targets.Count == 0)
            {
                GameEventBus.AllTargetsDestroyed();
            }
        }
    }
    private async UniTask LowerAllTargetsAsync()
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
        GameEventBus.OnSpawningTargets += GameEventBusOnSpawningTargets;
        GameEventBus.OnTargetDestroyed += TargetDestroyed;
    }

    private void GameEventBusOnSpawningTargets(int index, bool isLast)
    {
        SpawnTargets(index, isLast);
    }

    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnSpawningTargets -= GameEventBusOnSpawningTargets;
        GameEventBus.OnTargetDestroyed -= TargetDestroyed;
    }
    public void DestroyObstacles(int index)
    {
        _obstacleSets[index].gameObject.SetActive(false);
    }
    public void SetPlayer(Transform player)
    {
        print("for" + this + " " + player + "is set");
        print((player == null) + "Is null?");
        _player = player;
        print(_player);
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
    public InfoLevelSO GetLevelInfoSO()
    {
        return _infoLevelSO;
    }
    public void ResetLevel()
    {
        _targetCts.Cancel();
        _targetCts = new CancellationTokenSource();
        foreach (var target in _targets)
        {
            if (target != null)
                GameObject.Destroy(target.gameObject);
        }
        _targets.Clear();
        foreach (var obstacle in  _obstacleSets)
        {
            obstacle.gameObject.SetActive(true);
        }
    }
}
