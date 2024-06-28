using System;

namespace Dragoraptor.Interfaces
{
    public interface IPlayerDamaged
    {
        event Action<float> OnDamaged;
    }
}