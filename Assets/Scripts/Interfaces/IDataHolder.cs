using Dragoraptor.ScriptableObjects;

namespace Interfaces
{
    public interface IDataHolder
    {
        GamePlaySettings GetGamePlaySettings();
        Campaign GetCampaign();
    }
}