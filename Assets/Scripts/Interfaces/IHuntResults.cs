namespace Dragoraptor.Interfaces
{
    public interface IHuntResults
    {
        bool IsAlive { get; }
        bool IsSatietyCompleted { get; }
        bool IsSucces { get; }
        int BaseScore { get; }
        int TotalScore { get; }
        float CollectedSatiety { get; }
        float SatietyCondition { get; }
        float MaxSatiety { get; }
        float SatietyScoreMultipler { get; }
        float VictoryScoreMultipler { get; }
    }
}