using UnityEngine;

public class MovingObstacleHorizontal : MovingObstacle
{
    [SerializeField] private float _frequency = 0.2f;
    [SerializeField] private float _distance = 5f;

    private Vector3 _localStartPos;

    private void OnEnable()
    {
        SetStartPos();
    }

    protected override void Move()
    {
        float offset = Mathf.Sin(Time.time * _frequency * 2 * Mathf.PI) * _distance;
        Vector3 localForward = transform.localRotation * Vector3.forward;
        transform.localPosition = _localStartPos + localForward * offset;
    }
    private void SetStartPos()
    {
        _localStartPos = transform.localPosition;
    }
}
