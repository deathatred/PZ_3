using UnityEngine;

public class ShootingState : IGameState
{
    public void Enter(GameManager manager)
    {
    }

    public void Exit()
    {
        GameEventBus.ShootingEnded();
    }
}
