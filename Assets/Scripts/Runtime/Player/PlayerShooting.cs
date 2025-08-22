using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;

    private ObjectPool<Bullet> _bulletPool;
    private float _shootTimer;
    private float _shootTimerMax = 0.3f;

    private void Awake()
    {
        _bulletPool = new(_bulletPrefab, 30);
    }
    private void Update()
    {
        HandleShooting();
    }
    private void SpawnBullet()
    {
        Bullet bullet = _bulletPool.Get();
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
        if (PlayerInputHandler.Instance.ShootPressed && _shootTimer <= 0)
        {
            SpawnBullet();
            _shootTimer = _shootTimerMax;
        }
    }
}
