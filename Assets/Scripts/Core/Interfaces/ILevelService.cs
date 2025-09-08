using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface ILevelService : IDisposable
{
    Level CurrentLevel { get; }
    int TargetSpawnPointIndex { get; }
    int MovePointIndex { get; }
    int CurrentLevelIndex { get; }
    void SetCurrentLevel();
    UniTask LoadLevelAsync(int index, CancellationToken token);
    UniTask StartNextLevelWithDelayAsync();
    UniTask NextLevelAsync(CancellationToken token);
    UniTask ChangeLevelAsync(int level);
    void ResetLevel();
    List<Level> GetLevelsList();
    GameState GetCurrentLevelFlowState();
    void ProgressLevel();
    void AddMovePointIndex();
    void AddTargetSpawnPointIndex();
    bool IsTargetSpawnPointLast();
    void Init();
}
