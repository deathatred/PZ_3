using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerSpawnService
{
    private Transform _player;
    private Vector3 _playerDefaultSpawnPoint = new Vector3(0f, 0.6f, 0f);

    public PlayerSpawnService(Transform player)
    {
        _player = player;
    }
    public void ResetPlayerTransform()
    {
        _player.position = _playerDefaultSpawnPoint;
        _player.rotation = Quaternion.Euler(0f, 90f, 0f);
    }
}
