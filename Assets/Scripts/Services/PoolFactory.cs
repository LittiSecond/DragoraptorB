using System.Collections.Generic;
using UnityEngine;

using VContainer;

using ObjPool;
using Dragoraptor.Interfaces;
using VContainer.Unity;


namespace Dragoraptor
{
    public class PoolFactory : IPoolFactory
    {

        private Dictionary<string, GameObject> _prefabCollection;
        private IObjectResolver _objectResolver;
        private IPrefabLoader _prefabLoader;
        
        

        public PoolFactory(IObjectResolver resolver, IPrefabLoader prefabLoader)
        {
            _objectResolver = resolver;
            _prefabLoader = prefabLoader;
            _prefabCollection = new Dictionary<string, GameObject>();
        }
        
        #region IPoolFactory
        
        public PooledObject CreateObject(string type)
        {
            PooledObject newObject = null;

            GameObject prefab;
            if (!_prefabCollection.TryGetValue(type, out prefab))
            {
                prefab = _prefabLoader.GetPrefab(type);
                _prefabCollection.Add(type, prefab);
            }

            if (prefab != null)
            {
                GameObject newGO = _objectResolver.Instantiate(prefab);
                newObject = newGO.GetComponent<PooledObject>();
            }

            return newObject;
        }
        
        #endregion
        
    }
}