
using UnityEngine.UIElements;

using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class HuntScreenWidget : ScreenBehaviourBase
    {
        
        
        
        public HuntScreenWidget(UiFactory uiFactory) : base(uiFactory)
        {

        }
        
        protected override void Initialise()
        {
            _root = _factory.GetHuntScreen();
        }
        
        

    }
}