using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelsListScrollView : MonoBehaviour
{
    [SerializeField] private LevelSingleCell _levelSingleCellPrefab;
    [SerializeField] private GameObject _content;

    
    public void Init(List<Level> levelsList)
    {
        for (int i = _content.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(_content.transform.GetChild(i).gameObject);
        }
        foreach (Level level in levelsList)
        {
            InfoLevelSO levelInfoSO = level.GetLevelInfoSO();
            LevelSingleCell levelSingleCell = 
                GameObject.Instantiate(_levelSingleCellPrefab, _content.transform);
            levelSingleCell.Init(levelInfoSO);
        }
    }
}
