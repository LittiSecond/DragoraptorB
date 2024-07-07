
using UnityEngine;

using VContainer;

using ObjPool;

using Dragoraptor.Interfaces;
using TimersService;


namespace Dragoraptor.MonoBehs
{
    public class FreeFallStone : PooledObject, IProjectile
    {
        
        private const string HIT_VISUAL_EFFECT = "EffectBoom";

        [SerializeField] private float _verticalOffsetVisualEffect = -0.2f;
        [SerializeField] private float _damag;
        [SerializeField] private float _destroyDelay = 5.0f;
        [SerializeField] private SpriteRenderer _renderer;

        private IMultiFading _fader;
        private ITimersService _timersService;
        private IUpdateService _updateService;
        private IObjectPool _pool;

        private int _timerID;
        private bool _isDamageEnabled;
        private bool _isFadingEnabled;


        [Inject]
        private void Construct(IUpdateService updateService, ITimersService timersService, IObjectPool pool)
        {
            _timersService = timersService;
            _updateService = updateService;
            _pool = pool;
            _fader = new MultiFading(updateService);
            _fader.SetRenderers(new SpriteRenderer[] {_renderer});
        }


        #region IProjectile

        public float Damage
        {
            get => _damag; 
            set => _damag = value;
        }
        
        public void Kick()
        {
            _isDamageEnabled = true;
            _isFadingEnabled = true;
            _fader.FadingDuration = _destroyDelay;
        }
        
        #endregion
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject other = collision.gameObject;
            
            if (_isDamageEnabled)
            {
                if (other.layer == (int)SceneLayer.Player)
                {
                    ITakeDamage damageReceiver = other.GetComponent<ITakeDamage>();
                    if (damageReceiver != null)
                    {
                        damageReceiver.TakeDamage(_damag);
                        CreateVisualHitEffect();
                        _isDamageEnabled = false;
                    }
                }
            }

            if (_isFadingEnabled)
            {
                if (other.layer == (int)SceneLayer.Ground)
                {
                    _fader.StartFading();
                    _isFadingEnabled = false;
                    _isDamageEnabled = false;
                    if (_timerID == 0)
                    {
                        _timerID = _timersService.AddTimer(OnDestroyTimer, _destroyDelay);
                    }
                }
            }
        }

        private void CreateVisualHitEffect()
        {
            PooledObject effect = _pool.GetObjectOfType(HIT_VISUAL_EFFECT);
            if (effect)
            {
                Vector3 position = transform.position;
                position.y += _verticalOffsetVisualEffect;
                effect.transform.position = position;
                (effect as IActivatable)?.Activate();
            }
        }

        private void OnDestroyTimer()
        {
            _timerID = 0;
            DestroyItself();
        }
        
        private void DestroyItself()
        {
            PrepareToReturnToPool();
            ReturnToPool();
        }

        public override void PrepareToReturnToPool()
        {
            base.PrepareToReturnToPool();
            _fader.StopFading();
            _fader.RestoreColors();
            if (_timerID > 0)
            {
                _timersService.RemoveTimer(_timerID);
                _timerID = 0;
            }
        }
    }
}