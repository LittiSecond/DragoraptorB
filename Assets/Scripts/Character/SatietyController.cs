using System;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Character
{
    public class SatietyController : ISatietyController, ISatietyCollector, ISatietyObservable
    {

        private float _maxSatiety;
        private float _satiety;
        private float _victorySatiety;

        
        public SatietyController(IDataHolder dataHolder)
        {
            GamePlaySettings gps = dataHolder.GetGamePlaySettings();
            _maxSatiety = gps.MaxSatiety;
        }
        
        
        #region ISatietyController
        
        public void ResetSatiety()
        {
            _satiety = 0;
            OnValueChanged?.Invoke(_satiety);
        }

        public void SetVictorySatiety(float satietyRelativeMax)
        {
            _victorySatiety = satietyRelativeMax;
            OnVictorySatietyChanged?.Invoke(_victorySatiety);
        }
        
        #endregion


        #region ISatietyCollector
        
        public void AddSatiety(float additionalSatiety)
        {
            if (additionalSatiety > 0 && _satiety < _maxSatiety)
            {
                _satiety += additionalSatiety;
                if (_satiety >= _maxSatiety)
                {
                    _satiety = _maxSatiety;
                }
                OnValueChanged?.Invoke(_satiety);
            }
        }

        #endregion


        #region ISatietyObservable
        
        public event Action<float> OnVictorySatietyChanged;
        
        public event Action<float> OnMaxValueChanged;
        
        public event Action<float> OnValueChanged;

        public float MaxValue => _maxSatiety;

        public float Value => _satiety;

        #endregion

    }
}