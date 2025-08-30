using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRb;
    private float _moveSpeed = 5f;
    private CancellationTokenSource _moveCts;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        Init();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void Init()
    {
        _playerRb = GetComponent<Rigidbody>();
    }
    private void MoveToTarget(Transform target)
    {
        _moveCts?.Cancel();
        _moveCts = new CancellationTokenSource();

        MoveToTargetAsync(target, _moveCts.Token).Forget();
    }
    private async UniTask MoveToTargetAsync(Transform target, CancellationToken token)
    {
        await RotateToTargetAsync(target, token);

       
        while (Vector3.Distance(transform.position, target.position) > 0.6f)
        {
            token.ThrowIfCancellationRequested();

            Vector3 direction = (target.position - transform.position).normalized;
            _playerRb.linearVelocity = direction * _moveSpeed;
            await UniTask.WaitForFixedUpdate();
        }
        _playerRb.linearVelocity = Vector3.zero;
        GameEventBus.FinishedMoving();
    }
    private async UniTask RotateToTargetAsync(Transform target, CancellationToken token)
    {
        float rotationSpeed = 100f;
        Vector3 direction = (target.position - transform.position).normalized;
        while (Vector3.Angle(transform.forward, direction) > 1f)
        {
            token.ThrowIfCancellationRequested();
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
        GameEventBus.OnMenuClicked += GameEventBusOnMenuClicked;
    }

    private void GameEventBusOnMenuClicked()
    {
        _moveCts?.Cancel();
        _playerRb.linearVelocity = Vector3.zero;
    }

    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnNewMovingPoint -= GameEventBusOnNewMovingPoint;
        GameEventBus.OnMenuClicked -= GameEventBusOnMenuClicked;
    }
    private void GameEventBusOnNewMovingPoint(Transform obj)
    {
        MoveToTarget(obj);
    }
}
