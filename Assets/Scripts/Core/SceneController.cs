using UnityEngine;
using Dragoraptor.Interfaces;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Core
{
    public class SceneController : ISceneController
    {

        private GameObject _ground;
        private GameObject _backGround;
        private LevelDescriptor _currentLevel;
        private readonly ICurrentLevelDescriptorHolder _levelDescriptorHolder;

        private bool _isMainScreen;
        private bool _isLevelActive;
        private bool _isLevelCreated;


        public SceneController(ICurrentLevelDescriptorHolder currentLevelDescriptorHolder)
        {
            _levelDescriptorHolder = currentLevelDescriptorHolder;
        }
        
        
        
        #region ISceneController

        public void SetMainScreenScene()
        {
            if (_isLevelActive)
            {
                DeactivateLevel();
            }
            _isMainScreen = true;
        }

        public void BuildLevel()
        {
            if (_isMainScreen)
            {
                DeactivateMainScreen();
            }

            LevelDescriptor level = _levelDescriptorHolder.GetCurrentLevel();

            if (level == null)
            {
                Debug.LogError("SceneController-BuildLevel: currentLevel == null");
                return;
            }
            
            
            if ( (_currentLevel == null) || (_currentLevel.LevelId != level.LevelId))
            {
                DeactivateLevel();
                DestroyLevel();
            
                _currentLevel = level;
                CreateLevel();
            }
            else
            {
                if (!_isLevelActive)
                {
                    ActivateLevel();
                }
            }
        }

        public void RestartLevel()
        {
            if (_isLevelActive)
            {
                // TODO: to delete temporary objects to pools
                //Services.Instance.ObjectPool.ReturnAllToPool();
            }
        }

        #endregion
        
        
        private void DeactivateMainScreen()
        {
            _isMainScreen = false;
        }
        
        private void ActivateLevel()
        {
            if (!_isLevelActive)
            {
                _ground.SetActive(true);
                _backGround.SetActive(true);
                _isLevelActive = true;
            }
        }

        private void CreateLevel()
        {
            GameObject prefab = _currentLevel.BackgroundPrefab;
            _backGround = GameObject.Instantiate(prefab);
            prefab = _currentLevel.GroundPrefab;
            _ground = GameObject.Instantiate(prefab);
            _isLevelCreated = true;
            _isLevelActive = true;
        }

        private void DestroyLevel()
        {
            if (_isLevelCreated)
            {
                GameObject.Destroy(_backGround);
                _backGround = null;
                GameObject.Destroy(_ground);
                _ground = null;
                _isLevelCreated = false;
            }
        }

        private void DeactivateLevel()
        {
            if (_isLevelActive)
            {
                // TODO: to delete temporary objects to pools
                //Services.Instance.ObjectPool.ReturnAllToPool();
                _ground.SetActive(false);
                _backGround.SetActive(false);
                _isLevelActive = false;
            }
        }
        

    }
}