using UnityEngine;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;


namespace Dragoraptor.Character
{
    public class AnimationController : IBodyUser, ICharStateListener
    {
        private Animator _bodyAnimator;

        private readonly int _stateParameter = Animator.StringToHash("CharacterState");

        private bool _haveAnimator;


        // public AnimationController(CharacterStateHolder csh)
        // {
        //     csh.OnStateChanged += OnStateChanged;
        // }


        #region IBodyUser

        public void SetBody(PlayerBody pb)
        {
            _bodyAnimator = pb.GetBodyAnimator();
            _haveAnimator = true;
        }

        public void ClearBody()
        {
            _bodyAnimator = null;
            _haveAnimator = false;
        }

        #endregion


        #region ICharStateListener
        
        public void StateChanged(CharacterState newState)
        {
            if (_haveAnimator)
            {
                _bodyAnimator.SetInteger(_stateParameter, (int)newState);
            }
        }
        
        #endregion
        
    }
}