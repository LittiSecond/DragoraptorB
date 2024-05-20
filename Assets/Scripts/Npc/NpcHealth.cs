

using System;
using VContainer.Unity;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;


namespace Dragoraptor.Npc
{
    public class NpcHealth : ITakeDamage, IActivatable, IObservableResource
    {
        
        private IDamageObserver _damageObserver;
        
        private float _maxHealth;
        private float _currentHealth;
        private float _armor;


        public event Action OnHealthEnd;
        
        #region IObservableResource
        
        public event Action<float> OnMaxValueChanged;
        public event Action<float> OnValueChanged;
        public float MaxValue { get; }
        public float Value { get; }
        
        #endregion

        
        public NpcHealth(int maxHealth, int armor)
        {
            _maxHealth = maxHealth;
            _armor = armor;
        }
        
        public void ResetHealth()
        {
            _currentHealth = _maxHealth;
            OnValueChanged?.Invoke(_currentHealth);
        }

        public void SetDamageObserver(IDamageObserver observer)
        {
            _damageObserver = observer;
        }


        #region ITakeDamage
        
        public void TakeDamage(float amount)
        {
            amount -= _armor;
            if (amount > 0)
            {
                _damageObserver?.OnDamaged(amount);
                _currentHealth -= amount;
                if (_currentHealth < 0)
                {
                    _currentHealth = 0;
                }
                OnValueChanged?.Invoke(_currentHealth);

                if (_currentHealth == 0)
                {
                    OnHealthEnd?.Invoke();
                }
            }
        }
        
        #endregion


        #region IActivatable

        public void Activate()
        {
            ResetHealth();
        }
        
        #endregion


    }
}