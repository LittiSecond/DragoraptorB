using UnityEngine;

using VContainer;

using ObjPool;
using TimersService;

using Dragoraptor.Interfaces;


namespace Dragoraptor.MonoBehs
{
    public class EffectBoom : PooledObject, IActivatable
    {
        
        private const int ADDITIONAL_BITS = 2;

        [SerializeField] private float _liveTime = 2.0f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _sprites;

        private ITimersService _timersService;
        
        private int _timerID;

        [Inject]
        private void Construct(ITimersService timersService)
        {
            _timersService = timersService;
        }


        private void OnDestroyTimer()
        {
            _timerID = 0;
            DestroyItself();
        }
        
        private void DestroyItself()
        {
            ReturnToPool();
        }

        public override void PrepareToReturnToPool()
        {
            base.PrepareToReturnToPool();
            if (_timerID > 0)
            {
                _timersService.RemoveTimer(_timerID);
                _timerID = 0;
            }
        }

        private void SelectSprite()
        {
            int rndValue = UnityEngine.Random.Range(0, _sprites.Length << ADDITIONAL_BITS);
            _spriteRenderer.flipX = (rndValue & 1) == 1;
            rndValue >>= 1;
            _spriteRenderer.flipY = (rndValue & 1) == 1;
            rndValue >>= 1;
            _spriteRenderer.sprite = _sprites[rndValue];
        }


        #region IActivatable

        public void Activate()
        {
            if (_timerID > 0)
            {
                _timersService.RemoveTimer(_timerID);
            }
            _timerID = _timersService.AddTimer(OnDestroyTimer, _liveTime);
            
            SelectSprite();
        }

        #endregion
    }
}