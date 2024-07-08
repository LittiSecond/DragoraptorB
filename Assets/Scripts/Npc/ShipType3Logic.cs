using System;
using UnityEngine;

using ObjPool;
using Dragoraptor.Interfaces;
using Dragoraptor.Models.Npc;
using VContainer;


namespace Dragoraptor.Npc
{
    public class ShipType3Logic : NpcBaseLogic
    {
        private const string DESTRACTION_EFFECT = "ShipType3Crash";

        [SerializeField] private Transform _bulletStartPoint;
        [SerializeField] private ShipMovementStats _movementStats;
        [SerializeField] private ShipAttackStats _attackStats;
        [SerializeField] private Transform _healthBarRoot;
        [SerializeField] private Transform _healthBar;

        private ShipType3Movement _movement;
        private ShipType3Attack _attack;
        private IPlayerPosition _playerPosition;
        private NpcResourceIndicator _healthIndicator;
        
        
        protected override void Awake()
        {
            base.Awake();
            _movement = new ShipType3Movement(transform, _bulletStartPoint, _rigidbody, _movementStats);
            AddExecutable(_movement);
            AddCleanable(_movement);

            _attack = new ShipType3Attack(_bulletStartPoint, _attackStats);
            AddExecutable(_attack);
            AddActivatable(_attack);

            _healthIndicator = new NpcResourceIndicator(_healthBar, _healthBarRoot, _health);
        }

        private void Start()
        {
            _movement.SetPlayerPosition(_playerPosition);
            _attack.Construct(_playerPosition, _pool);
        }


        [Inject]
        private void Construct2(IPlayerPosition playerPosition)
        {
            _playerPosition = playerPosition;
        }
        
        protected override void OnHealthEnded()
        {
            PooledObject obj = _pool.GetObjectOfType(DESTRACTION_EFFECT);
            if (obj != null)
            {
                obj.transform.position = transform.position;
                (obj as IActivatable)?.Activate();
            }

            base.OnHealthEnded();
        }
    }
}