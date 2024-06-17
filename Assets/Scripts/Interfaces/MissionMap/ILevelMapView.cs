using UnityEngine;


namespace Dragoraptor.Interfaces.MissionMap
{
    public interface ILevelMapView
    {
        void CreateLevel(int levelNumber, Vector2 position, LevelStatus startStatus);
        void SetLevelStatus(int levelNumber, LevelStatus newStatus);
        void SetLevelSelected(int levelNumber);
        void ClearLevelSelection();
    }
}