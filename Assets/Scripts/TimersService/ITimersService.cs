using System;


namespace TimersService
{
    public interface ITimersService
    {
        int AddTimer(Action method, float duration);
        int AddRepeatedTimer(Action method, float duration);
        int AddTimer<TParamType>(Action<TParamType> method, float duration, TParamType parameter);
        int AddRepeatedTimer<TParamType>(Action<TParamType> method, float duration, TParamType parameter);
        void RemoveTimer(int timerId);
        bool Contains(int timerId);
        void PauseTimer(int timerId);
        void ResumeTimer(int timerId);
        float GetTimeLeft(int timerID);
        void ChangeDuration(int timerId, float newDuration);
        void ChangeDelayFromCurrent(int timerId, float newDelay);
        void ChangeDelayFromBegin(int timerId, float newDelay);
    }
}