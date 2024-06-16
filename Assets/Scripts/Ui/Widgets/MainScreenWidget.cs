using UnityEngine;
using UnityEngine.UIElements;

using EventBus;

using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class MainScreenWidget : ScreenWidgetBase
    {
        private const string STATISIC_BUTTON_NAME = "statistic-button";
        private const string MENU_BUTTON_NAME = "menu-button";
        private const string HUNT_BUTTON_NAME = "hunt-button";

        private IEventBus _eventBus;
        private LevelsMapWidget _levelsMapWidget;

        private IScreenWidget _currentPanel;
        

        public MainScreenWidget(IUiFactory uiFactory,
            IEventBus eventBus,
            LevelsMapWidget levelsMapWidget
        ) : base(uiFactory)
        {
            _eventBus = eventBus;
            _levelsMapWidget = levelsMapWidget;
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

            _levelsMapWidget.OnCloseButtonClick += CloseCurrentPanel;
        }



        private void HuntButtonClick(ClickEvent e)
        {
            //Debug.Log("MainScreenWidget->HuntButtonClick: ");
            //_eventBus.Invoke(new StartHuntRequestSignal());

            if (_currentPanel != _levelsMapWidget as IScreenWidget)
            {
                _currentPanel?.Hide();
                _levelsMapWidget.Show();
                _currentPanel = _levelsMapWidget;
            }
        }

        private void MenuButtonClick(ClickEvent e)
        {
            Debug.Log("MainScreenWidget->MenuButtonClick: ");
        }

        private void StatisticButtonClick(ClickEvent e)
        {
            Debug.Log("MainScreenWidget->StatisticButtonClick: ");
        }

        private void CloseCurrentPanel()
        {
            _currentPanel?.Hide();
            _currentPanel = null;
        }

        public override void Show()
        {
            base.Show();
            _currentPanel?.Show();
        }

        public override void Hide()
        {
            base.Hide();
            _currentPanel?.Hide();
        }
    }
}