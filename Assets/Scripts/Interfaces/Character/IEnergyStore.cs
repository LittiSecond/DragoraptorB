namespace Dragoraptor.Interfaces.Character
{
    public interface IEnergyStore
    {
        bool Spend(float amount);
        void AddEnergy(float amount);
    }
}