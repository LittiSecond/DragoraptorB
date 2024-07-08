using System;
using Dragoraptor.Interfaces;
using UnityEngine;
using VContainer;


namespace Dragoraptor.MonoBehs
{
    public class PickUpItemNearWaveMovement : PickUpItem, IExecutable
    {
        [SerializeField] private float _xMax;
        [SerializeField] private float _yMax;
        [SerializeField] private float _xDuration;
        [SerializeField] private float _yDuration;
        [SerializeField] private float _xPhase;
        [SerializeField] private float _yPhase;
        
        
        private IUpdateService _updateService;
        private Vector3 _startPosition;
        private float _xTimeCounter;
        private float _yTimeCounter;
        

        [Inject]
        private void Construct2(IUpdateService updateService)
        {
            _updateService = updateService;
        }
        

        public override void Activate()
        {
            base.Activate();
            _startPosition = transform.position;
            _xTimeCounter = _xDuration * _xPhase * Mathf.PI;
            _yTimeCounter = _yDuration * _yPhase * Mathf.PI;
            _updateService.AddToUpdate(this);
        }

        private void OnDisable()
        {
            _updateService.RemoveFromUpdate(this);
        }

        #region IExecutable
        
        public void Execute()
        {
            float deltaTime = Time.deltaTime;
                
            _xTimeCounter += deltaTime;
            if (_xTimeCounter > _xDuration)
            {
                _xTimeCounter -= _xDuration;
            }
            float xPhase = _xTimeCounter / _xDuration * Mathf.PI * 2 ;
            float dx = Mathf.Cos(xPhase) * _xMax;
                
            _yTimeCounter += deltaTime;
            if (_yTimeCounter > _yDuration)
            {
                _yTimeCounter -= _yDuration;
            }
            float yPhase = _yTimeCounter / _yDuration * Mathf.PI * 2 ;
            float dy = Mathf.Sin(yPhase) * _yMax;

            Vector3 newPosition = new Vector3()
            {
                x = _startPosition.x + dx,
                y = _startPosition.y + dy,
                z = _startPosition.z
            };

            transform.position = newPosition;
        }
        
        #endregion
    }
}