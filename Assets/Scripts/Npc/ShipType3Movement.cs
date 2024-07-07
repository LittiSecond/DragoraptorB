using Dragoraptor.Interfaces;
using Dragoraptor.Models.Npc;
using UnityEngine;


namespace Dragoraptor.Npc
{
    public class ShipType3Movement : IExecutable, ICleanable
    {
        
        private readonly Transform _rootTransform;
        private readonly Transform _bulletStartPoint;
        private readonly Rigidbody2D _rigidbody;
        private IPlayerPosition _playerPosition;
        private ShipMovementStats _stats;
        
        private Vector3 _velocity;



        public ShipType3Movement(Transform rootTransform, Transform bulletStartPoint,
            Rigidbody2D rigidbody, ShipMovementStats stats)
        {
            _rootTransform = rootTransform;
            _bulletStartPoint = bulletStartPoint;
            _rigidbody = rigidbody;
            _stats = stats;
        }

        public void SetPlayerPosition(IPlayerPosition playerPosition)
        {
            _playerPosition = playerPosition;
        }
        
        private float CalculateVerticalSpeed()
        {
            return (_rootTransform.position.y > _stats.MinY) ? -_stats.VerticalSpeed : 0.0f; 
        }

        private float CalculateHorizontalSpeed(float targetX)
        {
            float speed = 0.0f;
            float x = _bulletStartPoint.position.x;

            float dx = targetX - x;

            if (dx > _stats.MaxXInaccuracy)
            {
                speed = _stats.HorizontalSpeed;
            }
            else if (dx < -_stats.MaxXInaccuracy)
            {
                speed = -_stats.HorizontalSpeed;
            }

            return speed;
        }


        #region IExecutable
        
        public void Execute()
        {
            Vector3 newVelocity = Vector3.zero;
            Vector3? playerPosition = _playerPosition.GetPlayerPosition();
            if (playerPosition.HasValue)
            {
                newVelocity.x = CalculateHorizontalSpeed(playerPosition.Value.x);
                newVelocity.y = CalculateVerticalSpeed();
            }

            if (newVelocity != _velocity)
            {
                _rigidbody.velocity = newVelocity;
                _velocity = newVelocity;
            }
        }
        
        #endregion


        #region ICleanable

        public void Clear()
        {
            _rigidbody.velocity = Vector3.zero;
            _velocity = Vector3.zero;
        }
        
        #endregion
    }
}