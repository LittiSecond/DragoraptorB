using System;
using Dragoraptor.Enums;
using UnityEngine;
using UnityEngine.UIElements;


namespace Dragoraptor.Ui
{
    public class HuntMenuWidget : ScreenBehaviourBase
    {

        private const string CONTINUE_BUTTON_NAME = "continue-button";
        private const string DEFEAT_BUTTON_NAME = "break-defeat-button";
        private const string VICTORY_BUTTON_NAME = "break-victory-button";

        public event Action OnContinueButtonClick;
        public event Action OnBreakButtonClick;

        private Button _continueButton;
        private Button _defeatButton;
        private Button _victoryButton;

        private HuntMenuState _menuState;

        private bool _isEnabled;
        
        
        public HuntMenuWidget(UiFactory uiFactory) : base(uiFactory)
        {
            
        }

        protected override void Initialise()
        {
            _root = _factory.GetHuntMenu();

            _continueButton = _root.Q<Button>(CONTINUE_BUTTON_NAME);
            _continueButton.RegisterCallback<ClickEvent>(evt => OnContinueButtonClick?.Invoke());

            _defeatButton = _root.Q<Button>(DEFEAT_BUTTON_NAME);
            _defeatButton.RegisterCallback<ClickEvent>(evt => OnBreakButtonClick?.Invoke());

            _victoryButton = _root.Q<Button>(VICTORY_BUTTON_NAME);
            _victoryButton.RegisterCallback<ClickEvent>(evt => OnBreakButtonClick?.Invoke());
        }
        
        #region IScreenBehaviour

        public override void Show()
        {
            base.Show();
            _isEnabled = true;
            UpdateState();
        }

        public override void Hide()
        {
            base.Hide();
            _isEnabled = false;
        }

        #endregion

        public void SetState(HuntMenuState state)
        {
            _menuState = state;

            if (_isEnabled)
            {
                UpdateState();
            }
        }

        private void UpdateState()
        {
            if (_menuState == HuntMenuState.Defeat)
            {
                _victoryButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                _defeatButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }
            else
            {
                _victoryButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                _defeatButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
        }
        
    }
}