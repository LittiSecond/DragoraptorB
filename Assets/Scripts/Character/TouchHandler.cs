using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;


namespace Dragoraptor.Character
{
    public class TouchHandler : IInputHandler, ICharStateListener
    {

        private readonly IWalkLogic _walkLogic;
        private readonly ISceneGeometry _sceneGeometry;
        private readonly IAreaChecker _areaChecker;
        private readonly IJumpController _jumpController;
        private readonly IJumpPainter _jumpPainter;
        private readonly ICharHorizontalDirection _charDirection;
        private readonly IAttackController _attackController;
        private CharacterState _state;


        public TouchHandler(IWalkLogic walkLogic, 
            ISceneGeometry sceneGeometry, 
            IAreaChecker areaChecker,
            IJumpController jumpController,
            IJumpPainter jumpPainter,
            ICharHorizontalDirection charDirection, 
            IAttackController attackController)
        {
            _walkLogic = walkLogic;
            _sceneGeometry = sceneGeometry;
            _areaChecker = areaChecker;
            _jumpController = jumpController;
            _jumpPainter = jumpPainter;
            _charDirection = charDirection;
            _attackController = attackController;
        }
        
        #region IInputHandler
        
        public void HandleTouch(Touch touch)
        {
            if (_state == CharacterState.Idle || _state == CharacterState.Walk)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 position = _sceneGeometry.ConvertScreenPositionToWorld(touch.position);
                    ObjectType touchedObjectType = _areaChecker.CheckPoint(position);
                    if (touchedObjectType == ObjectType.Ground)
                    {
                        _walkLogic.SetDestination(position.x);
                        //_horizontalDirection.SetDestination(position);
                    }
                    else if (touchedObjectType == ObjectType.Player)
                    {
                        _jumpController.TouchBegin();
                        _jumpPainter.SetTouchPosition(position);
                    }
                    else
                    {
                        _attackController.TouchBegin();
                    }

                }
                
            }
            else if (_state == CharacterState.PrepareJump)
            {
                
                Vector2 position = _sceneGeometry.ConvertScreenPositionToWorld(touch.position);
                if (touch.phase == TouchPhase.Ended)
                {
                    _jumpController.TouchEnd(position);
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    _jumpPainter.SetTouchPosition(position);
                    _charDirection.TouchPrepareJump(position);
                    //_horizontalDirection.SetTouchPosition(position);
                }

            }
            else if (_state == CharacterState.FliesUp || _state == CharacterState.FliesDown)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _attackController.TouchBegin();
                }
            }
        }
        
        #endregion


        #region ICharStateListener
        
        public void StateChanged(CharacterState newState)
        {
            _state = newState;
        }
        
        #endregion
        
    }
}