using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public static class FirebaseFacade
{
    private static FirebaseBootstrap _bootstrap;
    private static FirebaseSaveLoadManager _saveLoad;

    private static UniTaskCompletionSource _initCompletion;

    private static bool _isInitialized;
    public static async UniTask Init()
    {
        if (_isInitialized) return;
        if (_initCompletion != null)
        {
            await _initCompletion.Task;
            return;
        }
        _initCompletion = new UniTaskCompletionSource();
        _bootstrap = new FirebaseBootstrap();
        await _bootstrap.Init();

        _saveLoad = new FirebaseSaveLoadManager(_bootstrap);
        _initCompletion.TrySetResult();
        _isInitialized = true;
    }
    public static async UniTask EnsureInitialized()
    {
        if (_initCompletion == null)
        {
           Init().Forget();
        }
        await _initCompletion.Task;
    }
    public static async UniTask SaveLevelStars(int level, Stars starsCount)
    {
        await EnsureInitialized();
        await _saveLoad.SaveLevelDataToFirebaseAsync(level, starsCount);
    }
    public static async UniTask<int> GetLevelStars(int level)
    {
        await EnsureInitialized();
        return await _saveLoad.LoadDataFromFromFirebaseAsync(level);
    }

    public static async UniTask ClearAllData()
    {
        await EnsureInitialized();
        await _saveLoad.ClearAllDataFromFirebaseAsync();
    }
  
}
