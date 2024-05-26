using UnityEngine;
using Dragoraptor.Interfaces;
using ObjPool;


namespace Dragoraptor 
{ 
    public class PrefabLoader : IPrefabLoader, IPoolPrefabLoader
    {
        public GameObject GetPrefab(string prefabID)
        {
            GameObject go = null;

            if (PrefabPaths.Paths.ContainsKey(prefabID))
            {
                go = Resources.Load<GameObject>(PrefabPaths.Paths[prefabID]);
            }
            return go;
        }

    }
}