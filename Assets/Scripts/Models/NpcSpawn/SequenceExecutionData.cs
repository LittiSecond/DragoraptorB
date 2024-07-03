namespace Dragoraptor.Models.NpcSpawn
{
    public class SequenceExecutionData
    {
        //public string[] PrefabIds;
        public int CyclesQuantity = 1;
        public float StartDelay;
        public float BetweenDelay;
        public bool IsRandomized;
        public float Duration;
        public SpawnData[] SpawnDatas;

        public float NextTime;
        public float SequenceStartTime;
        public int CyclesCounter;
        public int NextDataIndex;
        public bool IsFinished;
    }
}