using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinishedViewUI : MonoBehaviour
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private List<Image> _starsImages;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnsubscribeToEvents();
    }
    private void SubscribeToEvents()
    {
        _nextButton.onClick.AddListener(NextClicked);
        _menuButton.onClick.AddListener(MenuClicked);
        GameEventBus.OnLevelFinished += GameEventBusOnLevelFinished;
        GameEventBus.OnLevelLoaded += GameEventBusOnLevelLoaded;
    }
    private void UnsubscribeToEvents()
    {
        _nextButton.onClick.RemoveListener(NextClicked);
        _menuButton.onClick.RemoveListener(MenuClicked);
        GameEventBus.OnLevelFinished -= GameEventBusOnLevelFinished;
    }
    private void NextClicked()
    {
        GameEventBus.NextClicked();
    }
    private void MenuClicked()
    {
        GameEventBus.MenuClicked();
    }

    private void GameEventBusOnLevelFinished(Stars stars)
    {
        for (int i = 0; i < (int)stars; i++)
        {
            _starsImages[i].color = Color.yellow;
        }
    }
    private void GameEventBusOnLevelLoaded()
    {
        foreach (var starImage in _starsImages)
        {
            starImage.color = Color.white;
        }
    }
}
