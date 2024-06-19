using System.Collections.Generic;
using UnityEngine;

using EventBus;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.MissionMap;
using Dragoraptor.Models;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Core
{
    public class GameProgress : IGameProgressStart, ICurrentLevelDescriptorHolder
    {


        private ILevelMapView _levelsMapView;
        private IEventBus _eventBus;
        private ILevelLoader _levelLoader;
            
        private Campaign _campaign;
        private ProgressData _progressData;
        private LevelDescriptor _loadedLevel;


        private int _loadedLevelNumber;
        private int _selectedLevelNumber;
        

        public GameProgress(ILevelMapView levelMapView, IEventBus eventBus, IDataHolder dataHolder)
        {
            _levelsMapView = levelMapView;
            _eventBus = eventBus;
            _campaign = dataHolder.GetCampaign();
            _levelLoader = new LevelLoader();
        }
        
        
        #region IGameProgressStart
        
        public void InitializeGameProgress()
        {
            _levelLoader.SetCampaign(_campaign);
            PrepareProgressData();
            CreateLevelsOnMap();
            _selectedLevelNumber = 0;
            _eventBus.Subscribe<LevelClickedSignal>(SelectLevel);
        }
        
        #endregion


        #region ICurrentLevelDescriptorHolder
        
        public LevelDescriptor GetCurrentLevel()
        {
            if (_selectedLevelNumber != _loadedLevelNumber && _selectedLevelNumber > 0)
            {
                _loadedLevel = _levelLoader.GetLevelDescriptor(_selectedLevelNumber);
                _loadedLevelNumber = _selectedLevelNumber;
            }
            
            return _loadedLevel;
        }

        public bool IsLevelReady()
        {
            return _selectedLevelNumber > 0;
        }
        
        #endregion
        
        
        private void SelectLevel(LevelClickedSignal signal)
        {
            if (signal.LevelNumber == _selectedLevelNumber)
            {
                _levelsMapView.ClearLevelSelection();
                _selectedLevelNumber = 0;
            }
            else
            {
                _levelsMapView.ClearLevelSelection();
                _levelsMapView.SetLevelSelected(signal.LevelNumber);
                _selectedLevelNumber = signal.LevelNumber;
            }
            
        }
        
        private void PrepareProgressData()
        {
            _progressData = new ProgressData();
            for (int i = 0; i < _campaign.CampaignLevelDatas.Length; i++)
            {
                CampaignLevelData levelData = _campaign.CampaignLevelDatas[i];
                LevelProgressInfo levelProgressInfo = new LevelProgressInfo(i + 1);
                levelProgressInfo.Status = levelData.StartStatus;
                _progressData.Levels.Add(levelProgressInfo);
            }
        }
        
        private void UpdateUiLevelMap()
        {
            if (_levelsMapView == null ) return;

            List<LevelProgressInfo> levels = _progressData.Levels;
            foreach (var level in levels)
            {
                _levelsMapView.SetLevelStatus(level.LevelNumber, level.Status);
            }
        }

        private void CreateLevelsOnMap()
        {
            for (int i = 0; i < _campaign.CampaignLevelDatas.Length; i++)
            {
                CampaignLevelData levelData = _campaign.CampaignLevelDatas[i];
                _levelsMapView.CreateLevel(i + 1, levelData.PositionOnMap, levelData.StartStatus);
            }
        }
        
        
        
    }
}