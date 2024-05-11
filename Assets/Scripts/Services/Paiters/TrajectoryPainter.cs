using UnityEngine;

using Dragoraptor.Interfaces;


namespace Dragoraptor.Painters
{
    sealed class TrajectoryPainter : BaseLinePainter
    {

        private const float TIME_STEP = 0.1f;

        private const int TRAECTORY_CAPACITY = 32;

        private IJumpCalculator _jumpCalculator;
        
        private Vector2 _gravity;

        private Vector2[] _traectory;
        private float _xMin;
        private float _xMax;

        private int _count;

        private bool _isInitialized;


        public void Initialize(IJumpCalculator jc, Rect visibleArea)
        {
            _jumpCalculator = jc;
            _traectory = new Vector2[TRAECTORY_CAPACITY];

            _gravity = Physics2D.gravity;

            _xMin = visibleArea.xMin;
            _xMax = visibleArea.xMax;

            _isInitialized = true;
        }
        
        public override void SetData(Transform bodyTransform, LineRenderer lineRenderer)
        {
            base.SetData(bodyTransform, lineRenderer);
            _lineRenderer.SetPosition(0, Vector2.zero);
        }

        public override void Execute()
        {
            if (!_isInitialized) return;

            Vector2 jumpDirection = _bodyPosition - _touchPosition;
            Vector2 impulse = _jumpCalculator.CalculateJumpImpulse(jumpDirection);

            if (impulse == Vector2.zero)
            {
                _lineRenderer.enabled = false;
            }
            else
            {
                _lineRenderer.enabled = true;

                CalculateTraectory(impulse);
                SetPositionsToLineRenderer();
            }
        }

        private void CalculateTraectory(Vector2 velocity)
        {
            float xMin = _xMin - _bodyTransform.position.x;
            float xMax = _xMax - _bodyTransform.position.x;
            _count = 1;

            for (int i = 1; i < TRAECTORY_CAPACITY; i++)
            {
                float time = i * TIME_STEP;

                Vector2 current;
                current.x = velocity.x * time;
                current.y = velocity.y * time + _gravity.y * time * time / 2;

                _traectory[i] = current;
                _count++;

                if ( current.x <= xMin || current.x > xMax || current.y <= 0.0f )
                {
                    break;
                }
            }
        }

        private void SetPositionsToLineRenderer()
        {
            if (_lineRenderer.positionCount != _count)
            {
                _lineRenderer.positionCount = _count;
            }

            for (int i = 1; i < _count; i++)
            {
                _lineRenderer.SetPosition(i, _traectory[i]);
            }
        }

    }
}