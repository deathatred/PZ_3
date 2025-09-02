using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _flySpeed = 15f;
    private ObjectPool<Bullet> _bulletPool;
    private float _lifeTime = 5f;
    private float _lifeTimer;
    private int _damage = 1;

    private void Awake()
    {
        SetTimer();
    }
    public void Init(ObjectPool<Bullet> _pool)
    {
        _bulletPool = _pool;
    }
    private void Update()
    {
        HandleMovement();
    }
  
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(_damage);
            }
            ReturnToPool();
        }
    }
    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        _bulletPool.ReturnToPool(this);
    }
    private void HandleMovement()
    {
        transform.position += transform.forward * Time.deltaTime * _flySpeed;

        if (_lifeTimer > 0)
        {
            _lifeTimer -= Time.deltaTime;
        }
        else
        {
            ReturnToPool();
        }
    }
    private void SetTimer()
    {
        _lifeTimer = _lifeTime;
    }
    public void ApplyPrototype(BulletPrototypeSO prototype)
    {
        _flySpeed = prototype.Speed;
        _damage = prototype.Damage;
        GetComponentInChildren<MeshRenderer>().material.color = prototype.Color;
    }
}
