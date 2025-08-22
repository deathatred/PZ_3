using UnityEngine;

public class PlayerInputHandler : MonoBehaviour, PlayerInput.IPlayerActions
{
    private void Start()
    {
        InputManager.EnablePlayerInputActions();
        InputManager.SubscribeToPlayerInputActions(this);
    }
    private void OnDisable()
    {
        InputManager.DisablePlayerInputActions();
        InputManager.UnsubscribeToPlayerInputActions(this);
    }
    public void OnShoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print("shoot");
        }
    }
}
