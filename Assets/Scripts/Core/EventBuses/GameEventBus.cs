using System;
using UnityEngine;

public static class GameEventBus 
{
    public static event Action OnShoot;

    public static void Shoot()
    {
        OnShoot?.Invoke();
    }
}
