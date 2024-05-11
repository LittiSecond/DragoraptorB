using UnityEngine;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Painters
{
    sealed class PowerLinePainter : BaseLinePainter
    {

        private float _maxLength;

        public PowerLinePainter(GamePlaySettings gamePlaySettings)
        {
            _maxLength = gamePlaySettings.MaxJumpPowerIndicatorLength;
        }


        public override void Execute()
        {
            Vector2 secondPoint = _touchPosition - _bodyPosition;

            if (secondPoint.sqrMagnitude > _maxLength * _maxLength)
            {
                secondPoint.Normalize();
                secondPoint *= _maxLength;
            }

            _lineRenderer.SetPosition(1, (Vector3)secondPoint);
        }
    }
}