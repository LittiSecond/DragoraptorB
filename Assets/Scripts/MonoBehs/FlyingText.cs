using UnityEngine;
using ObjPool;

using Dragoraptor.Interfaces;
using VContainer;


namespace Dragoraptor.MonoBehs
{
    public class FlyingText : PooledObject, IExecutable
    {
        
        private const float X_TO_REVERS_X_DIRECTION = 2.0f;

        [SerializeField] private TMPro.TextMeshPro _text;
        [SerializeField] private float _liveTime = 2.0f;
        [SerializeField] private float _xSpeed = 0.2f;
        [SerializeField] private float _ySpeed = 0.75f;
        [SerializeField] private float _fadeDuration = 0.75f;

        private IUpdateService _updateService;
        
        private Color _startColor;
        private Color _endColor;
        private float _timeCounter;
        private float _fadingTimeCounter;


        private void Awake()
        {
            _startColor = _text.color;
            _endColor = _startColor;
            _endColor.a = 0.0f;
        }

        [Inject]
        private void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
        }


        public void StartFlying(string text)
        {
            _text.text = text;
            _text.color = _startColor;
            _timeCounter = 0.0f;
            _fadingTimeCounter = 0.0f;
            _updateService.AddToUpdate(this);
        }

        private void StopFlying()
        {
            _updateService.RemoveFromUpdate(this);
        }

        public override void PrepareToReturnToPool()
        {
            StopFlying();
        }

        private float CalculateXSpeed()
        {
            float x = transform.position.x;
            float xSpeed = (x > X_TO_REVERS_X_DIRECTION)? -_xSpeed : _xSpeed;
            return xSpeed;
        }

        private void Fading(float deltaTime)
        {
            _fadingTimeCounter += deltaTime;
            Color newColor = Color.Lerp(_startColor, _endColor, _fadingTimeCounter / _fadeDuration);
            _text.color = newColor;
        }


        #region IExecutable

        public void Execute()
        {
            float deltaTime = Time.deltaTime;

            float xSpeed = CalculateXSpeed();

            transform.Translate(xSpeed * deltaTime, _ySpeed * deltaTime, 0.0f);
            _timeCounter += deltaTime;

            if (_timeCounter >= _liveTime - _fadeDuration)
            {
                Fading(deltaTime);
            }

            if (_timeCounter >= _liveTime)
            {
                StopFlying();
                ReturnToPool();
            }
        }

        #endregion
    }
}