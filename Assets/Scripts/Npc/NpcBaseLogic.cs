using System;
using System.Collections.Generic;
using Dragoraptor.Interfaces;
using UnityEngine;

using ObjPool;
using VContainer.Unity;


namespace Dragoraptor.Npc
{
    public class NpcBaseLogic : PooledObject, IExecutable, ITakeDamage, ILiveCycleHolder, IHealthEndHolder
    {
        
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected Collider2D _collider;
        [SerializeField] private Transform _flyingDamagStartPoint;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _armor;
        [SerializeField] private int _scoreCost;

        
        public event Action<NpcBaseLogic> OnDestroy;
        

        #region IHealthEndHolder

        public event Action OnHealthEnd;

        #endregion

        private NpcHealth _health;
        private readonly List<IExecutable> _executeList = new List<IExecutable>();
        private readonly List<IActivatable> _activateList = new List<IActivatable>();
        private readonly List<ICleanable> _clearList = new List<ICleanable>();

        private float _destroyTimeCounter;

        protected bool _isEnabled;
        private bool _isDestroyTimer;
        
        
        protected virtual void Awake()
        {
            _health = new NpcHealth(_maxHealth, _armor);
            _health.OnHealthEnd += () =>
            {
                OnHealthEnd?.Invoke();
                OnHealthEnded();
            };

            _activateList.Add(_health);
            // if (_flyingDamagStartPoint)
            // {
            //     _flyingDamageCreator = new NpcFlyingDamagCreator(_flyingDamagStartPoint);
            //     _health.SetDamageObserver(_flyingDamageCreator);
            // }
        }

        
        public virtual void DestroyItSelf()
        {
            PrepareToReturnToPool();
            ReturnToPool();
        }

        protected void DestroyItselfDelay(float delay)
        {
            _destroyTimeCounter = delay;
            _isDestroyTimer = true;
        }

        public override void PrepareToReturnToPool()
        {
            base.PrepareToReturnToPool();
            for (int i = 0; i < _clearList.Count; i++)
            {
                _clearList[i].Clear();
            }

            _isEnabled = false;
            _isDestroyTimer = false;
            OnDestroy?.Invoke(this);
        }
        
        public virtual void Activate()
        {
            for (int i = 0; i < _activateList.Count; i++)
            {
                _activateList[i].Activate();
            }
            _isEnabled = true;
        }
        
        protected virtual void OnHealthEnded()
        {
            SendScoreReward();
            //DropItem();
            DestroyItSelf();
        }
        
        protected void SendScoreReward()
        {
            Debug.Log("NpcBaseLogic->SendScoreReward: " + gameObject.name);
        }

        #region IExecutable
        
        public void Execute()
        {
            if (_isEnabled)
            {
                for (int i = 0; i < _executeList.Count; i++)
                {
                    _executeList[i].Execute();
                }

                if (_isDestroyTimer)
                {
                    _destroyTimeCounter -= Time.deltaTime;
                    if (_destroyTimeCounter <= 0.0f)
                    {
                        _isDestroyTimer = false;
                        DestroyItSelf();
                    }
                }
            }
        }
        
        #endregion



        #region ITakeDamage
        
        public void TakeDamage(float amount)
        {
            _health.TakeDamage(amount);
        }
        
        #endregion


        #region ILiveCycleHolder
        
        public void AddExecutable(IExecutable executable)
        {
            _executeList.Add(executable);
        }

        public void AddActivatable(IActivatable activatable)
        {
            _activateList.Add(activatable);
        }

        public void AddCleanable(ICleanable cleanable)
        {
            _clearList.Add(cleanable);
        }
        
        #endregion

        
    }
}