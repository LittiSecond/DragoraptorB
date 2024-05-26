using UnityEngine;
using Dragoraptor.Interfaces.Npc;


namespace Dragoraptor.Npc
{
    public class NpcDirectionByScale : INpcDirection
    {

        private Transform _transform;
        
        private float _startXScale;
        private Direction _isDirectionLeft;


        #region INpcDirection

        public Direction HorizontalDirection 
        { 
            
            get
            {
                return _isDirectionLeft;
            }
            set
            {
                if (_isDirectionLeft != value)
                {
                    Vector3 scale = _transform.localScale;

                    if (value == Direction.Left)
                    {
                        scale.x = _startXScale * (-1.0f);
                    }
                    else
                    {
                        scale.x = _startXScale;
                    }
                    _transform.localScale = scale;
                    _isDirectionLeft = value;
                }
            }
        }
        
        #endregion


        public NpcDirectionByScale(Transform transform)
        {
            _transform = transform;
            _startXScale = _transform.localScale.x;
            _isDirectionLeft = (_startXScale < 0.0f) ? Direction.Left : Direction.Rigth;
        }
        
        
    }
}