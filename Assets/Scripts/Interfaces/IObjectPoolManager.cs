namespace Dragoraptor.Interfaces
{
    public interface IObjectPoolManager
    {
        void PreparePool();
        void ReturnAllToPool();
        void ClearPool();
    }
}