using UnityEngine.UIElements;


namespace Dragoraptor.Interfaces.Ui
{
    public interface IUiFactory
    {
        VisualElement GetMainScreen();
        VisualElement GetHuntScreen();
        VisualElement GetHuntMenu();
    }
}