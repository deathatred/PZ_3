using System.Diagnostics;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Debug = UnityEngine.Debug;

public class FinishedState : IGameState
{
    private GameManager _manager;
    public void Enter(GameManager manager)
    {
        _manager = manager;
        Stars stars = CountStars();
        FinishTheLevelAfterDelay(stars).Forget();
    }

    public void Exit()
    {
    }
    private Stars CountStars()
    {
        InfoLevelSO levelInfo = GameManager.Instance.CurrentLevel.GetLevelInfoSO();
        int remaining = _manager.GetRemainingBullets();
        if (remaining >= levelInfo.BulletsForThreeStars)
        {
            LevelsProgress.SaveStars(_manager.CurrentLevelIndex, (int)Stars.Three);
            levelInfo.StarsGained = (int)Stars.Three;
            return Stars.Three;      
        }
        else if (remaining >= levelInfo.BulletsForTwoStars)
        {
            LevelsProgress.SaveStars(_manager.CurrentLevelIndex, (int)Stars.Two);
            levelInfo.StarsGained = (int)Stars.Two;
            return Stars.Two;
        }
        else
        {
            LevelsProgress.SaveStars(_manager.CurrentLevelIndex, (int)Stars.One);
            levelInfo.StarsGained = (int)Stars.One;
            return Stars.One;
        }
    }
    private async UniTask FinishTheLevelAfterDelay(Stars stars)
    {
        await UniTask.Delay(5000);
      
        GameEventBus.LevelFinished(stars);
    }
}
