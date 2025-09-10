using Cysharp.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class LevelsProgress : MonoBehaviour
{
    private const string StarsKey = "Level_{0}_Stars";
    public static void SaveStars(int levelIndex, Stars stars)
    {
        FirebaseFacade.SaveLevelStars(levelIndex, stars).Forget();

    }

    public static async UniTask<int> GetStarsAsync(int levelIndex)
    {
        return await FirebaseFacade.GetLevelStars(levelIndex);
   
    }
}
