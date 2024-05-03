using Dragoraptor.Interfaces;

namespace Dragoraptor
{
    public class VictoryPossibilityStump : IVictoryPossibilityHolder
    {

        private bool _isVictory = false;
        
        // IVictoryPossibilityHolder
        public bool IsVictory
        {
            get
            {
                _isVictory = !_isVictory;
                return _isVictory;
            }
        }
        
        
    }
}