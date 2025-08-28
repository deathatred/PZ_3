using System;
using UnityEngine;

public static class GameEventBus
{
    #region Game Events
    public static event Action<Transform> OnTargetDestroyed;
    public static event Action<int, bool> OnSpawningTargets;
    public static event Action<Transform> OnNewMovingPoint;
    public static event Action OnLevelLoaded;
    public static event Action OnAllTargetsDestroyed;
    public static event Action OnFinishedMoving;
    public static event Action OnFinishedSpawning;
    public static event Action OnShootingEnded;
    public static event Action OnShoot;
    #endregion
    #region UI Events
    public static event Action OnSettingsClicked;
    public static event Action OnMenuClicked;
    public static event Action OnPlayClicked;
    public static event Action OnBackClicked;
    public static event Action OnLevelsClicked;
    #endregion

    #region Game Event Invokes
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
    public static void SpawnTargets(int targetSpawnPosIndex, bool isLast)
    {
        OnSpawningTargets?.Invoke(targetSpawnPosIndex, isLast);
    }
    public static void SetNewMovingPoint(Transform movingPoint)
    {
        OnNewMovingPoint?.Invoke(movingPoint);
    }
    public static void AllTargetsDestroyed()
    {
        OnAllTargetsDestroyed?.Invoke();
    }
    public static void FinishedMoving()
    {
        OnFinishedMoving?.Invoke();
    }
    public static void FinishedSpawning()
    {
        OnFinishedSpawning?.Invoke();
    }
    public static void ShootingEnded()
    {
        OnShootingEnded?.Invoke();
    }
    #endregion
    #region UI Event Invokes
    public static void MenuClicked()
    {
        OnMenuClicked?.Invoke();
    }
    public static void SettingsClicked()
    {
        OnSettingsClicked?.Invoke();
    }
    public static void PlayClicked()
    {
        OnPlayClicked?.Invoke();
    }
    public static void LevelsClicked()
    {
        OnLevelsClicked?.Invoke();
    }
    public static void BackClicked()
    {
        OnBackClicked?.Invoke();
    }
    #endregion
}
