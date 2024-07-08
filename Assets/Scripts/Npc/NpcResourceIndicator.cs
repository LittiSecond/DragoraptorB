using Dragoraptor.Interfaces.Character;
using UnityEngine;

namespace Dragoraptor.Npc
{
    public class NpcResourceIndicator
    {

        private float INACCURACY = 0.1f;
        
        private readonly Transform _root;
        private readonly Transform _bar;
        private readonly IObservableResource _source;
        
        private float _maxXScale;
        private float _maxValue;

        private bool _isVisible;
        

        public NpcResourceIndicator(Transform bar, Transform root, IObservableResource source)
        {
            _bar = bar;
            _root = root;
            _source = source;
            _maxXScale = _bar.localScale.x;
            _source.OnValueChanged += SetValue;
            _source.OnMaxValueChanged += MaxValueChanged;
            _isVisible = _root.gameObject.activeSelf;
            UpdateValues();
        }
        
        
        private void SetValue(float newValue)
        {
            if (_maxValue > 0.0f)
            {
                float scale = newValue / _maxValue;
                Vector3 localScale = _bar.localScale;
                localScale.x = scale * _maxXScale;
                _bar.localScale = localScale;

                float maxValue = _maxValue - INACCURACY;
                
                if (newValue < maxValue && !_isVisible)
                {
                    Show();
                }
                else if ( newValue >= maxValue && _isVisible )
                {
                    Hide();
                }
            }
            //Debug.Log($"NpcHealthIndicator->SetValue: _maxValue = {_maxValue}; newValue = {newValue}; " +
            //          $"_isVisible = {_isVisible};");
        }

        private void UpdateValues()
        {
            _maxValue = _source.MaxValue;
            SetValue(_source.Value);
        }

        private void Show()
        {
            _root.gameObject.SetActive(true);
            _isVisible = true;
        }

        private void Hide()
        {
            _root.gameObject.SetActive(false);
            _isVisible = false;
        }

        private void MaxValueChanged(float newMaxValue)
        {
            _maxValue = newMaxValue;
            SetValue(_source.Value);
        }
        
    }
}