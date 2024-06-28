using Dragoraptor.Interfaces;
using UnityEngine;
using VContainer.Unity;


namespace Dragoraptor.Character
{
    public class PlayerDamageVisualizer : ITickable
    {
        private const string PREFAB_ID = "ToDarkVisualEffect";

        private IPlayerDamaged _playerDamaged;
        private IPrefabLoader _prefabLoader;
        private ISceneGeometry _sceneGeometry;
        
        private enum FadeState { None, Up, Down};
        
        private SpriteRenderer _fadedImage;
        private float _max = 0.9f;
        private float _min = 0.4f;
        private float _upTime = 0.2f;
        private float _downTime = 0.5f;
        
        private FadeState _state;
        private Color _currentColor;
        private float _timeCounter;
        private float _targetAlpha;
        private float _beginAlpha;
        

        private bool _isInitialized;


        public PlayerDamageVisualizer(IPlayerDamaged playerDamaged, IPrefabLoader prefabLoader, ISceneGeometry sceneGeometry)
        {
            _playerDamaged = playerDamaged;
            _playerDamaged.OnDamaged += Damaged;
            _prefabLoader = prefabLoader;
            _sceneGeometry = sceneGeometry;
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
            var prefab = _prefabLoader.GetPrefab(PREFAB_ID); 
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
            float alpha = Mathf.Clamp( _timeCounter / _upTime, 0, 1) * (_targetAlpha - _beginAlpha) + _beginAlpha;
            _currentColor.a = alpha;
            _fadedImage.color = _currentColor;
            if (_timeCounter >= _upTime)
            {
                _state = FadeState.Down;
                _timeCounter = 0;
            }
        }

        private void DownAlpha()
        {
            _timeCounter += Time.deltaTime;
            float alpha = Mathf.Clamp(1 - _timeCounter / _downTime, 0, 1) * _targetAlpha;
            _currentColor.a = alpha;
            _fadedImage.color = _currentColor;
            if (_timeCounter >= _downTime)
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
                    _targetAlpha = Mathf.Clamp(value, _min, _max);
                    _timeCounter = 0;
                    _beginAlpha = 0;
                    _state = FadeState.Up;
                    break;
                case FadeState.Up:
                    if (value <= _targetAlpha)
                    {
                        break;
                    }
                    _targetAlpha = Mathf.Clamp(value, _min, _max);
                    break;
                case FadeState.Down:
                    float targetAlpha = Mathf.Clamp(value, _min, _max);
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