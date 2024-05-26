using ObjPool;


namespace Dragoraptor.Interfaces
{
    public interface IObjectPool
    {
        PooledObject GetObjectOfType(string type);
    }
}