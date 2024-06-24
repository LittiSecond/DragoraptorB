using UnityEngine.UIElements;


namespace Dragoraptor.Interfaces.Ui
{
    public interface IUiFactory
    {
        VisualElement GetMainScreen();
        VisualElement GetHuntScreen();
        VisualElement GetHuntMenu();
        VisualElement GetHuntResultWindow();
        VisualElement GetLevelsMapWindow();
        VisualElement GetNewMissionIndicator();
        VisualElement GetStatisticPanel();
        VisualElement GetSettingsPanel();
    }
}