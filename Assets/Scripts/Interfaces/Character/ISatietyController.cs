using System;


namespace Dragoraptor.Interfaces.Character
{
    public interface ISatietyController
    {
        void ResetSatiety();
        void SetVictorySatiety(float satietyRelativeMax);
    }
}