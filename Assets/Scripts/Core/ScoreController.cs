using System;
using Dragoraptor.Interfaces.Score;


namespace Dragoraptor.Core
{
    public class ScoreController : IScoreSource, IScoreCollector, IScoreManager
    {

        private int _score;


        #region IScoreSource
        
        public event Action<int> OnScoreChanged;
        
        public int GetScore()
        {
            return _score;
        }
        
        #endregion


        #region IScoreCollector
        
        public void AddScore(int amount)
        {
            if (amount > 0)
            {
                _score += amount;
                OnScoreChanged?.Invoke(_score);
            }
        }
        
        #endregion


        #region IScoreManager
        
        public void ClearScore()
        {
            _score = 0;
            OnScoreChanged?.Invoke(_score);
        }
        
        #endregion
        
    }
}