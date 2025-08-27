using UnityEngine;

public class MovingObstacleHorizontal : MovingObstacle
{
    [SerializeField] private float _frequency = 0.2f;
    [SerializeField] private float _distance = 5f;

    private Vector3 _localStartPos;

    private void OnEnable()
    {
        _localStartPos = transform.localPosition;
    }

    protected override void Move()
    {
        float offset = Mathf.Sin(Time.time * _frequency * 2 * Mathf.PI) * _distance;
        transform.localPosition = _localStartPos + Vector3.forward * offset;
    }
}
