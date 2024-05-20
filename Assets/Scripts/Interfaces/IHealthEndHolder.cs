using System;


namespace Dragoraptor.Interfaces
{
    public interface IHealthEndHolder
    {
        event Action OnHealthEnd;
    }
}