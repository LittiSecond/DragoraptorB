using UnityEngine.UIElements;

using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class HealthView : ResourceView
    {
        private const string HEALTH_VIEW_ROOT_NAME = "hp-root";
        private const string PROGRESS_BAR_NAME = "hp-bar";

        private IUiFactory _factory;
        
        
        public HealthView(IHealthObservable resource, IUiFactory factory) : base(resource)
        {
            _factory = factory;
        }


        public void Initialize()
        {
            VisualElement root = _factory.GetHuntScreen();
            VisualElement energyRoot = root.Q<VisualElement>(HEALTH_VIEW_ROOT_NAME);
            ProgressBar bar = energyRoot.Q<ProgressBar>(PROGRESS_BAR_NAME);
            base.Initialize(bar);
        }
    }
}