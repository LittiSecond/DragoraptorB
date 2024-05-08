using UnityEngine;
using Dragoraptor.Interfaces;
using Dragoraptor.MonoBehs;
using VContainer;
using VContainer.Unity;


namespace Dragoraptor.Character
{
    public class WalkController : ITickable, IBodyUser, ICharStateListener
    {


        private ICharStateHolder _stateHolder;
        
        
        private float _delay = 2.0f;
        private float _timeCounter;
        
        private bool _isEnabled;


        public WalkController()
        {
            Debug.Log("WalkController->ctor:");
        }
        
        // [Inject]
        // public void Construct(ICharStateHolder stateHolder)
        // {
        //     _stateHolder = stateHolder;
        // }
        
        
        #region IBodyUser
        
        public void SetBody(PlayerBody body)
        {
            Debug.Log("WalkController->SetBody:");
            _isEnabled = true;
        }

        public void ClearBody()
        {
            Debug.Log("WalkController->ClearBody:");
            _isEnabled = false;
        }
        
        #endregion

        
        #region ITickable

        public void Tick()
        {
            if (!_isEnabled) return;
            
            _timeCounter += Time.deltaTime;
            if (_timeCounter >= _delay)
            {
                Debug.Log("WalkController->Tick:");
                _stateHolder?.SetState(CharacterState.Walk);
                _timeCounter = 0.0f;
            }
        }

        #endregion


        #region ICharStateListener

        public void StateChanged(CharacterState newState)
        {
            Debug.Log("WalkController->StateChanged: newState = " + newState.ToString());
        }
        
        #endregion
        
        
    }
}