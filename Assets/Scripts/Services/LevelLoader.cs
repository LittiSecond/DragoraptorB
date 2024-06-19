using System.Collections.Generic;
using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor
{
    public class LevelLoader : ILevelLoader
    {

        private List<string> _levelPaths = new();
        
        private LevelDescriptor _loadedLevel;
        private int _loadedLevelNumber;

        private bool _haveCampaign;
        private bool _haveLevel;

        
        
        #region ILevelLoader
        
        public void SetCampaign(Campaign campaign)
        {
            _levelPaths.Clear();
            foreach (var levelData in campaign.CampaignLevelDatas)
            {
                _levelPaths.Add(levelData.LevelPath);
            }
            
            _haveCampaign = _levelPaths.Count > 0;
            UnloadLevel();
        }

        public LevelDescriptor GetLevelDescriptor(int levelNumber)
        {
            
            if (levelNumber <= 0 || levelNumber > _levelPaths.Count) return null;
            if (!_haveCampaign) return null;
            
            LevelDescriptor newLevel = null;

            if (_loadedLevelNumber > 0)
            {
                if (levelNumber == _loadedLevelNumber)
                {
                    newLevel = _loadedLevel;
                }
            }

            if (newLevel == null)
            {
                string path = CreateFullPath(levelNumber);
                newLevel = Resources.Load<LevelDescriptor>(path);

                if (newLevel)
                {
                    UnloadLevel();
                    _loadedLevel = newLevel;
                    _haveLevel = true;
                    _loadedLevelNumber = levelNumber;
                }
            }

            return newLevel;
        }
        
        #endregion
        
        
        private void UnloadLevel()
        {
            if (_haveLevel)
            {
                Resources.UnloadAsset(_loadedLevel);
                _loadedLevel = null;
                _haveLevel = false;
                _loadedLevelNumber = -1;
            }
        }

        private string CreateFullPath(int levelNumber)
        {
            return _levelPaths[levelNumber - 1];
        }
    }
}