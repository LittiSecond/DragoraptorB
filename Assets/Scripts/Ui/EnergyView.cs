using UnityEngine.UIElements;

using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class EnergyView : ResourceView
    {

        private const string ENERGY_VIEW_ROOT_NAME = "energy-root";
        private const string PROGRESS_BAR_NAME = "energy-bar";

        private IUiFactory _factory;
        
        
        public EnergyView(IEnergyObservable resource, IUiFactory factory) : base(resource)
        {
            _factory = factory;
        }


        public void Initialize()
        {
            VisualElement root = _factory.GetHuntScreen();
            VisualElement energyRoot = root.Q<VisualElement>(ENERGY_VIEW_ROOT_NAME);
            ProgressBar bar = energyRoot.Q<ProgressBar>(PROGRESS_BAR_NAME);
            base.Initialize(bar);
        }

    }
}