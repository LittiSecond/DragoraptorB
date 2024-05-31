using System;


namespace Dragoraptor.Interfaces.Score
{
    public interface IScoreSource
    {
        event Action<int> OnScoreChanged;
        int GetScore();
    }
}