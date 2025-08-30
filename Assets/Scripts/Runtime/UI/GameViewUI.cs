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
    public void Init(Level level)
    {
        InfoLevelSO levelInfo = level.GetLevelInfoSO();
        SetBulletsText(levelInfo.NumberOfBullets);
        SetLevelText(levelInfo.LevelNumber);
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _menuButton.onClick.AddListener(MenuPressed);
        GameEventBus.OnBulletShot += GameEventBusOnBulletShot;
        GameEventBus.OnLevelLoaded += GameEventBusOnLevelLoaded;
    }
    private void UnsubscribeFromEvents()
    {
        _menuButton.onClick.RemoveListener(MenuPressed);
        GameEventBus.OnBulletShot -= GameEventBusOnBulletShot;
        GameEventBus.OnLevelLoaded -= GameEventBusOnLevelLoaded;
    }
    private void GameEventBusOnBulletShot(int bullets)
    {
        _bulletsText.text = bullets.ToString();
    }
    private void MenuPressed()
    {
        GameEventBus.MenuClicked();
    }
    private void GameEventBusOnLevelLoaded(Level level)
    {
        Init(level);
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
