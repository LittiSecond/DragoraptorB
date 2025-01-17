﻿using UnityEngine;
using VContainer;
using Dragoraptor.Interfaces;
using ObjPool;


namespace Dragoraptor.MonoBehs
{
    public class DamageAura : MonoBehaviour, IActivatable, ICleanable
    {
        
        private const string HIT_VISUAL_EFFECT = "EffectBoom";
        
        [SerializeField] private Collider2D _collider;
        [SerializeField] private SpriteRenderer _effectSprite;
        [SerializeField] private int _damage;

        private ILiveCycleHolder _liveCycleHolder;
        private IHealthEndHolder _healthEndHolder;
        private IObjectPool _pool;

        private bool _isEnabled;
        

        private void Awake()
        {
            _liveCycleHolder = GetComponentInParent<ILiveCycleHolder>();
            _liveCycleHolder.AddActivatable(this);
            _liveCycleHolder.AddCleanable(this);
            _healthEndHolder = GetComponentInParent<IHealthEndHolder>();
            _healthEndHolder.OnHealthEnd += Deactivate;
        }

        [Inject]
        private void Construct(IObjectPool objectPool)
        {
            _pool = objectPool;
        }
        

        #region IActivatable

        public void Activate()
        {
            _collider.enabled = true;
            _effectSprite.enabled = true;
            _isEnabled = true;
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isEnabled) return;
            
            GameObject other = collision.gameObject;
            
            if (other.layer == (int)SceneLayer.Player)
            {
                ITakeDamage damagReceiver = other.GetComponent<ITakeDamage>();
                if (damagReceiver != null)
                {
                    damagReceiver.TakeDamage(_damage);
                    CreateVisualHitEffect();
                }
                
                Deactivate();
            }
        }

        private void CreateVisualHitEffect()
        {
            //Debug.Log("DamageAura->CreateVisualHitEffect: ");
            PooledObject effect = _pool.GetObjectOfType(HIT_VISUAL_EFFECT);
            if (effect)
            {
                Vector3 position = transform.position;
                effect.transform.position = position;
                IActivatable initializable = effect as IActivatable;
                initializable?.Activate();
            }
        }

        private void Deactivate()
        {
            _collider.enabled = false;
            _effectSprite.enabled = false;
            _isEnabled = false;
        }

        #region ICleanable

        public void Clear()
        {
            Deactivate();
        }

        #endregion
    }
}