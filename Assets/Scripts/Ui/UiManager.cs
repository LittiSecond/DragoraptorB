using Dragoraptor.Interfaces.Ui;
using UnityEngine;


namespace Dragoraptor.Ui
{
    public class UiManager
    {

        private IScreenBehaviour _mainScreen;
        private IScreenBehaviour _huntScreen;
        private IScreenBehaviour _currentScreen;
        

        public UiManager(MainScreenWidget mainScreen, HuntScreenWidget huntScreen)
        {
            _mainScreen = mainScreen;
            _huntScreen = huntScreen;
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
        
    }
}