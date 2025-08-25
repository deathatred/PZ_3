using UnityEngine;

public abstract class MovingObstacle : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 2f;
    
    protected virtual void Update()
    {
        Move();
    }
    protected abstract void Move();
}
