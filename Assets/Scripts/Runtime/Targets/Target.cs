using System;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private ColorsSO _colorsSO;
    [SerializeField] private ParticleSystem _particles;

    private MeshRenderer _renderer;
    private Color mainColor;
    private int _health = 1;
    private void OnEnable()
    {
        Init();
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
            Vector3 worldPos = _particles.transform.position;
            Quaternion worldRot = _particles.transform.rotation;
            Vector3 worldScale = _particles.transform.lossyScale;
            _particles.transform.parent = null;
            _particles.transform.position = worldPos;
            _particles.transform.rotation = worldRot;
            _particles.transform.localScale = Vector3.one;
            var main = _particles.main;
            main.startColor = mainColor;
            _particles.Play();
       
            Destroy(gameObject);
        }
    }
    private void Init()
    {
        _renderer = GetComponent<MeshRenderer>();
        List<Color> _colorsList = _colorsSO.ColorsList;
        mainColor = _colorsList[UnityEngine.Random.Range(0, _colorsList.Count)];
        _renderer.material.color = mainColor;
    }
}
