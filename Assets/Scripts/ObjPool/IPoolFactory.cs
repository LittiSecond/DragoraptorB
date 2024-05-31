namespace ObjPool
{
    public interface IPoolFactory
    {
        PooledObject CreateObject(string type);
    }
}