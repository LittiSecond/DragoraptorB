using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.MissionMap;
using UnityEngine;

namespace Dragoraptor.Core
{
    public class GameProgress : IGameProgressStart
    {


        private ILevelMapView _levelsMap;


        public GameProgress(ILevelMapView levelMapView)
        {
            _levelsMap = levelMapView;
        }
        
        
        #region IGameProgressStart
        
        public void InitializeGameProgress()
        {
            _levelsMap.CreateLevel(1, new Vector2(0.25f, 0.25f), LevelStatus.NotAvailable);
            _levelsMap.CreateLevel(2, new Vector2(0.40f, 0.40f), LevelStatus.Available);
            _levelsMap.CreateLevel(3, new Vector2(0.20f, 0.75f), LevelStatus.Finished);
            _levelsMap.CreateLevel(4, new Vector2(0.60f, 0.60f), LevelStatus.Hidden);
            _levelsMap.SetLevelSelected(2);
        }
        
        #endregion
        
    }
}