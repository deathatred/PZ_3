using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    private int _bullets;
    private ObjectPool<Bullet> _bulletPool;
    private float _shootTimer;
    private float _shootTimerMax = 0.3f;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        _bulletPool = new(_bulletPrefab, 30);
    }
    private void Update()
    {
        HandleShooting();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SpawnBullet()
    {
        Bullet bullet = _bulletPool.Get();
        bullet.Init(_bulletPool);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.gameObject.SetActive(true);
    }
    private void HandleShooting()
    {
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }
        if (PlayerInputHandler.Instance.ShootPressed && _shootTimer <= 0 && 
            GameManager.Instance.CurrentGameState is ShootingState)
        {
            SpawnBullet();
            _bullets -= 1;
            GameEventBus.BulletShot(_bullets);
            _shootTimer = _shootTimerMax;
        }
    }
    private void SubscribeToEvents()
    {
        GameEventBus.OnLevelLoaded += GameEventBusOnLevelLoaded;
    }

    private void GameEventBusOnLevelLoaded(Level level)
    {
        _bullets = level.GetLevelInfoSO().NumberOfBullets;
    }

    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnLevelLoaded -= GameEventBusOnLevelLoaded;
    }
    public int GetBullets()
    {
        return _bullets;
    }
}
