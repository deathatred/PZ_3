using UnityEngine;

public class MovingObstacleVertical : MovingObstacle
{
    private Vector3 _startPos;
    private float _distance = 5f;
    [SerializeField] private float _frequency = 0.2f;

    private void OnEnable()
    {
        _startPos = transform.position;
    }
    protected override void Move()
    {
        float xOffset = Mathf.Sin(Time.time * _frequency * 2 * Mathf.PI) * _distance;
        transform.position = _startPos + Vector3.up * xOffset;

    }
}
