using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRb;
    private float _moveSpeed = 5f;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void MoveToTarget(Transform target)
    {
        MoveToTargetAsync(target).Forget();
    }
    private async UniTask MoveToTargetAsync(Transform target)
    {
        await RotateToTarget(target);

       
        while (Vector3.Distance(transform.position, target.position) > 0.6f)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            _playerRb.linearVelocity = direction * _moveSpeed;
            await UniTask.WaitForFixedUpdate();
        }
        _playerRb.linearVelocity = Vector3.zero;
        GameEventBus.FinishedMoving();
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

    private void SubscribeToEvents()
    {
        GameEventBus.OnNewMovingPoint += GameEventBusOnNewMovingPoint;
    }
    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnNewMovingPoint -= GameEventBusOnNewMovingPoint;
    }
    private void GameEventBusOnNewMovingPoint(Transform obj)
    {
        MoveToTarget(obj);
    }
}
