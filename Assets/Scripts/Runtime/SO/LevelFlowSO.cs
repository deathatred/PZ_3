using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelFlow")]
public class LevelFlowSO : ScriptableObject
{
    public List<GameState> GameStateList;
}

