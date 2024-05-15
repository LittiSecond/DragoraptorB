using System;


namespace Dragoraptor.Interfaces.Character
{
    public interface IObservableResource
    {
        event Action<float> OnMaxValueChanged;
        event Action<float> OnValueChanged;
        float MaxValue { get; }
        float Value { get; }
    }
}