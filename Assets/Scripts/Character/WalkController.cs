using UnityEngine;

using VContainer.Unity;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;


namespace Dragoraptor.Character
{
    public class WalkController : ITickable, IBodyUser, ICharStateListener, IWalkLogic
    {
        private ICharStateHolder _stateHolder;
        private PlayerBody _playerBody;
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private Vector2 _velocity;
        private float _speed;
        private float _xDestination;
        private CharacterState _state;
        
        private bool _isEnabled;
        private bool _shouldMove;
        private bool _isDirectionRight;
        

        public WalkController(ICharStateHolder stateHolder, IDataHolder dataHolder)
        {
            _stateHolder = stateHolder;
            _speed = dataHolder.GetGamePlaySettings().WalkSpeed;
        }

        
        #region IBodyUser
        
        public void SetBody(PlayerBody body)
        {
            _playerBody = body;
            _transform = _playerBody.transform;
            _rigidbody = _playerBody.GetRigidbody();
            _isEnabled = true;
        }

        public void ClearBody()
        {
            if (_isEnabled)
            {
                StopMovement();
            }
            _playerBody = null;
            _transform = null;
            _rigidbody = null;
            _isEnabled = false;
        }
        
        #endregion

        
        #region ITickable

        public void Tick()
        {
            if (_isEnabled && _shouldMove)
            {
                float x = _transform.position.x;
                if (_isDirectionRight && (x >= _xDestination))
                {
                    StopMovement();
                    _stateHolder.SetState(CharacterState.Idle);
                }
                else if ( !_isDirectionRight && (x <= _xDestination))
                {
                    StopMovement();
                    _stateHolder.SetState(CharacterState.Idle);
                }
            }
        }

        #endregion


        #region ICharStateListener

        public void StateChanged(CharacterState newState)
        {
            if (newState != _state)
            {
                if (_state == CharacterState.Walk)
                {
                    StopMovement();
                }

                _state = newState;
            }
        }
        
        #endregion


        #region IWalkLogic

        public void SetDestination(float x)
        {
            if (_isEnabled && (_state == CharacterState.Idle || _state == CharacterState.Walk))
            {
                _xDestination = x;
                StartMovement();
                if (_state == CharacterState.Idle)
                {
                    _stateHolder.SetState(CharacterState.Walk);
                }
            }
        }
        
        #endregion
        
        
        private void StopMovement()
        {
            if (_isEnabled && _state == CharacterState.Walk)
            {
                _velocity = Vector2.zero;
                _rigidbody.velocity = Vector2.zero;
                _shouldMove = false;
            }
        }

        private void StartMovement()
        {
            _isDirectionRight = _xDestination > _transform.position.x;

            float direction = _isDirectionRight ? 1.0f : -1.0f;

            if (_isDirectionRight)
            {
                direction = 1.0f;
                _playerBody.SetDirection(Direction.Rigth);
            }
            else
            {
                direction = -1.0f;
                _playerBody.SetDirection(Direction.Left);
            }
            
            _velocity = new Vector2(_speed * direction, 0);
            _rigidbody.velocity = _velocity;

            _shouldMove = true;
        }
        
    }
}