using UnityEngine;

public class PlayerInputHandler : MonoBehaviour, PlayerInput.IPlayerActions
{
    public static PlayerInputHandler Instance {  get; private set; }
    public bool ShootPressed { get; private set;}
    private void Awake()
    {
        InitSingleton();
    }
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
            ShootPressed = true;
        }
        if (context.canceled)
        {
            ShootPressed = false;
        }
    }
    private void InitSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
