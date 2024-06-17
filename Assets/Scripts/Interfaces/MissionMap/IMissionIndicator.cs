namespace Dragoraptor.Interfaces.MissionMap
{
    public interface IMissionIndicator
    {
        int Number { get; }
        LevelStatus Status { set; }
        bool IsSelected { set; }
    }
}