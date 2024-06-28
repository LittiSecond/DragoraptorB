using UnityEngine;

namespace Dragoraptor.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCDVESettings", menuName = "Resources/CharDamagedVisualEffectSettings")]
    public class CharDamagedVisualEffectSettings : ScriptableObject
    {
        public string PrefabID = string.Empty;
        [Range(0.0f, 1.0f)]
        public float MaxAlpha = 0.9f;
        [Range(0.0f, 1.0f)]
        public float MinAlpha = 0.4f;
        public float UpTime = 0.2f;
        public float DownTime = 0.5f;
    }
}