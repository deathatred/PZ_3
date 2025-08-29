using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _levelsButton;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _playButton.onClick.AddListener(PlayClicked);
        _levelsButton.onClick.AddListener(LevelsClicked);
    }
    private void UnsubscribeFromEvents()
    {
        _playButton.onClick.RemoveListener(PlayClicked);
        _levelsButton.onClick.RemoveListener(LevelsClicked);
    }
    private void PlayClicked()
    {
        GameEventBus.PlayClicked();
    }
    private void LevelsClicked()
    {
        GameEventBus.LevelsClicked();
    }

}
