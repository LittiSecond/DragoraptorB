using System;


namespace Dragoraptor.Interfaces.Character
{
    public interface IPlayerHealth : IHealthEndHolder
    {
        void ResetHealth();
    }
}