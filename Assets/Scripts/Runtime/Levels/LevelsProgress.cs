using UnityEngine;

public class LevelsProgress : MonoBehaviour
{
    private const string StarsKey = "Level_{0}_Stars";
    public static void SaveStars(int levelIndex, int stars)
    {
        int currentStars = GetStars(levelIndex);
        if (stars > currentStars)
        {
            string key = string.Format(StarsKey, levelIndex);
            PlayerPrefs.SetInt(key, stars);
            PlayerPrefs.Save(); 
        }
    }

    public static int GetStars(int levelIndex)
    {
        string key = string.Format(StarsKey, levelIndex);
        return PlayerPrefs.GetInt(key, 0); 
    }
}
