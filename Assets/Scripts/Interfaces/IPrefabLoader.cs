using UnityEngine;


namespace Dragoraptor.Interfaces
{
    public interface IPrefabLoader
    {
        GameObject GetPrefab(string prefabID);
    }
}