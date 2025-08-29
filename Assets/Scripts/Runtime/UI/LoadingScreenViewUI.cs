using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreenViewUI : MonoBehaviour
{
    [SerializeField] private Image _loadingImage;

    private Canvas _loadingCanvas;
    private Tween _rotationTween;

    private void Start()
    {
        _loadingCanvas = GetComponent<Canvas>();
        _rotationTween = _loadingImage.rectTransform
            .DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear)
            .Pause();
    }

    private void Update()
    {
        if (_loadingCanvas.enabled && !_rotationTween.IsPlaying())
        {
            _rotationTween.Play();
        }
        else if (!_loadingCanvas.enabled && _rotationTween.IsPlaying())
        {
            _rotationTween.Pause();
        }
    }
}