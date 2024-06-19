using Dragoraptor.ScriptableObjects;

namespace Dragoraptor.Interfaces
{
    public interface ILevelLoader
    {
        void SetCampaign(Campaign campaign);
        LevelDescriptor GetLevelDescriptor(int levelNumber);
    }
}