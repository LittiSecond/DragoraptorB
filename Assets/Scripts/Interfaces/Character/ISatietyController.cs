using System;


namespace Dragoraptor.Interfaces.Character
{
    public interface ISatietyController
    {
        event Action OnVictorySatietyReached;
        event Action OnMaxSatietyReached;
        void ResetSatiety();
        void SetVictorySatiety(float satietyRelativeMax);
    }
}