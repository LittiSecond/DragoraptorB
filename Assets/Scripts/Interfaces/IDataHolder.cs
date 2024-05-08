using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Interfaces
{
    public interface IDataHolder
    {
        GamePlaySettings GetGamePlaySettings();
        Campaign GetCampaign();
    }
}