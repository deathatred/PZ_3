using UnityEngine;

public class ShootingState : IGameState
{
    public void Enter()
    {
    }

    public void Exit()
    {
        GameEventBus.ShootingEnded();
    }
}
