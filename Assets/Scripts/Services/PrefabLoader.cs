using UnityEngine;
using Dragoraptor.Interfaces;


namespace Dragoraptor 
{ 
    public class PrefabLoader : IPrefabLoader
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