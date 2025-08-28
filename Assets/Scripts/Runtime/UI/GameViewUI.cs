using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameViewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bulletsText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Button _menuButton;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        SetBulletsText(10);
        SetLevelText(1);
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _menuButton.onClick.AddListener(MenuPressed);
    }
    private void UnsubscribeFromEvents()
    {
        _menuButton.onClick.RemoveListener(MenuPressed);
    }
    private void MenuPressed()
    {
        GameEventBus.MenuClicked();
    }
    private void SetBulletsText(int amount)
    {
        _bulletsText.text = amount.ToString();
    }
    private void SetLevelText(int levelIndex)
    {
        _levelText.text = $"Level: {levelIndex}";
    }
}
