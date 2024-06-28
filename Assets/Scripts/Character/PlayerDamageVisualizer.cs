using UnityEngine;
using VContainer.Unity;

using Dragoraptor.Interfaces;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Character
{
    public class PlayerDamageVisualizer : ITickable
    {
        private readonly IPlayerDamaged _playerDamaged;
        private readonly IPrefabLoader _prefabLoader;
        private readonly ISceneGeometry _sceneGeometry;
        
        private enum FadeState { None, Up, Down};
        
        private SpriteRenderer _fadedImage;
        private CharDamagedVisualEffectSettings _visualSettings;
        
        private FadeState _state;
        private Color _currentColor;
        private float _timeCounter;
        private float _targetAlpha;
        private float _beginAlpha;
        

        private bool _isInitialized;


        public PlayerDamageVisualizer(IPlayerDamaged playerDamaged, 
            IPrefabLoader prefabLoader, 
            ISceneGeometry sceneGeometry, 
            IDataHolder dataHolder)
        {
            _playerDamaged = playerDamaged;
            _playerDamaged.OnDamaged += Damaged;
            _prefabLoader = prefabLoader;
            _sceneGeometry = sceneGeometry;
            _visualSettings = dataHolder.GetCharDmgVisualSettings();
        }


        private void Damaged(float amount)
        {
            if (!_isInitialized)
            {
                InitializeItself();
            }
            
            StartEffect(1.0f);
            //Debug.Log("PlayerDamageVisualizer->Damaged: amount = " + amount.ToString());
        }

        private void InitializeItself()
        {
            var prefab = _prefabLoader.GetPrefab(_visualSettings.PrefabID); 
            var fader = GameObject.Instantiate(prefab);
            _fadedImage = fader.GetComponent<SpriteRenderer>();
            _currentColor = _fadedImage.color;
            _state = FadeState.None;
            //_fadedImage.enabled = false;
            Rect visibleArea = _sceneGeometry.GetVisibleArea();
            Vector2 size = _fadedImage.size;
            var temp = fader.transform;
            Vector3 scale = temp.localScale;
            scale.x = visibleArea.width / size.x;
            scale.y = visibleArea.height / size.y;
            temp.localScale = scale;
            _isInitialized = true;
            
            //Debug.Log($"PlayerDamageVisualizer->InitializeItself: visibleArea = {visibleArea}; size = {size}");
        }
        
        #region ITickable

        public void Tick()
        {
            if (!_isInitialized) return;
            
            switch (_state)
            {
                case FadeState.Up:
                    UpAlpha();
                    break;
                case FadeState.Down:
                    DownAlpha();
                    break;
            }
        }

        #endregion
        
        
        private void UpAlpha()
        {
            _timeCounter += Time.deltaTime;
            float alpha = Mathf.Clamp( _timeCounter / _visualSettings.UpTime, 0, 1) * 
                (_targetAlpha - _beginAlpha) + _beginAlpha;
            _currentColor.a = alpha;
            _fadedImage.color = _currentColor;
            if (_timeCounter >= _visualSettings.UpTime)
            {
                _state = FadeState.Down;
                _timeCounter = 0;
            }
        }

        private void DownAlpha()
        {
            _timeCounter += Time.deltaTime;
            float alpha = Mathf.Clamp(1 - _timeCounter / _visualSettings.DownTime, 0, 1) * _targetAlpha;
            _currentColor.a = alpha;
            _fadedImage.color = _currentColor;
            if (_timeCounter >= _visualSettings.DownTime)
            {
                _state = FadeState.None;
                _fadedImage.enabled = false;
                _timeCounter = 0;
            }
        }

        private void StartEffect(float value)
        {
            switch (_state)
            {
                case FadeState.None:
                    _fadedImage.enabled = true;
                    _targetAlpha = Mathf.Clamp(value, _visualSettings.MinAlpha, _visualSettings.MaxAlpha);
                    _timeCounter = 0;
                    _beginAlpha = 0;
                    _state = FadeState.Up;
                    break;
                case FadeState.Up:
                    if (value <= _targetAlpha)
                    {
                        break;
                    }
                    _targetAlpha = Mathf.Clamp(value, _visualSettings.MinAlpha, _visualSettings.MaxAlpha);
                    break;
                case FadeState.Down:
                    float targetAlpha = Mathf.Clamp(value, _visualSettings.MinAlpha, _visualSettings.MaxAlpha);
                    float a = _currentColor.a;
                    if (targetAlpha <= a)
                    {
                        break;
                    }
                    _timeCounter = 0;
                    _beginAlpha = a;
                    _targetAlpha = targetAlpha;
                    _state = FadeState.Up;
                    break;
            }
        }
        
        
    }
}