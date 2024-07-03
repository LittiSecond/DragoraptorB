using UnityEngine;

using Dragoraptor.Models.NpcSpawn;


namespace Dragoraptor.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewSequence", menuName = "Resources/NewSpawnSequence")]
    public class SpawnSequence : ScriptableObject
    {
        //public string[] PrefabIds;
        public int Quantity = 1;
        public float StartDelay;
        public float BetweenDelay;
        public bool IsRandomized;
        public float Duration;
        public SpawnData[] SpawnDatas;
    }
}