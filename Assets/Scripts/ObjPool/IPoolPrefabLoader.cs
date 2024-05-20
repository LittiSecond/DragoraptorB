using UnityEngine;


namespace ObjPool
{
    public interface IPoolPrefabLoader
    {
        GameObject GetPrefab(string type);
    }
}