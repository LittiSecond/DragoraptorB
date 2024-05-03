using UnityEngine;
using UnityEngine.UIElements;

using EventBus;


namespace Dragoraptor.Ui
{
    public class MainScreenWidget : ScreenWidgetBase
    {
        private const string STATISIC_BUTTON_NAME = "statistic-button"; 
        private const string MENU_BUTTON_NAME = "menu-button"; 
        private const string HUNT_BUTTON_NAME = "hunt-button";

        private IEventBus _eventBus;
        

        public MainScreenWidget(UiFactory uiFactory, IEventBus eventBus) : base(uiFactory)
        {
            _eventBus = eventBus;
        }

        protected override void Initialise()
        {
            _root = _factory.GetMainScreen();
            Button huntButton = _root.Q<Button>(HUNT_BUTTON_NAME);
            huntButton.RegisterCallback<ClickEvent>(HuntButtonClick);

            Button menuButton = _root.Q<Button>(MENU_BUTTON_NAME);
            menuButton.RegisterCallback<ClickEvent>(MenuButtonClick);

            Button statButton = _root.Q<Button>(STATISIC_BUTTON_NAME);
            statButton.RegisterCallback<ClickEvent>(StatisticButtonClick);
        }



        private void HuntButtonClick(ClickEvent e)
        {
            Debug.Log("MainScreenWidget->HuntButtonClick: ");
            _eventBus.Invoke(new StartHuntRequestSignal());
        }
        
        private void MenuButtonClick(ClickEvent e)
        {
            Debug.Log("MainScreenWidget->MenuButtonClick: ");
        }
        
        private void StatisticButtonClick(ClickEvent e)
        {
            Debug.Log("MainScreenWidget->StatisticButtonClick: ");
        }

    }
}