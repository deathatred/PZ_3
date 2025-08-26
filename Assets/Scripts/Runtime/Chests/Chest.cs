
using System.Runtime.CompilerServices;
using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator _animator;
    public static int HitHash = Animator.StringToHash("Hit");
    public static int OpenHash = Animator.StringToHash("IsOpened");
    private int _health = 5;
    
    public void TakeDamage(int amount)
    {
        if (_health > 0)
        {
            print("chest damaged");
            _health -= amount;
            _animator.SetTrigger(HitHash);
        }
        if (_health <= 0)
        {
            GameEventBus.TargetDestroyed(transform);
            _animator.SetBool(OpenHash, true);
            Destroy(gameObject, 5f);
        }
    }
}
