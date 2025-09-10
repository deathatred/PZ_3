using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSingleCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _levelPreview;
    [SerializeField] private Image[] _starsImages;
    [SerializeField] private Button _levelButton;


    public async UniTask Init(InfoLevelSO levelInfoSO)
    {
        _levelText.text = $"Level {levelInfoSO.LevelNumber}";
        _levelPreview.sprite = levelInfoSO.LevelPreview;
        int starsCount = await LevelsProgress.GetStarsAsync(levelInfoSO.LevelNumber);
        for (int i = 0; i < starsCount; i++)
        {
            _starsImages[i].color = Color.yellow;
        }
        _levelButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GetCurrentLevelService().ChangeLevelAsync(levelInfoSO.LevelNumber).Forget();
            GameEventBus.LevelChosen();
        });
    }
}
