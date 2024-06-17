using System;
using System.Threading.Tasks;
using Dragoraptor.Interfaces.MissionMap;
using UnityEngine;
using UnityEngine.UIElements;

using EventBus;

using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class LevelsMapWidget : ScreenWidgetBase
    {

        private const string CLOSE_BUTTON_NAME = "close-button";
        private const string START_HUNT_BUTTON_NAME = "start-hunt-button";
        private const string LEVEL_MAP_PANEL_NAME = "map-image";
        
        public Action OnCloseButtonClick;

        private Button _closeButton;
        private Button _startHuntButton;
        private VisualElement _levelMapPanel;
        
        private IEventBus _eventBus;
        private ILevelsMapController _levelMapController;
        
        
        
        
        public LevelsMapWidget(IUiFactory uiFactory, 
            IEventBus eventBus,
            ILevelsMapController levelMap
            ) : base(uiFactory)
        {
            _eventBus = eventBus;
            _levelMapController = levelMap;
        }

        protected override void Initialise()
        {
            _root = _factory.GetLevelsMapWindow();
            _closeButton = _root.Q<Button>(CLOSE_BUTTON_NAME);
            _closeButton.RegisterCallback<ClickEvent>(evt => OnCloseButtonClick?.Invoke());
            _startHuntButton = _root.Q<Button>(START_HUNT_BUTTON_NAME);
            _startHuntButton.RegisterCallback<ClickEvent>(StartHuntButtonClick);
            _levelMapPanel = _root.Q<VisualElement>(LEVEL_MAP_PANEL_NAME);
            _levelMapController.SetMapRoot(_levelMapPanel);

            //DoResize();
        }

        private void StartHuntButtonClick(ClickEvent evt)
        {
            Debug.Log("LevelsMapWidget->StartHuntButtonClick: ");
            _eventBus.Invoke(new StartHuntRequestSignal());
        }

        private async void DoResize()
        {
            await Task.Delay(100);
            float width = _root.style.width.value.value;
            float height = width * 1.5f;
            _root.style.height = new StyleLength(height);
            Debug.Log($"LevelsMapWidget->DoResize: width = {width}, height = {height} ");
        }

    }
}