using System.Collections.Generic;
using System.Threading.Tasks;
using Dragoraptor.Core;
using UnityEngine;
using UnityEngine.UIElements;

using EventBus;

using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class HuntScreenWidget : ScreenWidgetBase
    {

        private const string MENU_BUTTON_NAME = "menu-button";

        private Button _menuButton;
        private HuntMenuWidget _menuWidget;
        private HuntResultWidget _huntResultWidget;
        private IEventBus _eventBus;
        private IReadOnlyList<IHuntUiInitializable> _toInitializeObjects;


        private bool _isMenuOpened;
        private bool _isEndHuntScreenOpen;
        
        
        public HuntScreenWidget(IUiFactory uiFactory, 
            HuntMenuWidget menuWidget, 
            IEventBus eventBus,
            HuntResultWidget resultWidget,
            IReadOnlyList<IHuntUiInitializable> initializables) 
            : base(uiFactory)
        {
            _menuWidget = menuWidget;
            _eventBus = eventBus;
            _huntResultWidget = resultWidget;
            _toInitializeObjects = initializables;
        }
        
        protected override void Initialise()
        {
            _root = _factory.GetHuntScreen();
            _menuButton = _root.Q<Button>(MENU_BUTTON_NAME);
            _menuButton.RegisterCallback<ClickEvent>(MenuButtonClick);
            _menuWidget.OnContinueButtonClick += ContinueButtonClick;
            _menuWidget.OnBreakButtonClick += BreakHuntButtonClick;

            for (int i = 0; i < _toInitializeObjects.Count; i++)
            {
                _toInitializeObjects[i].InitializeUi();
            }
            
            _huntResultWidget.AddListeners(GetOutOfTheHuntButtonClick, RestartHuntButtonClick);
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

        private void BreakHuntButtonClick()
        {
            CloseMenu();
            _eventBus.Invoke(new StopHuntRequestSignal());
            
        }

        public void ShowEndHuntScreen()
        {
            ShowEndHuntScreenDelay();
        }

        private async void ShowEndHuntScreenDelay()
        {
            await Task.Delay(100);
            _huntResultWidget.Show();
            _isEndHuntScreenOpen = true;
        }

        private void HideEndHuntScreen()
        {
            _huntResultWidget.Hide();
            _isEndHuntScreenOpen = false;
        }

        private void RestartHuntButtonClick()
        {
            Debug.Log("HuntScreenWidget->RestartHuntButtonClick:");
            if (_isEndHuntScreenOpen)
            {
                HideEndHuntScreen();
            }

            if (_isMenuOpened)
            {
                CloseMenu();
            }
            
            _eventBus.Invoke(new RestartHuntRequestSignal());
        }
        
        private void GetOutOfTheHuntButtonClick()
        {
            Debug.Log("HuntScreenWidget->GetOutOfTheHuntButtonClick:");
            if (_isEndHuntScreenOpen)
            {
                HideEndHuntScreen();
            }

            if (_isMenuOpened)
            {
                CloseMenu();
            }
            _eventBus.Invoke(new ExitFromHuntRequestSignal());
        }

    }
}