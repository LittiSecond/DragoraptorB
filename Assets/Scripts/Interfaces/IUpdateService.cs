namespace Dragoraptor.Interfaces
{
    public interface IUpdateService
    {
        void AddToUpdate(IExecutable executable);
        void RemoveFromUpdate(IExecutable executable);
    }
}