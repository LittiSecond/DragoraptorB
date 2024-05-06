using UnityEngine;
using System; 


namespace Dragoraptor.MonoBehs
{
    public sealed class PlayerBody : MonoBehaviour, ITakeDamage
    {

        [SerializeField] private Rigidbody2D _rigedbody;
        [SerializeField] private LineRenderer _trajectoryRenderer;
        [SerializeField] private LineRenderer _powerRenderer;
        [SerializeField] private Animator _bodyAnimator;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private bool _isDamageBlocked;

        public event Action OnGroundContact;

        private Direction _direction;
        private ITakeDamage _damageReceiver;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (OnGroundContact != null)
            {
                if (collision.gameObject.layer == (int)SceneLayer.Ground)
                {
                    OnGroundContact();
                }
            }
        }

        private void OnEnable()
        {
            _direction = (_bodySpriteRenderer.flipX)? Direction.Rigth : Direction.Left;
        }


        public Rigidbody2D GetRigidbody()
        {
            return _rigedbody;
        }

        public (LineRenderer, LineRenderer) GetLineRenderers()
        {
            return (_trajectoryRenderer, _powerRenderer);
        }

        public Animator GetBodyAnimator()
        {
            return _bodyAnimator;
        }

        public void SetDirection(Direction direction)
        {
            if (_direction != direction)
            {
                _direction = direction;
                _bodySpriteRenderer.flipX = _direction == Direction.Rigth;
            }
        }

        public void SetDamageReceiver(ITakeDamage takeDamage)
        {
            _damageReceiver = takeDamage;
        }


        #region ITakeDamag

        public void TakeDamage(float amount)
        {
            if (!_isDamageBlocked)
            {
                _damageReceiver?.TakeDamage(amount);
            }
        }

        #endregion

    }
}