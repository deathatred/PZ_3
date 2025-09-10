using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public static class FirebaseFacade
{
    private static FirebaseBootstrap _bootstrap;
    private static FirebaseSaveLoadManager _saveLoad;

    public static async UniTask Init()
    {
        _bootstrap = new FirebaseBootstrap();
        await _bootstrap.Init();

        _saveLoad = new FirebaseSaveLoadManager(_bootstrap);                                    
    }
    public static async UniTask SaveLevelStars(int level, Stars starsCount)
    {
        await _saveLoad.SaveDataToFirebaseAsync(level, starsCount);
    }
    public static async UniTask<int> GetLevelStars(int level)
    {
        return await _saveLoad.LoadDataFromFromFirebaseAsync(level);
    }

    public static async UniTask ClearAllData()
    {
        await _saveLoad.ClearAllDataFromFirebaseAsync();
    }
  
}
