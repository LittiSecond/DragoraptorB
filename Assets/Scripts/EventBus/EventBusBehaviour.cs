using System;
using System.Collections.Generic;


namespace EventBus
{
    public class EventBusBehaviour : IEventBus
    {
        private readonly Dictionary<string, List<object>> _events = new Dictionary<string, List<object>>();

        public void Subscribe<T>(Action<T> callback)
        {
            string key = typeof(T).Name;

            if (_events.ContainsKey(key))
            {
                _events[key].Add(callback);
            }
            else
            {
                _events.Add(key, new List<object>() {callback});
            }
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            string key = typeof(T).Name;

            if (_events.TryGetValue(key, out List<object> eventsOfKey))
            {
                for (int i = 0; i < eventsOfKey.Count; i++)
                {
                    if (eventsOfKey[i].Equals(callback))
                    {
                        eventsOfKey.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void Invoke<T>(T signal)
        {
            string key = typeof(T).Name;
            
            if (_events.TryGetValue(key, out List<object> eventsOfKey))
            {
                for (int i = 0; i < eventsOfKey.Count; i++)
                {
                    Action<T> callback = eventsOfKey[i] as Action<T>;
                    callback?.Invoke(signal);
                }
            }
        }
    }
}

