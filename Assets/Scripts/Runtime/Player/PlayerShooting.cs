using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;

    private ObjectPool<Bullet> _bulletPool;

    private void Awake()
    {
        _bulletPool = new(_bulletPrefab, 30);
    }

}
