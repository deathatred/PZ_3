using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMenuViewUI : MonoBehaviour
{
    [SerializeField] private LevelsListScrollView _levelListScrollView;
    [SerializeField] private Button _backButton;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        InitComponents();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _backButton.onClick.AddListener(BackClicked);
        GameEventBus.OnLevelsClicked += InitComponents;
    }
    private void UnsubscribeFromEvents()
    {
        _backButton.onClick.RemoveListener(BackClicked);
        GameEventBus.OnLevelsClicked -= InitComponents;
    }
    private void BackClicked()
    {
        GameEventBus.BackClicked();
    }
    private void InitComponents()
    {
        _levelListScrollView.Init(GameManager.Instance.GetLevelsList());
    }
}
