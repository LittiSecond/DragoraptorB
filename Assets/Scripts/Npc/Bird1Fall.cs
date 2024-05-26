using UnityEngine;
using Dragoraptor.Interfaces;


namespace Dragoraptor.Npc
{
    public class Bird1Fall : ICleanable
    {


        private readonly Collider2D _collider;
        private readonly Rigidbody2D _rigidbody;

        private bool _isFall;


        public Bird1Fall(Collider2D c, Rigidbody2D rb)
        {
            _collider = c;
            _rigidbody = rb;
        }


        public void StartFall()
        {
            if (!_isFall)
            {
                _collider.isTrigger = false;
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                _collider.gameObject.layer = (int)SceneLayer.BulletPhisic;
                _isFall = true;
            }
        }

        public void OnGroundContact()
        {
            StopFall();
        }

        private void StopFall()
        {
            if (_isFall)
            {
                _collider.isTrigger = true;
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;
                _collider.gameObject.layer = (int)SceneLayer.Npc;
                _rigidbody.velocity = Vector2.zero;
                _isFall = false;
            }
        }

        
        #region ICleanable
        
        public void Clear()
        {
            StopFall();
        }
        
        #endregion

    }
}