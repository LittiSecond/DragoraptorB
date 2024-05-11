using UnityEngine;

using VContainer.Unity;

using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;


namespace Dragoraptor.Character
{
    public class FlightController : ITickable, IBodyUser, ICharStateListener
    {

        private ICharStateHolder _stateHolder;
        private PlayerBody _playerBody;
        private Transform _bodyTransform;
        private Rigidbody2D _rigidbody;

        private float _previousY;

        private CharacterState _state;

        private bool _haveBody;
        private bool _isEnabled;
        private bool _isFirstFrame;


        public FlightController(ICharStateHolder stateHolder)
        {
            _stateHolder = stateHolder;
        }
        
        
        #region ITickable
        
        public void Tick()
        {
            if (_isEnabled)
            {
                float currentY = _bodyTransform.position.y;
                if (_state == CharacterState.FliesUp)
                {
                    if (_isFirstFrame)
                    {
                        _isFirstFrame = false;
                    }
                    else if (currentY < _previousY)
                    {
                        _stateHolder.SetState(CharacterState.FliesDown);
                    }
                }
                _previousY = currentY;
            }
        }
        
        #endregion


        #region IBodyUser
        
        public void SetBody(PlayerBody body)
        {
            _playerBody = body;
            _bodyTransform = _playerBody.transform;
            _rigidbody = _playerBody.GetRigidbody();
            _playerBody.OnGroundContact += OnGroundContact;
            _haveBody = true;
        }

        public void ClearBody()
        {
            _playerBody.OnGroundContact -= OnGroundContact;
            _playerBody = null;
            _bodyTransform = null;
            _rigidbody = null;
            _haveBody = false;
            _isEnabled = false;
            _isFirstFrame = false;
        }
        
        #endregion


        #region ICharStateListener
        
        public void StateChanged(CharacterState newState)
        {
            _state = newState;
            _isEnabled = _haveBody && (_state == CharacterState.FliesUp || _state == CharacterState.FliesDown);

            if (_state == CharacterState.FliesUp)
            {
                _isFirstFrame = true;
            }
        }
        
        #endregion
        
        
        private void OnGroundContact()
        {
            if (_state == CharacterState.Death)
            {
                _rigidbody.velocity = Vector2.zero;
            }
            if (_isEnabled)
            {
                _rigidbody.velocity = Vector2.zero;
                _stateHolder.SetState(CharacterState.Idle);
            }
        }
        
    }
}