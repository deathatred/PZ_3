using System;
using UnityEngine;

public static class GameEventBus 
{
    public static event Action<Transform> OnTargetDestroyed;
    public static event Action<Transform> OnSpawningTargets;
    public static event Action<Transform> OnNewMovingPoint;

    public static event Action OnLevelLoaded;
    public static event Action OnAllTargetsDestroyed;
    public static event Action OnShoot;

    public static void Shoot()
    {
        OnShoot?.Invoke();
    }
    public static void TargetDestroyed(Transform target)
    {
        OnTargetDestroyed?.Invoke(target);
    }
    public static void LevelLoaded()
    {
        OnLevelLoaded?.Invoke();
    }
    public static void SpawnTargets(Transform targetSpawnPos)
    {
        OnSpawningTargets?.Invoke(targetSpawnPos);
    }
    public static void SetNewMovingPoint(Transform movingPoint)
    {
        OnNewMovingPoint?.Invoke(movingPoint);
    }
    public static void AllTargetsDestroyed()
    {
        OnAllTargetsDestroyed?.Invoke();
    }
}
