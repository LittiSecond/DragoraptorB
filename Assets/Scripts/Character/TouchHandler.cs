using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;


namespace Dragoraptor.Character
{
    public class TouchHandler : IInputHandler, ICharStateListener
    {

        private IWalkLogic _walkLogic;
        private ISceneGeometry _sceneGeometry;
        private IAreaChecker _areaChecker;
        private IJumpController _jumpController;
        private IJumpPainter _jumpPainter;
        private CharacterState _state;


        public TouchHandler(IWalkLogic walkLogic, 
            ISceneGeometry sceneGeometry, 
            IAreaChecker areaChecker,
            IJumpController jumpController,
            IJumpPainter jumpPainter)
        {
            _walkLogic = walkLogic;
            _sceneGeometry = sceneGeometry;
            _areaChecker = areaChecker;
            _jumpController = jumpController;
            _jumpPainter = jumpPainter;
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
                        //     _attackController.TouchBegin();
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
                    //_horizontalDirection.SetTouchPosition(position);
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