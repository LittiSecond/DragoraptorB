using UnityEngine;

using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Npc
{
    public class Bird1Logic : NpcBaseLogic
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _destroyDelay = 5.1f;

        private NpcMovementUsingWayPoints _movement;
        private NpcDirectionByScale _direction;
        private Bird1Fall _fall;
        private Bird1Animation _animation;


        protected override void Awake()
        {
            base.Awake();
            var tempTransform = transform;
            _direction = new NpcDirectionByScale(tempTransform);
            _movement = new NpcMovementUsingWayPoints(tempTransform, _rigidbody, _direction);
            AddExecutable(_movement);
            AddActivatable(_movement);
            AddCleanable(_movement);
            _movement.OnWayFinished += OnWayFinished;
            _movement.Speed = _speed;
            _fall = new Bird1Fall(_collider, _rigidbody);
            AddCleanable(_fall);
            _animation = new Bird1Animation(_animator);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == (int)SceneLayer.Ground)
            {
                _fall.OnGroundContact();
                _animation.SetGrounded();
                DropItem();
                DestroyItselfDelay(_destroyDelay);
                _collider.enabled = false;
            }
        }


        public override void SetAdditionalData(NpcData additionalData)
        {
            NpcDataWay data = additionalData as NpcDataWay;
            if (data != null)
            {
                _movement.SetWay(data);
            }
        }

        protected override void OnHealthEnded()
        {
            _movement.StopMovementLogic();
            _fall.StartFall();
            _animation.SetFall();
            SendScoreReward();
        }

        public override void Activate()
        {
            base.Activate();
            _animation.SetFlying();
            _collider.enabled = true;
        }

        private void OnWayFinished()
        {
            DestroyItSelf();
        }
    }
}