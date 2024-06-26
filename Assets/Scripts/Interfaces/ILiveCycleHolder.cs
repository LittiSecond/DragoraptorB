namespace Dragoraptor.Interfaces
{
    public interface ILiveCycleHolder
    {
        void AddExecutable(IExecutable executable);
        void AddActivatable(IActivatable activatable);
        void AddCleanable(ICleanable cleanable);
    }
}