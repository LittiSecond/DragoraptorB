using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer.Unity;


namespace TimersService
{
    
    public class TimersServiceBehaviour : ITimersService, ITickable
    {

        private class MyTimer
        {
            public int Id;
            public float Duration;
            public float TimeLeft;
            public bool IsRepeated;
            public Action Func;

            public virtual void Invoke() => Func();
        }
        
        private class MyTimer<TParam> : MyTimer
        {
            public TParam Data;
            public Action<TParam> FuncP;

            public override void Invoke() => FuncP(Data);
        }


        private List<MyTimer> _timers = new();
        private List<MyTimer> _pausedTimers = new();

        private int _nextId = 1;

        
        #region ITimersService
        
        public int AddTimer(Action method, float duration)
        {
            return AddNewTimer(method, duration, false);
        }

        public int AddRepeatedTimer(Action method, float duration)
        {
            return AddNewTimer(method, duration, true);
        }

        public int AddTimer<TParamType>(Action<TParamType> method, float duration, TParamType parameter)
        {
            return AddNewTimer(method, duration, parameter, false);
        }

        public int AddRepeatedTimer<TParamType>(Action<TParamType> method, float duration, TParamType parameter)
        {
            return AddNewTimer(method, duration, parameter, true);
        }

        public void RemoveTimer(int timerId)
        {
            int count = _timers.RemoveAll(item => item.Id == timerId);
            if (count == 0)
            {
                count = _pausedTimers.RemoveAll(item => item.Id == timerId);
                if (count == 0)
                {
                    Debug.LogError("TimersService->RemoveTimer: timerId not found.");
                }
            }
        }

        public float GetTimeLeft(int timerId)
        {
            float timeLeft = 0.0f;

            if (TryFindTimer(timerId, out MyTimer timer))
            {
                timeLeft = timer.TimeLeft;
            }
            else
            {
                Debug.LogError("TimersService->GetTimeLeft: timerId not found.");
            }

            return timeLeft;
        }

        public bool Contains(int timerId)
        {
            bool isContains = _timers.Exists(timer => timer.Id == timerId);

            if (!isContains)
            {
                isContains = _pausedTimers.Exists(timer => timer.Id == timerId);
            }

            return isContains;
        }

        public void PauseTimer(int timerId)
        {
            var timer = _timers.FirstOrDefault();
            if (timer != null)
            {
                _timers.Remove(timer);
                _pausedTimers.Add(timer);
            }
        }

        public void ResumeTimer(int timerId)
        {
            var timer = _pausedTimers.FirstOrDefault();
            if (timer != null)
            {
                _pausedTimers.Remove(timer);
                _timers.Add(timer);
            }
        }

        public void ChangeDuration(int timerId, float newDuration)
        {
            if (newDuration <= 0.0f)
            {
                Debug.LogError("TimersService->ChangeDuration: Error: newDuration = " + newDuration.ToString());
                return;
            }
            
            if (TryFindTimer(timerId, out MyTimer timer))
            {
                timer.Duration = newDuration;
            }
            else
            {
                Debug.LogError("TimersService->ChangeDuration: timerId not found.");
            }
        }

        public void ChangeDelayFromCurrent(int timerId, float newDelay)
        {
            if (newDelay <= 0.0f)
            {
                Debug.LogError("TimersService->ChangeDelayFromCurrent: Error: newDelay = " + newDelay.ToString());
                return;
            }
            
            if (TryFindTimer(timerId, out MyTimer timer))
            {
                timer.TimeLeft = newDelay;
            }
            else
            {
                Debug.LogError("TimersService->ChangeDelayFromCurrent: timerId not found.");
            }
        }

        public void ChangeDelayFromBegin(int timerId, float newDelay)
        {
            if (newDelay <= 0.0f)
            {
                Debug.LogError("TimersService->ChangeDelayFromBegin: Error: newDelay = " + newDelay.ToString());
                return;
            }
            
            if (TryFindTimer(timerId, out MyTimer timer))
            {
                float newTimeLeft = newDelay - ( timer.Duration - timer.TimeLeft);
                timer.TimeLeft = (newTimeLeft > 0.0f) ? newTimeLeft : 0.0f;
            }
            else
            {
                Debug.LogError("TimersService->ChangeDelayFromCurrent: timerId not found.");
            }
        }
        
        #endregion


        #region ITickable

        public void Tick()
        {
            if (_timers.Count > 0)
            {
                float deltaTime = Time.deltaTime;
                for (int i = 0; i < _timers.Count; i++)
                {
                    var current = _timers[i];
                    current.TimeLeft -= deltaTime;
                    if (current.TimeLeft <= 0.0f)
                    {
                        if (current.IsRepeated)
                        {
                            current.TimeLeft = current.Duration;
                        }
                        else
                        {
                            _timers.Remove(current);
                            i--;
                        }
                        
                        current.Invoke();
                    }
                }

            }
        }

        #endregion


        private int GenerateNewId()
        {
            return _nextId++;
        }
        
        private int AddNewTimer(Action method, float duration, bool isRepeated)
        {
            if (method == null)
            {
                Debug.LogError("TimersService->AddTimer: method == null; Timer not added.");
                return 0;
            }

            if (duration <= 0.0f)
            {
                Debug.LogError("TimersService->AddTimer: duration <= 0; Timer not added.");
                return 0;
            }
            
            MyTimer newTimer = new MyTimer()
            {
                Id = GenerateNewId(),
                Duration = duration,
                TimeLeft = duration,
                IsRepeated = isRepeated,
                Func = method
            };
            _timers.Add(newTimer);
            //Debug.Log($"TimersService->AddTimer: newTimer.Id = {newTimer.Id} ");
            return newTimer.Id;
        }
        
        private int AddNewTimer<TParamType>(Action<TParamType> method, float duration, TParamType parameter, bool isRepeated)
        {
            if (method == null)
            {
                Debug.LogError("TimersService->AddTimer: method == null; Timer not added.");
                return 0;
            }

            if (duration <= 0.0f)
            {
                Debug.LogError("TimersService->AddTimer: duration <= 0; Timer not added.");
                return 0;
            }

            MyTimer<TParamType> newTimer = new MyTimer<TParamType>()
            {
                Id = GenerateNewId(),
                Duration = duration,
                TimeLeft = duration,
                IsRepeated = isRepeated,
                FuncP = method,
                Data = parameter
            };
            _timers.Add(newTimer);
            //Debug.Log($"TimersService->AddTimer<>: newTimer.Id = {newTimer.Id} ");
            return newTimer.Id;
        }

        private bool TryFindTimer(int timerId, out MyTimer timer)
        {
            timer = _timers.FirstOrDefault( tim => tim.Id == timerId );
            if (timer == null)
            {
                timer = _pausedTimers.FirstOrDefault( tim => tim.Id == timerId );
            }

            return timer != null;
        }
        
    }
}