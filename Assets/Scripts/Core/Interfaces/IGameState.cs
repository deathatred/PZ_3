using UnityEngine;

public interface IGameState
{
    void Enter(GameManager manager);
    void Exit();
}
