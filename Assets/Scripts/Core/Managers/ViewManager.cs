using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [SerializeField] private List<Canvas> _views = new();
    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        ChangeCanvas(0);
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    public void ChangeCanvas(int id)
    {
        if (id > _views.Count)
        {
            Debug.LogError($"This canvas id {id}, does not exist");
            return;
        }
        foreach (Canvas canvas in _views)
        {
            canvas.enabled = false;
        }
        _views[id].enabled = true;
        //GameEventBus.CanvasChange(id);
    }
    private void SubscribeToEvents()
    {
        GameEventBus.OnPlayClicked += GameEventBusOnPlayClicked;
        GameEventBus.OnLevelsClicked += GameEventBusOnLevelsClicked;
        GameEventBus.OnMenuClicked += GameEventBusOnMenuClicked;
        GameEventBus.OnBackClicked += GameEventBusOnBackClicked;
        GameEventBus.OnLevelFinished += GameEventBusOnLevelFinished;
        GameEventBus.OnLevelLoaded += GameEventBus_OnLevelLoaded;
        GameEventBus.OnNextClicked += GameEventBus_OnNextClicked;
    }
    private void UnsubscribeFromEvents()
    {
        GameEventBus.OnPlayClicked -= GameEventBusOnPlayClicked;
        GameEventBus.OnLevelsClicked -= GameEventBusOnLevelsClicked;
        GameEventBus.OnMenuClicked -= GameEventBusOnMenuClicked;
        GameEventBus.OnBackClicked -= GameEventBusOnBackClicked;
        GameEventBus.OnLevelFinished -= GameEventBusOnLevelFinished;
        GameEventBus.OnLevelLoaded -= GameEventBus_OnLevelLoaded;
        GameEventBus.OnNextClicked -= GameEventBus_OnNextClicked;
    }
    private void GameEventBusOnPlayClicked()
    {
        ChangeCanvas(1);
    }
    private void GameEventBus_OnLevelLoaded()
    {
        ChangeCanvas(1);
    }
    private void GameEventBus_OnNextClicked()
    {
        ChangeCanvas(4);
    }
    private void GameEventBusOnLevelsClicked()
    {
        ChangeCanvas(2);
    }
    private void GameEventBusOnMenuClicked()
    {
        ChangeCanvas(0);
    }
    private void GameEventBusOnBackClicked()
    {
        ChangeCanvas(0);
    }
    private void GameEventBusOnLevelFinished(Stars stars)
    {
        ChangeCanvas(3);
    }
}
