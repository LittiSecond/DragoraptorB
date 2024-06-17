using UnityEngine.UIElements;

namespace Dragoraptor.Interfaces
{
    public interface IUiPrefabLoader
    {
        VisualTreeAsset GetUiPrefab(string prefabID);
    }
}