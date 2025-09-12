using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class FirebaseBootstrap
{
    private const string DATABASE_URL =
        "https://pz-3-10fd7-default-rtdb.europe-west1.firebasedatabase.app";

    public static string Uid { get; private set; }
    public static DatabaseReference Db { get; private set; }
    public static bool IsReady { get; private set; }


    public async UniTask Init(Action onReady = null, bool force = false,CancellationTokenSource cts = default)
    {
        if (IsReady && !force)
        {
            onReady?.Invoke();
            return;
        }

        IsReady = false;
        cts?.Cancel();
        cts = new CancellationTokenSource();
        try
        {
            var depTask = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (depTask != DependencyStatus.Available)
            {
                Debug.LogError($"Fb Deps: {depTask}");
            }

            var auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser == null)
            {
                var signIn = auth.SignInAnonymouslyAsync();
                await signIn.AsUniTask().AttachExternalCancellation(cts.Token);
                if (signIn.IsFaulted) return;
            }
            Uid = auth.CurrentUser.UserId;


            var app = FirebaseApp.DefaultInstance;
            var db = FirebaseDatabase.GetInstance(app, DATABASE_URL);
            db.SetPersistenceEnabled(false);
            Db = db.RootReference;

            IsReady = true;
            onReady?.Invoke();
            Debug.Log($"FB Ready, uid = {Uid}");
        }
        catch (OperationCanceledException)
        {
            Debug.LogError("FB Init cancelled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"FB Init error {ex}");
        }
    }
}
