using UnityEngine;
using ObjPool;
using TimersService;


namespace Dragoraptor.MonoBehs
{
    public class ClawScratch : PooledObject
    {
        [SerializeField] private SpriteRenderer _image;
        [SerializeField] private float _liveTime = 0.5f;

        private ITimersService _timersService;
        private int _timerId;
        
        
        public void Activate(Direction direction, ITimersService timersService)
        {
            if (_timerId == 0)
            {
                _timersService = timersService;
                _timerId = timersService.AddTimer(DestroyItself, _liveTime);
            }

            if (direction == Direction.Left)
            {
                _image.flipX = true;
            }
            else
            {
                _image.flipX = false;
            }
        }

        private void DestroyItself()
        {
            _timerId = 0;
            ReturnToPool();
        }
        
        public override void PrepareToReturnToPool()
        {
            base.PrepareToReturnToPool();
            if (_timerId > 0)
            {
                _timersService.RemoveTimer(_timerId);
                _timerId = 0;
            }
        }

    }
}