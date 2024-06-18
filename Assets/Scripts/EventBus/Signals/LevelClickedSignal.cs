namespace EventBus
{
    public class LevelClickedSignal
    {
        public int LevelNumber { get; private set; }

        public LevelClickedSignal(int levelNumber)
        {
            LevelNumber = levelNumber;
        }
    }
}