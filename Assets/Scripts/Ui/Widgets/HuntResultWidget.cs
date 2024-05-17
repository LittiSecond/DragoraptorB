using System;
using UnityEngine.UIElements;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class HuntResultWidget : ScreenWidgetBase
    {

        private const string FINISH_BUTTON_NAME = "finish-button";
        private const string RESTART_BUTTON_NAME = "restart-button";

        private Button _finishButton;
        private Button _restartButton;
        
        private Action _restartListener;
        private Action _finishListener;
        
        
        
        public HuntResultWidget(IUiFactory uiFactory) : base(uiFactory)
        {
        }

        protected override void Initialise()
        {
            _root = _factory.GetHuntResultWindow();
            _finishButton = _root.Q<Button>(FINISH_BUTTON_NAME);
            _finishButton.RegisterCallback<ClickEvent>(evt => _finishListener());
            _restartButton = _root.Q<Button>(RESTART_BUTTON_NAME);
            _restartButton.RegisterCallback<ClickEvent>(evt=>_restartListener());
        }
        
        public void AddListeners(Action finishListener, Action restartListener)
        {
            _finishListener = finishListener;
            _restartListener = restartListener;
        }
        
        
    }
}