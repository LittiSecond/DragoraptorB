using System;

using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Character
{
    public class PlayerHealth : IPlayerHealth, ITakeDamage, IBodyUser, IHealthObservable
    {

        private float _maxHealth;
        private float _health;
        private float _armor;
        
        
        #region IHealthEndHolder

        public event Action OnHealthEnd;

        #endregion
        
        
        #region IObservableResource
        
        public event Action<float> OnMaxValueChanged;
        
        public event Action<float> OnValueChanged;
        
        public float MaxValue { get => _maxHealth; }
        
        public float Value { get => _health; }
        
        #endregion


        public PlayerHealth(IDataHolder dataHolder)
        {
            GamePlaySettings gps = dataHolder.GetGamePlaySettings();
            _maxHealth = gps.MaxHealth;
            _armor = gps.Armor;
        }
        
        
        #region IPlayerHealth

        public void ResetHealth()
        {
            _health = _maxHealth;
            OnValueChanged?.Invoke(_health);
        }
        
        #endregion


        #region ITakeDamage

        public void TakeDamage(float amount)
        {
            amount -= _armor;
            if (amount > 0)
            {
                _health -= amount;
                if (_health < 0)
                {
                    _health = 0;
                }
                OnValueChanged?.Invoke(_health);

                if (_health == 0)
                {
                    OnHealthEnd?.Invoke();
                }
            }
        }
        
        #endregion


        #region IBodyUser

        public void SetBody(PlayerBody body)
        {
            body.SetDamageReceiver(this);
        }

        public void ClearBody()
        {
            
        }
        
        #endregion


    }
}