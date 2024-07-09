using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.Models.Npc;
using ObjPool;


namespace Dragoraptor.Npc
{
    public class ShipType3Attack : IExecutable, IActivatable
    {

        private const string BULLET_ID = "StoneBall";
        private const float PLAYER_ON_GROUND_MAX_Y = -3.2f; 

        private readonly Transform _bulletStartPoint;
        private IObjectPool _pool;
        private IPlayerPosition _playerPosition;
        private ShipAttackStats _stats;

        
        private float _timeCounter;


        private bool _isReady;

        
        public ShipType3Attack(Transform bulletStartPoint, ShipAttackStats stats)
        {
            _bulletStartPoint = bulletStartPoint;
            _stats = stats;
        }
        
        public void Construct(IPlayerPosition playerPosition, IObjectPool pool)
        {
            _playerPosition = playerPosition;
            _pool = pool;
        }
        
        private void Attack()
        {
            PooledObject obj = _pool.GetObjectOfType(BULLET_ID);
            if (obj != null)
            {
                IProjectile bullet = obj as IProjectile;
                if (bullet != null)
                {
                    obj.transform.position = _bulletStartPoint.position;
                    bullet.Damage = _stats.Power;
                    bullet.Kick();
                }
            }
        }

        #region IExecutable
        
        public void Execute()
        {
            if (_isReady)
            {
                Vector3? targetPosition = _playerPosition.GetPlayerPosition();
                if (targetPosition.HasValue)
                {
                    float dx = _bulletStartPoint.position.x - targetPosition.Value.x;
                    bool isOnGround = targetPosition.Value.y < PLAYER_ON_GROUND_MAX_Y; 
                    
                    if ( isOnGround && dx > -_stats.MaxXInaccuracy && dx < _stats.MaxXInaccuracy )
                    {
                        Attack();
                        _isReady = false;
                    }
                }
            }
            else
            {
                _timeCounter += Time.deltaTime;
                if (_timeCounter >= _stats.ReloadTime)
                {
                    _timeCounter = 0.0f;
                    _isReady = true;
                }
            }
        }
        
        #endregion


        #region IActivatable
        
        public void Activate()
        {
            _timeCounter = 0.0f;
            _isReady = false;
        }
        
        #endregion

        
    }
}