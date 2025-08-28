using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSingleCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _levelPreview;
    [SerializeField] private Image[] _starsImages;

    public void Init(InfoLevelSO levelInfoSO)
    {
        _levelText.text = $"Level {levelInfoSO.LevelNumber}";
        _levelPreview.sprite = levelInfoSO.LevelPreview;
        for (int i = 0; i < levelInfoSO.StarsGained; i++)
        {
            _starsImages[i].gameObject.SetActive(true); //change color to yellow
        }
    }
}
