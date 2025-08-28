using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo")]
public class InfoLevelSO : ScriptableObject
{
    public int LevelNumber;
    public Sprite LevelPreview;
    public int StarsGained;
}
