using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        DontDestroyOnLoad(gameObject);
    }
    public static void EnablePlayerInputActions()
    {
        _playerInput.Player.Enable();
    }
    public static void DisablePlayerInputActions()
    {
        _playerInput.Player.Disable();
    }
    public static void SubscribeToPlayerInputActions(PlayerInput.IPlayerActions input)
    {
        _playerInput.Player.SetCallbacks(input);
    }
    public static void UnsubscribeToPlayerInputActions(PlayerInput.IPlayerActions input)
    {
        _playerInput.Player.RemoveCallbacks(input);
    }
}
