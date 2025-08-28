using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelsListScrollView : MonoBehaviour
{
    [SerializeField] private LevelSingleCell _levelSingleCellPrefab;
    [SerializeField] private GameObject _content;

    public void Init(List<Level> levelsList)
    {
        foreach (Level level in levelsList)
        {
            InfoLevelSO levelInfoSO = level.GetLevelInfoSO();
            LevelSingleCell levelSingleCell = 
                GameObject.Instantiate(_levelSingleCellPrefab, _content.transform);
            levelSingleCell.Init(levelInfoSO);
        }
    }
}
