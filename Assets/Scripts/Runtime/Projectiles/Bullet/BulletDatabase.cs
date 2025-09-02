using System.Collections.Generic;
using UnityEngine;

public class BulletDatabase : MonoBehaviour
{
    public static BulletDatabase Instance;

    [SerializeField] private BulletPrototypeContainerSO _bulletPrototypesContainer;

    private Dictionary<string, BulletPrototypeSO> _bulletDictionary = new Dictionary<string, BulletPrototypeSO>();
    private void Awake()
    {
        InitSingleton();
        InitDictionary();
    }
    public BulletPrototypeSO GetPrototype(string key)
    {
        return _bulletDictionary[key];
    }
    private void InitDictionary()
    {
        for (int i = 0; i < _bulletPrototypesContainer.BulletPrototypes.Count; i++)
        {
            BulletPrototypeEntry bulletPrototypeEntry = _bulletPrototypesContainer.BulletPrototypes[i];
            _bulletDictionary.Add(bulletPrototypeEntry.Key, bulletPrototypeEntry.Prototype);
        }
    }
    private void InitSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
