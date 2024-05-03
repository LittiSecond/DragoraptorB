
using UnityEngine.UIElements;
using EventBus;


namespace Dragoraptor.Ui
{
    public class HuntScreenWidget : ScreenWidgetBase
    {

        private const string MENU_BUTTON_NAME = "menu-button";

        private Button _menuButton;
        private HuntMenuWidget _menuWidget;
        private IEventBus _eventBus;


        private bool _isMenuOpened;
        
        
        public HuntScreenWidget(UiFactory uiFactory, HuntMenuWidget menuWidget, IEventBus eventBus) 
            : base(uiFactory)
        {
            _menuWidget = menuWidget;
            _eventBus = eventBus;
        }
        
        protected override void Initialise()
        {
            _root = _factory.GetHuntScreen();
            _menuButton = _root.Q<Button>(MENU_BUTTON_NAME);
            _menuButton.RegisterCallback<ClickEvent>(MenuButtonClick);
            _menuWidget.OnContinueButtonClick += ContinueButtonClick;
            _menuWidget.OnBreakButtonClick += BreakButtonClick;
        }

        private void MenuButtonClick(ClickEvent e)
        {
            if (_isMenuOpened)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        private void OpenMenu()
        {
            _menuWidget.Show();
            _isMenuOpened = true;
            _eventBus.Invoke(new HuntMenuOnOffSignal(_isMenuOpened));
        }

        private void CloseMenu()
        {
            _menuWidget.Hide();
            _isMenuOpened = false;
            _eventBus.Invoke(new HuntMenuOnOffSignal(_isMenuOpened));
        }

        private void ContinueButtonClick()
        {
            CloseMenu();
        }

        private void BreakButtonClick()
        {
            CloseMenu();
            _eventBus.Invoke(new StopHuntRequestSignal());
        }
        

    }
}