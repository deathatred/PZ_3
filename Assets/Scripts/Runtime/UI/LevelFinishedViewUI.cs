using System;
using System.Collections.Generic;
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
    }
    private void UnsubscribeToEvents()
    {
        _nextButton.onClick.RemoveListener(NextClicked);
        _menuButton.onClick.RemoveListener(MenuClicked);
    }
    private void NextClicked()
    {

    }
    private void MenuClicked()
    {
        GameEventBus.MenuClicked();
    }
}
