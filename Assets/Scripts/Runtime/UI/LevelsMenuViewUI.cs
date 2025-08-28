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
        _levelListScrollView.Init(GameManager.Instance.GetLevelsList());
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _backButton.onClick.AddListener(BackClicked);
    }
    private void UnsubscribeFromEvents()
    {
        _backButton.onClick.RemoveListener(BackClicked);
    }
    private void BackClicked()
    {
        GameEventBus.BackClicked();
    }
}
