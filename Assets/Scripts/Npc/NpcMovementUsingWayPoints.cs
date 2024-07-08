using System;
using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Npc;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Npc
{
    public class NpcMovementUsingWayPoints : IExecutable, IActivatable, ICleanable
    {
        
        private const float DEFAULT_X_WAY_POINT_POS = 4.0f;
        
        public event Action OnWayFinished;

        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;
        private readonly INpcDirection _visualDirection;

        private Vector2[] _way;

        private Vector2 _destination;
        private Vector2 _startPosition;

        private float _arrivalDistanceSqr = 0.01f;
        private float _speed = 1.0f;
        private int _nexWayPointIndex;

        private bool _haveWay;
        private bool _isRelativeStartPosition;
        private bool _isCyclic;
        private bool _isEnabled;

        public float Speed
        {
            set => _speed = value;
        }
        

        public NpcMovementUsingWayPoints(Transform transform, Rigidbody2D rigidbody, INpcDirection direction)
        {
            _transform = transform;
            _rigidbody = rigidbody;
            _visualDirection = direction;
        }

        private void SetFlightDirection(Vector2 destination)
        {
            if (_haveWay)
            {
                Vector2 position = _transform.position;
                Vector2 direction = destination - position;
                direction.Normalize();
                Vector2 newVelocity = direction * _speed;
                _rigidbody.velocity = newVelocity;
                _visualDirection.HorizontalDirection = (direction.x < 0.0f) ? Direction.Left : Direction.Rigth;
            }
        }

        private bool SelectNewDestination()
        {
            bool isSelected = false;
            if (_haveWay)
            {
                _nexWayPointIndex++;
                if (_nexWayPointIndex >= _way.Length)
                {
                    if (_isCyclic)
                    {
                        _nexWayPointIndex = 0;
                        _destination = CalculateDestination(_way[_nexWayPointIndex]);
                        isSelected = true;
                    }
                }
                else
                {
                    _destination = CalculateDestination(_way[_nexWayPointIndex]);
                    isSelected = true;
                }
            }

            return isSelected;
        }

        private Vector2 CalculateDestination(Vector2 wayPoint)
        {
            if (_isRelativeStartPosition)
            {
                wayPoint += _startPosition;
            }

            return wayPoint;
        }

        public void SetWay(NpcDataWay way)
        {
            _haveWay = false;
            if (way.Way == null) return;
            int length = way.Way.Length;
            if (length == 0) return;
            
            _isCyclic = way.IsCyclic;
            _isRelativeStartPosition = way.IsRelativeStartPosition;
            _nexWayPointIndex = 0;
            
            _way = new Vector2[way.Way.Length];
            for (int i = 0; i < length; i++)
            {
                _way[i] = way.Way[i];
            }
            
            _haveWay = true;
        }

        private void StopMovement()
        {
            _rigidbody.velocity = Vector2.zero;
        }

        public void StopMovementLogic()
        {
            _isEnabled = false;
            StopMovement();
        }


        #region IExecutable

        public void Execute()
        {
            if (_isEnabled)
            {
                Vector2 direction = _destination - (Vector2)_transform.position;
                if (direction.sqrMagnitude <= _arrivalDistanceSqr)
                {
                    if (SelectNewDestination())
                    {
                        SetFlightDirection(_destination);
                    }
                    else
                    {
                        StopMovement();
                        StopMovementLogic();
                        OnWayFinished?.Invoke();
                    }
                }
            }
        }

        #endregion


        #region IActivatable

        public void Activate()
        {
            _nexWayPointIndex = 0;
            if (!_haveWay)
            {
                GenerateDefaultWay();
            }

            _isEnabled = true;
            _startPosition = _transform.position;
            _destination = CalculateDestination(_way[_nexWayPointIndex]);
            SetFlightDirection(_destination);
            
        }

        #endregion


        #region ICleanable

        public void Clear()
        {
            _isEnabled = false;
            _haveWay = false;
            _way = null;
        }

        #endregion

        private void GenerateDefaultWay()
        {
            Vector2 wayPoint = new Vector2();

            wayPoint.x = (_transform.position.x > 0.0f) ? -DEFAULT_X_WAY_POINT_POS : DEFAULT_X_WAY_POINT_POS;
            wayPoint.y = _transform.position.y;

            _way = new Vector2[1];
            _way[0] = wayPoint;

            _haveWay = true;
        }
    }
}