using UnityEngine;

using VContainer.Unity;

using EventBus;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Ui;
using Dragoraptor.ScriptableObjects;



namespace Dragoraptor.Core
{
    public class LevelTimerController : ILevelTimer, ITickable
    {

        private const float UI_UPDATE_INTERVAL = 1.0f;
        
        private ITimerView _timerView;
        private ICurrentLevelDescriptorHolder _descriptorHolder;
        private IEventBus _eventBus;

        private float _updateTimeCounter;
        private float _levelTimeCounter;

        private bool _isTiming;


        public LevelTimerController(ITimerView timerView, ICurrentLevelDescriptorHolder holder, IEventBus eventBus)
        {
            _timerView = timerView;
            _descriptorHolder = holder;
            _eventBus = eventBus;
        }
        
        
        #region ILevelTimer
        
        public void StartTimer()
        {
            if (_isTiming)
            {
                StopTimer();
            }

            LevelDescriptor levelDescriptor = _descriptorHolder.GetCurrentLevel();
            _levelTimeCounter = levelDescriptor.LevelDuration;
            _updateTimeCounter = 0.0f;
            UpdateUi();
            _isTiming = true;
        }

        public void StopTimer()
        {
            _isTiming = false;
        }
        
        #endregion


        #region ITickable
        
        public void Tick()
        {
            if (_isTiming)
            {
                float deltaTime = Time.deltaTime;

                _updateTimeCounter += deltaTime;
                if (_updateTimeCounter >= UI_UPDATE_INTERVAL)
                {
                    UpdateUi();
                    _updateTimeCounter = 0.0f;
                }

                _levelTimeCounter -= deltaTime;
                if (_levelTimeCounter <= 0.0f)
                {
                    StopTimer();
                    _eventBus.Invoke(new LevelTimeUpSignal());
                }

            }
        }
        
        #endregion

        
        private void UpdateUi()
        {
            _timerView.SetTime(_levelTimeCounter);
        }
        
    }
}