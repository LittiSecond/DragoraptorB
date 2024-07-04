using UnityEngine;


namespace Dragoraptor.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewWay", menuName = "Resources/NpcDataWay")]
    public class NpcDataWay : NpcData
    {
        public bool IsCyclic;
        public bool IsRelativeStartPosition;
        public Vector2[] Way;
    }
}