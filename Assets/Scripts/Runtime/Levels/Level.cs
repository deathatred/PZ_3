using System.Collections.Generic;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Transform> _movePoint = new List<Transform>();
    [SerializeField] private List<Transform> _targetSpawnPoints = new List<Transform>();
    [SerializeField] private Transform _targetPrefab;
    [SerializeField] private LevelFlowSO _levelFlowSO;

    private List<Transform> _targets = new List<Transform>();
    private int _targetsAmount = 10;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Start()
    {
        SpawnTargets(0); //TODO: Change to event that spawns depending on game state
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
    private void SpawnTargets(int index)
    {   
        Vector3 currentTargetPos = _targetSpawnPoints[index].position;
        float offset = 0.5f;
        float offsetAmount = 1f;
        for (int i = 0; i < _targetsAmount; i++)
        {
            print("spawning");
            Transform target = GameObject.Instantiate(_targetPrefab,
                new Vector3(currentTargetPos.x, currentTargetPos.y + offset, currentTargetPos.z),
                Quaternion.identity);
            _targets.Add(target);
            offset += offsetAmount;
        }
    }
    private void TargetDestroyed(Transform target)
    {
        _targets.Remove(target);
        LowerAllTargets().Forget();
        if (_targets.Count == 0 )
        {
            GameEventBus.AllTargetsDestroyed();
        }
    }
    private async UniTask LowerAllTargets()
    {
        float amountToLower = 1f;
        float duration = 1f;
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
        GameEventBus.OnTargetDestroyed += TargetDestroyed;
    }
    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnTargetDestroyed -= TargetDestroyed;
    }
}
