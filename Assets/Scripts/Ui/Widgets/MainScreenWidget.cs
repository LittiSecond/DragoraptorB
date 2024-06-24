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
        private SettingsPanelWidget _settingsPanel;
        private StatisticPanelWidget _statisticPanel;

        private IScreenWidget _currentPanel;
        

        public MainScreenWidget(IUiFactory uiFactory,
            IEventBus eventBus,
            LevelsMapWidget levelsMapWidget, 
            SettingsPanelWidget settingsPanel,
            StatisticPanelWidget statisticPanel
        ) : base(uiFactory)
        {
            _eventBus = eventBus;
            _levelsMapWidget = levelsMapWidget;
            _settingsPanel = settingsPanel;
            _statisticPanel = statisticPanel;
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
            _statisticPanel.OnCloseButtonClick += CloseCurrentPanel;
            _settingsPanel.OnCloseButtonClick += CloseCurrentPanel;
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
            //Debug.Log("MainScreenWidget->MenuButtonClick: ");
            if (_currentPanel != _settingsPanel as IScreenWidget)
            {
                _currentPanel?.Hide();
                _settingsPanel.Show();
                _currentPanel = _settingsPanel;
            }
        }

        private void StatisticButtonClick(ClickEvent e)
        {
            //Debug.Log("MainScreenWidget->StatisticButtonClick: ");
            if (_currentPanel != _statisticPanel as IScreenWidget)
            {
                _currentPanel?.Hide();
                _statisticPanel.Show();
                _currentPanel = _statisticPanel;
            }
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