using Dragoraptor.Interfaces;
using UnityEngine;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;


namespace Dragoraptor.Character
{
    public class JumpController : IJumpController, IBodyUser, ICharStateListener
    {
        
        private ICharStateHolder _stateHolder;
        private IJumpCalculator _jumpCalculator;
        //private readonly IResouceStore _energyStore;
        private Transform _bodyTransform;
        private Rigidbody2D _rigidbody;

        private CharacterState _state;
        
        private bool _haveBody;


        public JumpController(ICharStateHolder holder, IJumpCalculator calculator)
        {
            _stateHolder = holder;
            _jumpCalculator = calculator;
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
                Vector2 jumpDirection =  (Vector2)_bodyTransform.position - worldPosition;

                Vector2 impulse = _jumpCalculator.CalculateJumpImpulse(jumpDirection);

                if (impulse != Vector2.zero)
                {
                    int jumpCost = (int)_jumpCalculator.CalculateJumpCost();
                    //if (_energyStore.SpendResource(jumpCost))
                    if (true)
                    {
                        _rigidbody.AddForce(impulse, ForceMode2D.Impulse);
                        _stateHolder.SetState(CharacterState.FliesUp);
                    }
                    else
                    {
                        _stateHolder.SetState(CharacterState.Idle);
                    }
                }
                else
                {
                    _stateHolder.SetState(CharacterState.Idle);
                }

            }
            
        }
        
        #endregion
        
    }
}