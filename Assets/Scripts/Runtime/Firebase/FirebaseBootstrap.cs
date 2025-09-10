using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System;
using Firebase.Database;
using Firebase;
using Firebase.Auth;

public class FirebaseBootstrap 
{
    private const string DATABASE_URL = 
        "https://pz-3-10fd7-default-rtdb.europe-west1.firebasedatabase.app";

    public static string Uid { get; private set; }
    public static DatabaseReference Db { get; private set; }
    public static bool IsReady { get; private set; }

    public async UniTask Init(Action onReady = null, bool force = false)
    {
        if (IsReady && !force)
        {
            onReady?.Invoke();
            return;
        }

        IsReady = false;

        var depTask = FirebaseApp.CheckAndFixDependenciesAsync();
        await depTask.AsUniTask();

        if (depTask.IsFaulted || depTask.Result != DependencyStatus.Available)
        {
            Debug.LogError($" FB Deps: {depTask.Result}");
            return;
        }

        var app = FirebaseApp.DefaultInstance;
        var auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser == null)
        {
            var signIn = auth.SignInAnonymouslyAsync();
            await signIn.AsUniTask();
            if (signIn.IsFaulted) return;
        }
        Uid = auth.CurrentUser.UserId;

        var db = FirebaseDatabase.GetInstance(app, DATABASE_URL);
        db.SetPersistenceEnabled(false);
        Db = db.RootReference;

        IsReady = true;
        onReady?.Invoke();
        Debug.Log($"FB Ready, uid = {Uid}");
    }
}
                                                                                                                                                   