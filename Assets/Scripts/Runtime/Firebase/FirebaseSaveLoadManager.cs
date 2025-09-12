using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Database;
using System;
using System.Threading;
using UnityEngine;

public class FirebaseSaveLoadManager
{
    private const string LEVEL_STARS_COUNT_KEY = "LEVEL_{0}_STARS_COUNT";


    private FirebaseBootstrap _firebase;

    public FirebaseSaveLoadManager(FirebaseBootstrap bootstrap)
    {
        _firebase = bootstrap;
    }
    public async UniTask SaveLevelDataToFirebaseAsync(int levelNumber, Stars starsCount, 
        CancellationTokenSource cts = default)
    {
        try
        {
            var db = FirebaseBootstrap.Db;
            var uid = FirebaseBootstrap.Uid;

            string key = string.Format(LEVEL_STARS_COUNT_KEY, levelNumber);
            await db.Child($"users/{uid}/{key}").SetValueAsync((int)starsCount).
                AsUniTask().AttachExternalCancellation(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Saving data from FB canceled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Saving data erorred {ex}");
        }
    }
    public async UniTask<int> LoadDataFromFromFirebaseAsync(int levelNumber, CancellationTokenSource cts = default)
    {
        try
        {
            var db = FirebaseBootstrap.Db;
            var uid = FirebaseBootstrap.Uid;

            string key = string.Format(LEVEL_STARS_COUNT_KEY, levelNumber);
            var snapshot = await db.Child($"users/{uid}/{key}").GetValueAsync().
                AsUniTask().AttachExternalCancellation(cts.Token);
            if (snapshot.Exists && int.TryParse(snapshot.Value.ToString(), out int starsCount))
            {
                return starsCount;
            }

            return 0;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Loading data from FB canceled");
            return 0;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Loading data erorred {ex}");
            return 0;
        }
    }
    public async UniTask ClearAllDataFromFirebaseAsync(CancellationTokenSource cts = default)
    {
        var db = FirebaseBootstrap.Db;
        var uid = FirebaseBootstrap.Uid;

        var t = db.Child($"$users/{uid}").RemoveValueAsync();
        await t.AsUniTask().AttachExternalCancellation(cts.Token);
    }
}
