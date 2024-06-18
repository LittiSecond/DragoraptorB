using UnityEngine;

using EventBus;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.MissionMap;


namespace Dragoraptor.Core
{
    public class GameProgress : IGameProgressStart
    {


        private ILevelMapView _levelsMap;
        private IEventBus _eventBus;

        private int _selectedLevel;

        public GameProgress(ILevelMapView levelMapView, IEventBus eventBus)
        {
            _levelsMap = levelMapView;
            _eventBus = eventBus;
        }
        
        
        #region IGameProgressStart
        
        public void InitializeGameProgress()
        {
            _levelsMap.CreateLevel(1, new Vector2(0.25f, 0.25f), LevelStatus.NotAvailable);
            _levelsMap.CreateLevel(2, new Vector2(0.40f, 0.40f), LevelStatus.Available);
            _levelsMap.CreateLevel(3, new Vector2(0.20f, 0.75f), LevelStatus.Finished);
            _levelsMap.CreateLevel(4, new Vector2(0.60f, 0.60f), LevelStatus.Hidden);
            //_levelsMap.SetLevelSelected(2);
            _selectedLevel = 0;
            _eventBus.Subscribe<LevelClickedSignal>(LevelClicked);
        }
        
        #endregion

        
        
        private void LevelClicked(LevelClickedSignal signal)
        {
            if (signal.LevelNumber == _selectedLevel)
            {
                _levelsMap.ClearLevelSelection();
                _selectedLevel = 0;
            }
            else
            {
                _levelsMap.ClearLevelSelection();
                _levelsMap.SetLevelSelected(signal.LevelNumber);
                _selectedLevel = signal.LevelNumber;
            }
            
        }
        
        
    }
}