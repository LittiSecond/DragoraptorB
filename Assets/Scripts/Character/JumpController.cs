using UnityEngine;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;


namespace Dragoraptor.Character
{
    public class JumpController : IJumpController, IBodyUser, ICharStateListener
    {
        
        private ICharStateHolder _stateHolder;
        private Transform _bodyTransform;
        private Rigidbody2D _rigidbody;
        
        private CharacterState _state;
        
        private bool _haveBody;


        public JumpController(ICharStateHolder holder)
        {
            _stateHolder = holder;
        }


        #region IBodyUser
        
        public void SetBody(PlayerBody body)
        {
            _bodyTransform = body.transform;
            _rigidbody = body.GetRigidbody();
            _haveBody = true;
        }

        public void ClearBody()
        {
            _bodyTransform = null;
            _rigidbody = null;
            _haveBody = false;
        }
        
        #endregion
        
        
        #region ICharStateListener

        public void StateChanged(CharacterState newState)
        {
            _state = newState;
        }
        
        #endregion


        #region IJumpController
        
        public void TouchBegin()
        {
            if (_haveBody && (_state == CharacterState.Idle || _state == CharacterState.Walk ))
            {
                _state = CharacterState.PrepareJump;
                _stateHolder.SetState(CharacterState.PrepareJump);
            }
        }

        public void TouchEnd(Vector2 worldPosition)
        {
            if (_haveBody && _state == CharacterState.PrepareJump)
            {
                _state = CharacterState.Idle;
                _stateHolder.SetState(CharacterState.Idle);
            }
        }
        
        #endregion
        
    }
}