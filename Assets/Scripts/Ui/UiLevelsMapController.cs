using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using EventBus;

using Dragoraptor.Interfaces.MissionMap;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class UiLevelsMapController : ILevelsMapController, ILevelMapView
    {

        private Dictionary<int, UiMissionIndicator> _missionsTable = new();

        private IUiFactory _factory;
        private IEventBus _eventBus;
        private VisualElement _mapRoot;
        private UiMissionIndicator _selectedMission;


        private bool _isVisualEnabled;


        public UiLevelsMapController(IUiFactory factory, IEventBus eventBus)
        {
            _factory = factory;
            _eventBus = eventBus;
        }
        
        
        #region ILevelsMapController
        
        public void SetMapRoot(VisualElement mapRootPanel)
        {
            _mapRoot = mapRootPanel;
            foreach (var missionKvp in _missionsTable)
            {
                missionKvp.Value.SetParent(_mapRoot);
            }

            _isVisualEnabled = true;
        }
        
        #endregion


        #region ILevelMapView
        
        public void CreateLevel(int levelNumber, Vector2 position, LevelStatus startStatus)
        {
            if (!_missionsTable.ContainsKey(levelNumber))
            {
                UiMissionIndicator newIndicator = new UiMissionIndicator(_factory, _eventBus,
                    levelNumber, position, startStatus);
                _missionsTable.Add(levelNumber, newIndicator);
                if (_isVisualEnabled)
                {
                    newIndicator.SetParent(_mapRoot);
                }
            }
        }

        public void SetLevelStatus(int levelNumber, LevelStatus newStatus)
        {
            if (_missionsTable.TryGetValue(levelNumber, out UiMissionIndicator mission))
            {
                mission.Status = newStatus;
            }
        }

        public void SetLevelSelected(int levelNumber)
        {
            if (_missionsTable.TryGetValue(levelNumber, out UiMissionIndicator mission))
            {
                ClearLevelSelection();
                mission.IsSelected = true;
                _selectedMission = mission;
            }
        }

        public void ClearLevelSelection()
        {
            if (_selectedMission != null)
            {
                _selectedMission.IsSelected = false;
                _selectedMission = null;
            }
        }
        
        #endregion
        
        
        
        
        
        
    }
}