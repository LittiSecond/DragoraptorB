using UnityEngine;

using TimersService;
using ObjPool;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.Models;
using Dragoraptor.MonoBehs;



namespace Dragoraptor.Character
{
    public class AttackController : IAttackController, IBodyUser, ICharStateListener
    {
        
        private const string HIT_VISUAL_EFFECT = "EffectBoom";
        private const string ATTACK_VISUAL_EFFECT = "ClawScratch";

        private readonly IEnergyStore _energyStore;
        private readonly ITimersService _timersService;
        private readonly IObjectPool _objectPool;
        private readonly ICharHorizontalDirection _directionController;
        private Transform _bodyTransform;
        private readonly AttackAreasPack _attackAreas;
        private CharacterState _state;

        private float _energyCost;
        private float _attackInterval;

        private int _attackPower;
        private int _layerMaskToAttack = (1 << (int)SceneLayer.Npc);
        private int _timerId;

        private bool _haveBody;
        private bool _shouldAttack;


        public AttackController(IEnergyStore energyStore, 
            ITimersService timersService, 
            IObjectPool objectPool,
            IDataHolder dataHolder,
            ICharHorizontalDirection direction)
        {
            _energyStore = energyStore;
            _timersService = timersService;
            _objectPool = objectPool;
            _directionController = direction;
            
            var gps = dataHolder.GetGamePlaySettings();
            _attackAreas = gps.AttackAreas;
            _energyCost = gps.AttackEnergyCost;
            _attackInterval = gps.AttackInterval;
            _attackPower = gps.AttackPower;

            _timerId = 0;
        }

        #region IAttackController
        
        public void TouchBegin()
        {
            if (_haveBody)
            {
                if (CheckStateCanAttack())
                {
                    if (_timerId == 0)
                    {
                        DoAttack();
                    }
                    else
                    {
                        _shouldAttack = true;
                    }
                }
            }
        }
        
        #endregion


        #region IBodyUser
        
        public void SetBody(PlayerBody body)
        {
            _bodyTransform = body.transform;
            _haveBody = true;
        }

        public void ClearBody()
        {
            _bodyTransform = null;
            _haveBody = false;
            _shouldAttack = false;
            if (_timerId > 0)
            {
                _timersService.RemoveTimer(_timerId);
                _timerId = 0;
            }
        }
        
        #endregion


        #region ICharStateListener
        
        public void StateChanged(CharacterState newState)
        {
            _state = newState;
        }
        
        #endregion
        
        
        private bool CheckStateCanAttack()
        {
            return (_state == CharacterState.Idle || _state == CharacterState.Walk ||
                    _state == CharacterState.FliesUp || _state == CharacterState.FliesDown);
        }

        private void DoAttack()
        {
            _shouldAttack = false;
            if (_energyStore.Spend(_energyCost))
            {
                Rect damagedArea = CalculateDamagedArea();
                
                CreateVisualAttackEffect(damagedArea);
                
                Collider2D[] targets = Physics2D.OverlapAreaAll(damagedArea.min, damagedArea.max, _layerMaskToAttack);

                if (targets.Length > 0)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        MakeDamage(targets[i]);
                    }
                    //CreateVisualHitEffect(damagedArea);
                }

                _timerId = _timersService.AddTimer(OnAttackTimer, _attackInterval);
            }
        }
        
        private Rect CalculateDamagedArea()
        {
            Rect rect;
            Direction dir = _directionController.HorizontalDirection;
            if (dir == Direction.Rigth)
            {
                switch (_state)
                {
                    case CharacterState.FliesUp:
                        rect = _attackAreas.RightFliesUp;
                        break;
                    case CharacterState.FliesDown:
                        rect = _attackAreas.RightFliesDown;
                        break;
                    case CharacterState.Walk:
                        rect = _attackAreas.RightWalk;
                        break;
                    default:
                        rect = _attackAreas.RightIdle;
                        break;
                }
            }
            else
            {
                switch (_state)
                {
                    case CharacterState.FliesUp:
                        rect = _attackAreas.LeftFliesUp;
                        break;
                    case CharacterState.FliesDown:
                        rect = _attackAreas.LeftFliesDown;
                        break;
                    case CharacterState.Walk:
                        rect = _attackAreas.LeftWalk;
                        break;
                    default:
                        rect = _attackAreas.LeftIdle;
                        break;
                }
            }

            Vector2 position = _bodyTransform.position;
            rect.max += position;
            rect.min += position;
            return rect;
        }

        private void MakeDamage(Collider2D targetCollider)
        {
            ITakeDamage target = targetCollider.GetComponent<ITakeDamage>();
            if (target != null)
            {
                target.TakeDamage(_attackPower);
            }
        }

        private void CreateVisualHitEffect(Rect area)
        {
            CreateEffect(area, HIT_VISUAL_EFFECT);
        }

        private void CreateVisualAttackEffect(Rect area)
        {
            CreateEffect(area, ATTACK_VISUAL_EFFECT);
        }

        private void CreateEffect(Rect area, string prefabID)
        {
            PooledObject effect = _objectPool.GetObjectOfType(prefabID);
            if (effect)
            {
                effect.transform.position = (Vector3)area.center;
                IActivatable initializable = effect as IActivatable;
                initializable?.Activate();
            }
        }

        private void OnAttackTimer()
        {
            _timerId = 0;
            if (_shouldAttack)
            {
                if (CheckStateCanAttack())
                {
                    DoAttack();
                }
                else
                {
                    _shouldAttack = false;
                }
            }
        }
        
    }
}