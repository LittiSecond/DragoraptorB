using UnityEngine;
using Dragoraptor.Interfaces;
using VContainer.Unity;


namespace Dragoraptor.Input
{
    public class TouchInput : IInput, ITickable
    {

        private readonly IInputHandler _handler;
        private readonly IPointerUiChecker _pointerUiChecker;

        private bool _isEnabled;


        public TouchInput(IInputHandler inputHandler, IPointerUiChecker pointerUiChecker)
        {
            _handler = inputHandler;
            _pointerUiChecker = pointerUiChecker;
        }

        #region IInput
        
        public void On()
        {
            _isEnabled = true;
        }

        public void Off()
        {
            _isEnabled = false;
        }
        
        #endregion


        #region ITickable
        
        public void Tick()
        {
            if (_isEnabled)
            {
                if (UnityEngine.Input.touchCount > 0)
                {
                    Touch touch = UnityEngine.Input.touches[0];
                    
                    //Debug.Log("TouchInput->Tick: UnityEngine.Input.touchCount > 0");
                    
                    if (!_pointerUiChecker.IsPointerUnderUiElement(touch.position))
                    {
                        _handler.HandleTouch(touch);
                    }
                }
            }
        }
        
        #endregion

    }
}