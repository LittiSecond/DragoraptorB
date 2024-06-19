using System.Collections.Generic;


namespace Dragoraptor.Models
{
    public class ProgressData
    {
        public List<LevelProgressInfo> Levels;
        public int CompletedLevels;
        public int HuntsTotal;
        public int TotalScore;
        public int LastScore;
        public int CurrentLevelNumber;

        
        public ProgressData()
        {
            Levels = new List<LevelProgressInfo>();
        }


        public void Clear()
        {
            Levels.Clear();
            CompletedLevels = 0;
            HuntsTotal = 0;
            TotalScore = 0;
            LastScore = 0;
            CurrentLevelNumber = 0;
        }
    }
}