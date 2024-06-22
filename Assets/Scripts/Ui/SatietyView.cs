using UnityEngine;
using UnityEngine.UIElements;

using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class SatietyView : ResourceView, IHuntUiInitializable
    {

        private const float TO_PERCENT_MULTIPLER = 100.0f;
        
        private const string SATIETY_VIEW_ROOT_NAME = "satiety-root";
        private const string PROGRESS_BAR_NAME = "satiety-bar";
        private const string MARKER_NAME = "satiety-marker";

        private VisualElement _victoryMarker;
        private IUiFactory _factory;
        private ISatietyObservable _satietySource;


        public SatietyView(ISatietyObservable resource, IUiFactory factory) : base(resource)
        {
            _factory = factory;
            _satietySource = resource;
        }

        #region IHuntUiInitializable
        
        public void InitializeUi()
        {
            VisualElement root = _factory.GetHuntScreen();
            VisualElement satietyRoot = root.Q<VisualElement>(SATIETY_VIEW_ROOT_NAME);
            ProgressBar bar = satietyRoot.Q<ProgressBar>(PROGRESS_BAR_NAME);
            _victoryMarker = satietyRoot.Q<VisualElement>(MARKER_NAME);
            _satietySource.OnVictorySatietyChanged += VictorySatietyChanged;
            base.Initialize(bar);
        }
        
        #endregion

        private void VictorySatietyChanged(float satietyRelativeMax)
        {
            if (satietyRelativeMax >= 0.0 && satietyRelativeMax <= 1.0)
            {
                _victoryMarker.style.left = new StyleLength(Length.Percent(satietyRelativeMax * TO_PERCENT_MULTIPLER));
            }
        }
        
    }
}