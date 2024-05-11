using System.Collections.Generic;
using UnityEngine;

using Dragoraptor.Interfaces.Character;
using VContainer;


namespace Dragoraptor.Character
{
    public class CharStateHolder : ICharStateHolder
    {

        private IReadOnlyList<ICharStateListener> _listeners;

        private CharacterState _state;

        public CharStateHolder()
        {
            Debug.Log("CharStateHolder->ctor:");
        }

        public void SetStateListeners(IReadOnlyList<ICharStateListener> list)
        {
            _listeners = list;
        }
        
        #region ICharStateHolder

        public CharacterState State => _state;
        
        public void SetState(CharacterState newState)
        {
            Debug.Log("CharStateHolder->SetState: newState = " + newState.ToString());
            if (newState == _state) return;
            _state = newState;
            
            if (_listeners != null)
            {
                for (int i = 0; i < _listeners.Count; i++)
                {
                    _listeners[i].StateChanged(newState);
                }
            }

        }
        
        #endregion
        
        
    }
}