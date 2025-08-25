using UnityEngine;

public class MovingObstacleRotatingAround : MovingObstacle
{
    [SerializeField] private Transform _centre;
    protected override void Move()
    {
        if (_centre != null)
        {
            transform.RotateAround(_centre.position, Vector3.up, _moveSpeed *  Time.deltaTime);    
        }
    }
}
