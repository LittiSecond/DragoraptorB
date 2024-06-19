namespace Dragoraptor.Models
{
    public class LevelProgressInfo
    {
        private int _levelNumber;

        public int LevelNumber { get => _levelNumber; }
        public int BestScore { get; set; }
        public LevelStatus Status { get; set; }


        public LevelProgressInfo(int levelNumber)
        {
            _levelNumber = levelNumber;
        }
    }
}