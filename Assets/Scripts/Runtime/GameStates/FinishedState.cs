using Cysharp.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;
using Zenject;


public class FinishedState : IGameState
{
    private readonly PlayerShooting _playerShooting;
    private readonly ILevelService _levelService;

    [Inject]
    public FinishedState(PlayerShooting playerShooting, ILevelService levelService)
    {
        _playerShooting = playerShooting;
        _levelService = levelService;
    }
    public void Enter()
    {
        Stars stars = CountStars();
        FinishTheLevelAfterDelayAsync(stars).Forget();
    }

    public void Exit()
    {
    }
    private Stars CountStars()
    {
        InfoLevelSO levelInfo = _levelService.CurrentLevel.GetLevelInfoSO();
        int remaining = _playerShooting.GetBullets();
        if (remaining >= levelInfo.BulletsForThreeStars)
        {
            LevelsProgress.SaveStars(_levelService.CurrentLevelIndex, Stars.Three);
            levelInfo.StarsGained = (int)Stars.Three;
            return Stars.Three;      
        }
        else if (remaining >= levelInfo.BulletsForTwoStars)
        {
            LevelsProgress.SaveStars(_levelService.CurrentLevelIndex, Stars.Two);
            levelInfo.StarsGained = (int)Stars.Two;
            return Stars.Two;
        }
        else
        {
            LevelsProgress.SaveStars(_levelService.CurrentLevelIndex, Stars.One);
            levelInfo.StarsGained = (int)Stars.One;
            return Stars.One;
        }
    }
    private async UniTask FinishTheLevelAfterDelayAsync(Stars stars)
    {
        await UniTask.Delay(5000);

        GameEventBus.LevelFinished(stars);
    }
}
