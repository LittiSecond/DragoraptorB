using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;
using UnityEngine;

namespace Dragoraptor.Character
{
    public class CharHorizontalDirection : ICharHorizontalDirection, IBodyUser
    {

        
        private PlayerBody _playerBody;
        private Transform _transform;
        private Direction _direction;

        
        
        #region ICharHorizontalDirection
        
        public void TouchPrepareJump(Vector2 worldPosition)
        {
            Direction direction = (_transform.position.x > worldPosition.x) ? Direction.Rigth : Direction.Left;
            SetBodyDirection(direction);
        }
        
        #endregion


        #region IBodyUser
        
        public void SetBody(PlayerBody body)
        {
            _playerBody = body;
            _transform = _playerBody.transform;
        }

        public void ClearBody()
        {
            _playerBody = null;
            _transform = null;
        }
        
        #endregion
        
        
        private void SetBodyDirection(Direction direction)
        {
            if (direction != _direction)
            {
                _direction = direction;
                _playerBody.SetDirection(_direction);
            }
        }
        
    }
}