using UnityEngine;

public class MenuState : IGameState
{
    public void Enter(GameManager manager)
    {
        Debug.Log("Entered menu state");
    }

    public void Exit()
    {
        Debug.Log("Exited menu state");
    }
}
