using Game.Utilities;
using NaughtyAttributes;

namespace Game.Managers
{
    public class ScoreManager : StaticInstance<ScoreManager>
    {
        [ShowNonSerializedField]
        int _currentScore = 0;

        public void OnScoreChanged(int score)
        {
            _currentScore += score;
            UIManager.Instance.UpdateScoreText(_currentScore);
        }
    }
}
