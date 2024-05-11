using UnityEngine;
using Dragoraptor.Interfaces;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor
{
    public class JumpCalculator : IJumpCalculator
    {
        
        private float _minJumpForce;
        private float _maxJumpForce;
        private float _energyCost;
        //      distances betwin _bodyTransform and touch point
        private float _cancelJumpDistance;
        private float _cancelJumpSqrDistance;
        private float _maxJumpForceDistance;

        private float _jumpForce;

        // force calculation data: Force = k*distance + b
        private float _k;
        private float _b;


        public JumpCalculator(IDataHolder dataHolder)
        {
            GamePlaySettings gps = dataHolder.GetGamePlaySettings();
            
            _minJumpForce = gps.MinJumpForce;
            _maxJumpForce = gps.MaxJumpForce;
            _energyCost = gps.JumpEnergyCost;
            _cancelJumpDistance = gps.NoJumpPowerIndicatorLength;
            _cancelJumpSqrDistance = _cancelJumpDistance * _cancelJumpDistance;
            _maxJumpForceDistance = gps.MaxJumpPowerIndicatorLength;

            CalculateJumpForceCalcData();
        }


        private void CalculateJumpForceCalcData()
        {
            _k = (_maxJumpForce - _minJumpForce) / (_maxJumpForceDistance - _cancelJumpDistance);
            _b = _maxJumpForce - _k * _maxJumpForceDistance;
        }


        #region IJumpCalculator
        
        public Vector2 CalculateJumpImpulse(Vector2 jumpDirection)
        {
            Vector2 impulse = Vector2.zero;

            float sqrDistance = jumpDirection.sqrMagnitude;

            if (sqrDistance > _cancelJumpSqrDistance)
            {
                if (CheckIsJumpDirectionGood(jumpDirection))
                {
                    jumpDirection.Normalize();

                    float distance = Mathf.Sqrt(sqrDistance);
                    if (distance > _maxJumpForceDistance)
                    {
                        distance = _maxJumpForceDistance;
                    }
                    _jumpForce = CalculateJumpForce(distance);
                    impulse = jumpDirection * _jumpForce;
                }
            }

            return impulse;
        }
        
        public float CalculateJumpCost()
        {
            return _jumpForce * _energyCost;
        }
        
        #endregion

        
        private bool CheckIsJumpDirectionGood(Vector2 direction)
        {
            return direction.y > 0.0f;
        }

        private float CalculateJumpForce(float distance)
        {
            return _k * distance + _b;
        }





    }
}