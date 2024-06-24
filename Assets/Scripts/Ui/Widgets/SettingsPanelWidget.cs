using System;
using UnityEngine.UIElements;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class SettingsPanelWidget : ScreenWidgetBase
    {
        private const string CLOSE_BUTTON_NAME = "close-button";
        
        public Action OnCloseButtonClick;
        
        private Button _closeButton;
        
        
        public SettingsPanelWidget(IUiFactory uiFactory) : base(uiFactory)
        {
            
        }

        protected override void Initialise()
        {
            _root = _factory.GetSettingsPanel();
            _closeButton = _root.Q<Button>(CLOSE_BUTTON_NAME);
            _closeButton.RegisterCallback<ClickEvent>(evt => OnCloseButtonClick?.Invoke());
        }
        
        
    }
}