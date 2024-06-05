using Dragoraptor.Models;


namespace Dragoraptor.Interfaces.Character
{
    public interface IItemCollector
    {
        bool PickUp(PickableResource resource);
    }
}