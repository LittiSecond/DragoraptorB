using System;

namespace Dragoraptor.Models.NpcSpawn
{
    [Serializable]
    public class SpawnChain
    {
        public int Quantity = 1;
        public float StartDelay;
        public float BetweenDelay;
        public bool IsRandomized;
        public float Duration;
        public SpawnData[] SpawnDatas;
    }
}