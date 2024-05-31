using UnityEngine;
using ObjPool;
using Dragoraptor.Interfaces;


namespace Dragoraptor.Core
{
    public class ObjectPoolManager : IObjectPool, IObjectPoolManager
    {

        private IPoolPrefabLoader _loaderForPool;
        private IPoolFactory _factory;
        private ObjectPool2 _pool;



        public ObjectPoolManager(IPoolPrefabLoader poolPrefabLoader, IPoolFactory factory)
        {
            _loaderForPool = poolPrefabLoader;
            _factory = factory;
            //TODO: refactoring object pool
            _pool = new ObjectPool2(factory);
            _pool.SetPrefabLoader(_loaderForPool);
        }
        
        
        #region IObjectPool
        
        public PooledObject GetObjectOfType(string type)
        {
            return _pool.GetObjectOfType(type);
        }
        
        #endregion


        #region IObjectPoolManager
        
        public void PreparePool()
        {
            Debug.Log("ObjectPoolManager->PreparePool:");
        }

        public void ReturnAllToPool()
        {
            Debug.Log("ObjectPoolManager->ReturnAllToPool:");
            _pool.ReturnAllToPool();
        }

        public void ClearPool()
        {
            Debug.Log("ObjectPoolManager->ClearPool:");
            _pool.ReturnAllToPool();
            _pool.Clear();
        }
        
        #endregion
        
        
    }
}