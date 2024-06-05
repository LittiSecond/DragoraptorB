using UnityEngine;


namespace Dragoraptor.Npc
{
    public class Bird1Logic : NpcBaseLogic
    {
        [SerializeField] private Animator _animator;
        //[SerializeField] private Fading _fading;
        [SerializeField] private float _destroyDelay = 5.1f;

        private Bird1Movement _movement;
        private NpcDirectionByScale _direction;
        private Bird1Fall _fall;
        private Bird1Animation _animation;


        protected override void Awake()
        {
            base.Awake();
            var tempTransform = transform;
            _direction = new NpcDirectionByScale(tempTransform);
            _movement = new Bird1Movement(tempTransform, _rigidbody, _direction);
            AddExecutable(_movement);
            AddActivatable(_movement);
            AddCleanable(_movement);
            _movement.OnWayFinished += OnWayFinished;
            _fall = new Bird1Fall(_collider, _rigidbody);
            AddCleanable(_fall);
            _animation = new Bird1Animation(_animator);
            //AddExecutable(_fading);
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


        // public override void SetAdditionalData(NpcData additionalData)
        // {
        //     NpcDataWay data = additionalData as NpcDataWay;
        //     if (data != null)
        //     {
        //         _movement.SetWay(data);
        //     }
        // }

        protected override void OnHealthEnded()
        {
            _movement.StopMovementLogic();
            _fall.StartFall();
            _animation.SetFall();
            SendScoreReward();
        }

        public override void Activate()
        {
            _movement.SetWay();
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