using System.Collections.Generic;
using UnityEngine;

using EventBus;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.MissionMap;
using Dragoraptor.Models;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Core
{
    public class GameProgress : IGameProgressStart, ICurrentLevelDescriptorHolder, IGameProgressCollector, IProgressDataHolder
    {

        private readonly ILevelMapView _levelsMapView;
        private readonly IEventBus _eventBus;
        private readonly ILevelLoader _levelLoader;
            
        private Campaign _campaign;
        private ProgressData _progressData;
        private LevelDescriptor _loadedLevel;

        private int _loadedLevelNumber;
        

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
            SelectLastAvailableLevel();
            _eventBus.Subscribe<LevelClickedSignal>(SelectLevel);
        }
        
        #endregion


        #region ICurrentLevelDescriptorHolder
        
        public LevelDescriptor GetCurrentLevel()
        {
            int selectedLevelNumber = _progressData.CurrentLevelNumber;
            if (selectedLevelNumber != _loadedLevelNumber && selectedLevelNumber > 0)
            {
                _loadedLevel = _levelLoader.GetLevelDescriptor(selectedLevelNumber);
                _loadedLevelNumber = selectedLevelNumber;
            }
            
            return _loadedLevel;
        }

        public bool IsLevelReady()
        {
            return _progressData.CurrentLevelNumber > 0;
        }
        
        #endregion
        
        
        private void SelectLevel(LevelClickedSignal signal)
        {
            if (signal.LevelNumber == _progressData.CurrentLevelNumber)
            {
                //_levelsMapView.ClearLevelSelection();
                //_selectedLevelNumber = 0;
            }
            else
            {
                LevelStatus status = _progressData.Levels[signal.LevelNumber - 1].Status;
                if (status == LevelStatus.Available || status == LevelStatus.Finished)
                {
                    _levelsMapView.ClearLevelSelection();
                    _levelsMapView.SetLevelSelected(signal.LevelNumber);
                    _progressData.CurrentLevelNumber = signal.LevelNumber;
                }
            }
            
        }
        
        private void PrepareProgressData()
        {
            _progressData = new ProgressData();
            for (int i = 0; i < _campaign.CampaignLevelDatas.Length; i++)
            {
                CampaignLevelData levelData = _campaign.CampaignLevelDatas[i];
                LevelProgressInfo levelProgressInfo = new LevelProgressInfo(i + 1);
                LevelStatus status = levelData.StartStatus;
                levelProgressInfo.Status = status;
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

        private void SelectLastAvailableLevel()
        {
            for (int i = _progressData.Levels.Count - 1; i >= 0; i--)
            {
                LevelStatus status = _progressData.Levels[i].Status;
                if (status == LevelStatus.Available || status == LevelStatus.Finished)
                {
                    _progressData.CurrentLevelNumber = i + 1;
                    _levelsMapView.ClearLevelSelection();
                    _levelsMapView.SetLevelSelected(_progressData.CurrentLevelNumber);
                    break;
                }
            }
        }

        
        #region IGameProgressCollector
        
        public void RegistrateHuntResults(IHuntResults results)
        {
            _progressData.HuntsTotal++;
            _progressData.TotalScore += results.TotalScore;
            _progressData.LastScore = results.TotalScore;

            LevelProgressInfo currentLevel = _progressData.Levels[_progressData.CurrentLevelNumber - 1];
            if (currentLevel.BestScore < results.TotalScore)
            {
                currentLevel.BestScore = results.TotalScore;
            }

            if (results.IsSucces)
            {
                if (_progressData.CurrentLevelNumber < _progressData.Levels.Count)
                {
                    int nextLevelNumber = _progressData.CurrentLevelNumber + 1;
                    LevelProgressInfo nextLevel = _progressData.Levels[nextLevelNumber - 1];
                    if (nextLevel.Status == LevelStatus.Hidden || nextLevel.Status == LevelStatus.NotAvailable)
                    {
                        nextLevel.Status = LevelStatus.Available;
                        _levelsMapView.SetLevelStatus(nextLevelNumber, LevelStatus.Available);
                    }
                }

                if (currentLevel.Status == LevelStatus.Available)
                {
                    currentLevel.Status = LevelStatus.Finished;
                    _levelsMapView.SetLevelStatus(_progressData.CurrentLevelNumber, LevelStatus.Finished);
                    _progressData.CompletedLevels++;
                }
            }
        }
        
        #endregion


        #region IProgressDataHolder

        public ProgressData GetProgressData()
        {
            return _progressData;
        }

        #endregion
        
    }
}