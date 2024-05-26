using UnityEngine;

namespace Dragoraptor.Npc
{
    public class Bird1Animation
    {
        
        private const string ANIMATION_PROPERTI_NAME = "State";
        private const int ANIMATION_STATE_FLYING = 0;
        private const int ANIMATION_STATE_FALL = 1;
        private const int ANIMATION_STATE_GROUNDED = 2;


        private readonly Animator _animator;
        private readonly int _state;


        public Bird1Animation(Animator animator)
        {
            _animator = animator;
            _state = Animator.StringToHash(ANIMATION_PROPERTI_NAME);
        }


        public void SetFlying()
        {
            _animator.SetInteger(_state, ANIMATION_STATE_FLYING);
        }

        public void SetFall()
        {
            _animator.SetInteger(_state, ANIMATION_STATE_FALL);
        }

        public void SetGrounded()
        {
            _animator.SetInteger(_state, ANIMATION_STATE_GROUNDED);
        }
    }
}