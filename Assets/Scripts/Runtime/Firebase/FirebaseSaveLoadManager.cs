using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Database;
using UnityEngine;

public class FirebaseSaveLoadManager
{
    private const string LEVEL_STARS_COUNT_KEY = "LEVEL_{0}_STARS_COUNT";

    private bool _levelDirty = false;

    private FirebaseBootstrap _firebase;

    public FirebaseSaveLoadManager(FirebaseBootstrap bootstrap)
    {
        _firebase = bootstrap;
    }
    public async UniTask SaveDataToFirebaseAsync(int levelNumber, Stars starsCount)
    {
        var db = FirebaseBootstrap.Db;
        var uid = FirebaseBootstrap.Uid;

        string key = string.Format(LEVEL_STARS_COUNT_KEY, levelNumber);
        await db.Child($"users/{uid}/{key}").SetValueAsync((int)starsCount);
    }
    public async UniTask<int> LoadDataFromFromFirebaseAsync(int levelNumber)
    {
        var db = FirebaseBootstrap.Db;
        var uid = FirebaseBootstrap.Uid;

        string key = string.Format(LEVEL_STARS_COUNT_KEY, levelNumber);
        var snapshot = await db.Child($"users/{uid}/{key}").GetValueAsync();
        if (snapshot.Exists && int.TryParse(snapshot.Value.ToString(), out int starsCount))
        {
            return starsCount; 
        }

        return 0;
    }
    public async UniTask ClearAllDataFromFirebaseAsync()
    {
        var db = FirebaseBootstrap.Db;
        var uid = FirebaseBootstrap.Uid;

        var t = db.Child($"$users/{uid}").RemoveValueAsync();
        await t.AsUniTask();
    }
}
