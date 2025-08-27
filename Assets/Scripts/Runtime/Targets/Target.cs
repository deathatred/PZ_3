using System;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private ColorsSO _colorsSO;
    [SerializeField] private ParticleSystem _particles;

   
    private Color _mainColor;
    private int _health = 1;
    public void TakeDamage(int amount)
    {
        if (_health > 0)
        {
            _health -= amount;
        }
        if (_health <= 0)
        {
            GameEventBus.TargetDestroyed(transform);
            Vector3 worldPos = _particles.transform.position;
            Quaternion worldRot = _particles.transform.rotation;
            Vector3 worldScale = _particles.transform.lossyScale;
            _particles.transform.parent = null;
            _particles.transform.position = worldPos;
            _particles.transform.rotation = worldRot;
            _particles.transform.localScale = Vector3.one;
            var main = _particles.main;
            main.startColor = _mainColor;
            _particles.Play();
       
            Destroy(gameObject);
        }
    }
    public void SetMainColor(Color color)
    {
        _mainColor = color;
    }
}
