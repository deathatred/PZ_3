using UnityEngine;

public class ShootingState : IGameState
{
    public void Enter(GameManager manager)
    {
        Debug.Log("Entered Shooting State");
    }

    public void Exit()
    {
        GameEventBus.ShootingEnded();
        Debug.Log("Exited Shooting State");
    }
}
