using System;
using UnityEngine;
using UnityEngine.UIElements;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class HuntMenuWidget : ScreenWidgetBase
    {

        private const string CONTINUE_BUTTON_NAME = "continue-button";
        private const string DEFEAT_BUTTON_NAME = "break-defeat-button";
        private const string VICTORY_BUTTON_NAME = "break-victory-button";
        private const string ROOT_STYLE_NAME = "menu-root";

        public event Action OnContinueButtonClick;
        public event Action OnBreakButtonClick;

        private Button _continueButton;
        private Button _defeatButton;
        private Button _victoryButton;

        private IVictoryPossibilityHolder _victoryInfoSource;

        
        public HuntMenuWidget(IUiFactory uiFactory, IVictoryPossibilityHolder victoryPossibilityHolder) 
            : base(uiFactory)
        {
            _victoryInfoSource = victoryPossibilityHolder;
        }

        protected override void Initialise()
        {
            _root = _factory.GetHuntMenu();
            
            _root.AddToClassList(ROOT_STYLE_NAME);

            _continueButton = _root.Q<Button>(CONTINUE_BUTTON_NAME);
            _continueButton.RegisterCallback<ClickEvent>(evt => OnContinueButtonClick?.Invoke());

            _defeatButton = _root.Q<Button>(DEFEAT_BUTTON_NAME);
            _defeatButton.RegisterCallback<ClickEvent>(evt => OnBreakButtonClick?.Invoke());

            _victoryButton = _root.Q<Button>(VICTORY_BUTTON_NAME);
            _victoryButton.RegisterCallback<ClickEvent>(evt => OnBreakButtonClick?.Invoke());
        }

        
        #region IScreenWidget

        public override void Show()
        {
            base.Show();
            UpdateState();
        }

        #endregion
        

        private void UpdateState()
        {
            if (_victoryInfoSource.IsVictory)
            {
                _victoryButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                _defeatButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
            else
            {
                _victoryButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                _defeatButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }
        }
        
    }
}