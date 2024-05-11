using UnityEngine;

using VContainer.Unity;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;
using Dragoraptor.Painters;


namespace Dragoraptor.Character
{
    public class JumpPainter : IJumpPainter, IBodyUser, ICharStateListener, ITickable
    {

        private PowerLinePainter _powerLinePainter;
        private TrajectoryPainter _trajectoryPainter;
        private IJumpCalculator _jumpCalculator;
        private ISceneGeometry _sceneGeometry;

        private CharacterState _state;

        private bool _isEnabled;
        private bool _haveBody;
        private bool _isInitialized;


        public JumpPainter(IDataHolder dataHolder, IJumpCalculator calculator, ISceneGeometry sceneGeometry)
        {
            _powerLinePainter = new PowerLinePainter(dataHolder.GetGamePlaySettings());
            _trajectoryPainter = new TrajectoryPainter();
            _jumpCalculator = calculator;
            _sceneGeometry = sceneGeometry;
        }
        

        #region IJumpPainter
        
        public void SetTouchPosition(Vector2 position)
        {
            _powerLinePainter.SetTouchPosition(position);
            _trajectoryPainter.SetTouchPosition(position);
        }
        
        #endregion


        #region IBodyUser
        
        public void SetBody(PlayerBody playerBody)
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            
            (LineRenderer, LineRenderer) lr = playerBody.GetLineRenderers();
            _powerLinePainter.SetData(playerBody.transform, lr.Item2);
            _trajectoryPainter.SetData(playerBody.transform, lr.Item1);
            _haveBody = true;
        }

        public void ClearBody()
        {
            _powerLinePainter.ClearData();
            _trajectoryPainter.ClearData();
            _haveBody = false;
        }
        
        #endregion


        #region ICharStateListener

        public void StateChanged(CharacterState newState)
        {
            if (newState != _state)
            {
                if (newState == CharacterState.PrepareJump)
                {
                    DrawingOn();
                }
                else if (_state == CharacterState.PrepareJump)
                {
                    DrawingOff();
                }

                _state = newState;
            }
        }
        
        #endregion


        #region ITickable
        
        public void Tick()
        {
            if (_isEnabled )
            {
                _powerLinePainter.Execute();
                _trajectoryPainter.Execute();
            }
        }
        
        #endregion

        private void Initialize()
        {
            _trajectoryPainter.Initialize(_jumpCalculator, _sceneGeometry.GetVisibleArea());
            _isInitialized = true;
        }

        private void DrawingOn()
        {
            if (_haveBody)
            {
                _powerLinePainter.DrawingOn();
                _trajectoryPainter.DrawingOn();
                _isEnabled = true;
            }
        }

        private void DrawingOff()
        {
            if (_haveBody)
            {
                _powerLinePainter.DrawingOff();
                _trajectoryPainter.DrawingOff();
            }
            _isEnabled = false;
        }
        
    }
}