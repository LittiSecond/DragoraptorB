using Dragoraptor.Interfaces.Ui;
using UnityEngine;


namespace Dragoraptor.Ui
{
    public class UiManager
    {

        private HuntScreenWidget _huntScreenWidget;
        
        private IScreenWidget _mainScreen;
        private IScreenWidget _huntScreen;
        private IScreenWidget _currentScreen;
        

        public UiManager(MainScreenWidget mainScreen, HuntScreenWidget huntScreen)
        {
            _mainScreen = mainScreen;
            _huntScreen = huntScreen;
            _huntScreenWidget = huntScreen;
        }

        public void SwitchToMainScreen()
        {
            Debug.Log("UiManager->SwitchToMainScreen:");

            _currentScreen?.Hide();
            _currentScreen = _mainScreen;
            _currentScreen.Show();
        }


        public void SwitchToHunt()
        {
            Debug.Log("UiManager->SwitchToHunt:");
            
            _currentScreen?.Hide();
            _currentScreen = _huntScreen;
            _currentScreen.Show();
        }

        public void ShowEndHuntWindow()
        {
            _huntScreenWidget.ShowEndHuntScreen();
        }
        
    }
}