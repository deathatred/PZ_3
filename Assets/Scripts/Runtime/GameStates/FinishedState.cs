using Cysharp.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Debug = UnityEngine.Debug;

public class FinishedState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        Stars stars = CountStars();
        FinishTheLevelAfterDelayAsync(stars).Forget();
    }

    public void Exit()
    {
    }
    private Stars CountStars()
    {
        ILevelService levelService = _manager.GetCurrentLevelService();
        InfoLevelSO levelInfo = levelService.CurrentLevel.GetLevelInfoSO();
        var playerShooting = _manager.GetPlayer().GetComponent<PlayerShooting>();
        int remaining = playerShooting.GetBullets();
        if (remaining >= levelInfo.BulletsForThreeStars)
        {
            LevelsProgress.SaveStars(levelService.CurrentLevelIndex, Stars.Three);
            levelInfo.StarsGained = (int)Stars.Three;
            return Stars.Three;      
        }
        else if (remaining >= levelInfo.BulletsForTwoStars)
        {
            LevelsProgress.SaveStars(levelService.CurrentLevelIndex, Stars.Two);
            levelInfo.StarsGained = (int)Stars.Two;
            return Stars.Two;
        }
        else
        {
            LevelsProgress.SaveStars(levelService.CurrentLevelIndex, Stars.One);
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
