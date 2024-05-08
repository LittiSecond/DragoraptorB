using System.Collections.Generic;
using UnityEngine;

using Dragoraptor.Interfaces;
using VContainer;


namespace Dragoraptor.Character
{
    public class CharStateHolder : ICharStateHolder
    {

        private IReadOnlyList<ICharStateListener> _listeners;


        public CharStateHolder()
        {
            //_listeners = list;
            Debug.Log("CharStateHolder->ctor:");
        }
        
        // public CharStateHolder(IReadOnlyList<ICharStateListener> list)
        // {
        //     _listeners = list;
        //     Debug.Log("CharStateHolder->ctor:");
        // }
        
        [Inject]
        public void Construct(IReadOnlyList<ICharStateListener> list)
        {
            _listeners = list;
            Debug.Log("CharStateHolder->Construct:");
        }
        
        
        public void SetStateListeners(IReadOnlyList<ICharStateListener> list)
        {
            _listeners = list;
        }
        
        #region ICharStateHolder
        
        public void SetState(CharacterState newState)
        {
            Debug.Log("CharStateHolder->SetState: newState = " + newState.ToString());

            if (_listeners != null)
            {
                foreach (var listener in _listeners)
                {
                    listener.StateChanged(newState);
                }
            }

        }
        
        #endregion
        
        
    }
}