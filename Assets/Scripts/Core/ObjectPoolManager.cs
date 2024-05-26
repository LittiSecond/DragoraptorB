using UnityEngine;
using ObjPool;
using Dragoraptor.Interfaces;


namespace Dragoraptor.Core
{
    public class ObjectPoolManager : IObjectPool, IObjectPoolManager
    {

        private IPoolPrefabLoader _loaderForPool;
        private ObjectPool2 _pool;



        public ObjectPoolManager(IPoolPrefabLoader poolPrefabLoader)
        {
            _loaderForPool = poolPrefabLoader;
            _pool = new ObjectPool2();
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