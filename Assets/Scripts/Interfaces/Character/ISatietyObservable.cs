using System;


namespace Dragoraptor.Interfaces.Character
{
    public interface ISatietyObservable : IObservableResource
    {
        event Action<float> OnVictorySatietyChanged;
    }
}