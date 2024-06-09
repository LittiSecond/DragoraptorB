using Dragoraptor.Interfaces;

namespace Dragoraptor.Models
{
    public class HuntResults : IHuntResults
    {

        public bool IsAlive { get; set; }

        public bool IsSatietyCompleted { get; set; }

        public bool IsSucces { get; set; }

        public int BaseScore { get; set; }

        public float CollectedSatiety { get; set; }

        public float MaxSatiety { get; set; }

        public float SatietyCondition { get; set; }

        public float SatietyScoreMultipler { get; set; }

        public int TotalScore { get; set; }

        public float VictoryScoreMultipler { get; set; }

    }
}