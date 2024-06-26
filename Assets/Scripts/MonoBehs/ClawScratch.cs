using UnityEngine;

using VContainer;

using ObjPool;
using TimersService;

using Dragoraptor.Interfaces;


namespace Dragoraptor.MonoBehs
{
    public class ClawScratch : PooledObject, IActivatable
    {
        [SerializeField] private SpriteRenderer _image;
        [SerializeField] private float _liveTime = 0.5f;

        private ITimersService _timersService;
        private IPlayerPosition _playerPosition;
        private int _timerId;

        
        [Inject]
        private void Construct(ITimersService timersService, IPlayerPosition playerPosition)
        {
            _timersService = timersService;
            _playerPosition = playerPosition;
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


        #region IActivatable
        
        public void Activate()
        {
            if (_timerId == 0)
            {
                _timerId = _timersService.AddTimer(DestroyItself, _liveTime);
            }

            var pos = _playerPosition.GetPlayerPosition();

            if (pos.HasValue)
            {
                if (pos.HasValue)
                {
                    if (transform.position.x - pos.Value.x > 0.0f)
                    {
                        _image.flipX = false;
                    }
                    else
                    {
                        _image.flipX = true;
                    }
                
                }
            }
        }
        
        #endregion
    }
}