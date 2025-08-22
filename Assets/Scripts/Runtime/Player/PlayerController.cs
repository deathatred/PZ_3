using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRb;
    private float _moveSpeed = 5f;

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
    }
    private void MoveToTarget(Transform target)
    {
        MoveToTargetAsync(target).Forget();
    }
    private async UniTask MoveToTargetAsync(Transform target)
    {
        await RotateToTarget(target);

        Vector3 direction = (target.position - transform.position).normalized;
        while (Vector3.Distance(transform.position, target.position) > 0.6f)
        {
            _playerRb.linearVelocity = direction * _moveSpeed;
            await UniTask.WaitForFixedUpdate();
        }
        _playerRb.linearVelocity = Vector3.zero;
    }
    private async UniTask RotateToTarget(Transform target)
    {
        float rotationSpeed = 100f;
        Vector3 direction = (target.position - transform.position).normalized;
        while (Vector3.Angle(transform.forward, direction) > 1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
            await UniTask.NextFrame();
        }
    }
 
}
