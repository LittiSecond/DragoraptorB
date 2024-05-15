using System;
using UnityEngine;

using VContainer.Unity;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Ui;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Character
{
    public class EnergyController : ITickable, IEnergyLogic, IEnergyObservable, IEnergyStore, ICharStateListener
    {
        private const float UPDATE_OBSERVERS_INTERVAL = 0.5f;

        private INoEnergyMessageView _noEnergyMessage;

        private float _maxEnergy;
        private float _energy;
        private float _idleRegeneration;
        private float _walkRegeneration;
        private float _regenerationDelay;
        private float _updateObserversTimer;
        private float _changeRegenerationTimer;

        private CharacterState _state;
        private CharacterState _regenerationMode;

        private bool _isEnabled;
        private bool _isRegeneration;
        private bool _isChangeTimer;


        public EnergyController(INoEnergyMessageView noEnergyMessage, IDataHolder dataHolder)
        {
            _noEnergyMessage = noEnergyMessage;
            GamePlaySettings settings = dataHolder.GetGamePlaySettings();
            _maxEnergy = settings.Energy;
            _idleRegeneration = settings.EnergyRegeneration;
            _walkRegeneration = settings.WalkEnergyRegeneration;
            _regenerationDelay = settings.RegenerationDelay;
        }
        

        #region ITickable
        
        public void Tick()
        {
            if (_isEnabled)
            {
                float deltaTime = Time.deltaTime;
                if (_isRegeneration)
                {
                    RegenerationLogic(deltaTime);
                }
                
                UpdateObserversLogic(deltaTime);

                if (_isChangeTimer)
                {
                    ChangeRegenerationLogic(deltaTime);
                }
            }
        }
        
        #endregion


        #region IEnergyObservable, IObservableResource

        public event Action<float> OnMaxValueChanged;
        public event Action<float> OnValueChanged;
        public float MaxValue => _maxEnergy;
        public float Value => _energy;
        
        #endregion


        #region IEnergyStore
        
        public bool Spend(float amount)
        {
            bool isSpended = false;
            if (_isEnabled)
            {
                float amountF = amount;
                if (amountF <= _energy)
                {
                    _energy -= amountF;
                    OnValueChanged?.Invoke(_energy);
                    _updateObserversTimer = 0.0f;
                    isSpended = true;
                    _isRegeneration = true;
                }
                else
                {
                    _noEnergyMessage?.Show();
                }
            }
            return isSpended;
        }

        public void AddEnergy(float amount)
        {
            if (amount <= 0.0f) return;
            if (_energy >= _maxEnergy) return;

            _energy += amount;
            if (_energy >= _maxEnergy)
            {
                _energy = _maxEnergy;
                _isRegeneration = false;
            }
            OnValueChanged?.Invoke(_energy);
        }
        
        #endregion


        #region ICharStateListener

        public void StateChanged(CharacterState newState)
        {
            if (newState != _state)
            {
                if (newState == CharacterState.Idle || newState == CharacterState.PrepareJump)
                {
                    if (_regenerationMode == CharacterState.None || _regenerationMode == CharacterState.Walk)
                    {
                        _changeRegenerationTimer = 0.0f;
                        _isChangeTimer = true;
                    }
                }
                else if (newState == CharacterState.Walk)
                {
                    if (_regenerationMode == CharacterState.Idle)
                    {
                        _regenerationMode = CharacterState.Walk;
                        _isChangeTimer = false;
                    }
                    else if (_regenerationMode == CharacterState.None)
                    {
                        _changeRegenerationTimer = 0.0f;
                        _isChangeTimer = true;
                    }
                }
                else
                {
                    _regenerationMode = CharacterState.None;
                    _isChangeTimer = false;
                }

                _state = newState;
            }
        }
        
        #endregion


        #region IEnergyLogic
        
        public void On()
        {
            _isEnabled = true;
        }

        public void Off()
        {
            _isEnabled = false;
        }

        public void Reset()
        {
            _energy = _maxEnergy;
            _isRegeneration = false;
            _isChangeTimer = false;
            OnValueChanged?.Invoke(_energy);
            _updateObserversTimer = 0.0f;
        }
        
        #endregion
        
        private void RegenerationLogic(float deltaTime)
        {
            bool shouldRegen = false;
            float regen = 0.0f;
            if (_regenerationMode == CharacterState.Idle)
            {
                regen = _idleRegeneration;
                shouldRegen = true;
            }
            else if (_regenerationMode == CharacterState.Walk)
            {
                regen = _walkRegeneration;
                shouldRegen = true;
            }

            if (shouldRegen)
            {
                _energy += regen * deltaTime;
                if (_energy >= _maxEnergy)
                {
                    _energy = _maxEnergy;
                    _isRegeneration = false;
                }
            }
        }
        
        private void ChangeRegenerationLogic(float deltaTime)
        {

            _changeRegenerationTimer += deltaTime;
            if (_changeRegenerationTimer >= _regenerationDelay)
            {
                _changeRegenerationTimer = 0.0f;
                _isChangeTimer = false;
                if (_state == CharacterState.Idle || _state == CharacterState.PrepareJump)
                {
                    _regenerationMode = CharacterState.Idle;
                    _isRegeneration = true;
                }
                else if (_state == CharacterState.Walk)
                {
                    _regenerationMode = CharacterState.Walk;
                    _isRegeneration = true;
                }
                else
                {
                    _regenerationMode = CharacterState.None;
                    _isRegeneration = false;
                }
            }
            
        }
        
        private void UpdateObserversLogic(float deltaTime)
        {
            if (_updateObserversTimer < UPDATE_OBSERVERS_INTERVAL)
            {
                _updateObserversTimer += deltaTime;
            }
            else
            {
                OnValueChanged?.Invoke(_energy);
                _updateObserversTimer = 0.0f;
            }
        }
        
        
    }
}