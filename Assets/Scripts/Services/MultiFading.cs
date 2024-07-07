using UnityEngine;

using Dragoraptor.Interfaces;


namespace Dragoraptor
{
    public class MultiFading : IMultiFading, IExecutable
    {

        private IUpdateService _updateService;
        private SpriteRenderer[] _renderers;

        private Color[] _startColors;
        private Color[] _endColors;
        
        private float _fadingDuration = 5.0f;
        private float _startTime;

        private bool _isFading;
        

        public MultiFading(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        #region IMultiFading

        public float FadingDuration { set => _fadingDuration = value; }
        
        public void SetRenderers(SpriteRenderer[] renderers)
        {
            _renderers = renderers;
            _startColors = new Color[renderers.Length];
            _endColors = new Color[renderers.Length];
            for (int i = 0; i < _renderers.Length; i++)
            {
                Color color = _renderers[i].color;
                _startColors[i] = color;
                color.a = 0.0f;
                _endColors[i] = color;
            }
        }

        public void StartFading()
        {
            if (!_isFading)
            {
                _updateService.AddToUpdate(this);
                _isFading = true;
                // for (int i = 0; i < _renderers.Length; i++)
                // {
                //     Color color = _renderers[i].color;
                //     _startColors[i] = color;
                //     color.a = 0.0f;
                //     _endColors[i] = color;
                // }

                _startTime = Time.time;
            }
        }

        public void StopFading()
        {
            if (_isFading)
            {
                _updateService.RemoveFromUpdate(this);
                _isFading = false;
            }
        }

        public void RestoreColors()
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].color = _startColors[i];
            }
        }
        
        #endregion


        #region IExecutable

        public void Execute()
        {
            float timepassed = Time.time - _startTime;

            for (int i = 0; i < _renderers.Length; i++)
            {
                Color newColor = Color.Lerp(_startColors[i], _endColors[i], timepassed / _fadingDuration);
                _renderers[i].color = newColor;
            }

            if (timepassed >= _fadingDuration)
            {
                StopFading();
            }
        }
        
        #endregion
    }
}