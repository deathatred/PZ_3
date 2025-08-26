using System;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private ColorsSO _colorsSO;

    private MeshRenderer _renderer;
    private int _health = 1;
    private void OnEnable()
    {
        _renderer = GetComponent<MeshRenderer>();
        List<Color> _colorsList = _colorsSO.ColorsList;
        Color matColor = _colorsList[UnityEngine.Random.Range(0, _colorsList.Count)];
        _renderer.material.color = matColor;
    }
    public void TakeDamage(int amount)
    {
        if (_health > 0)
        {
            _health -= amount;
        }
        if (_health <= 0)
        {
            GameEventBus.TargetDestroyed(transform);
            //TODO: Spawn Particles
            Destroy(gameObject);

        }
    }
}
