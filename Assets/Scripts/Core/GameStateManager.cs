using UnityEngine;
using Dragoraptor.Enums;
using Dragoraptor.Ui;
using EventBus;



namespace Dragoraptor.Core
{
    public class GameStateManager
    {


        private UiManager _uiManager;
        private IEventBus _eventBus;
        
        private GameState _gameState;

        
        public GameStateManager(UiManager uiManager, IEventBus eventBus)
        {
            _uiManager = uiManager;
            _eventBus = eventBus;
        }
        
        public void StartProgram()
        {
            Debug.Log("GameStateManager->StartProgram:");
            if (_gameState == GameState.None)
            {
                _uiManager.SwitchToMainScreen();
                _gameState = GameState.MainScreen;
                _eventBus.Subscribe<StartHuntRequestSignal>(SwitchToHunt);
                _eventBus.Subscribe<StopHuntRequestSignal>(SwitchToMainScreen);
            }
        }


        private void SwitchToHunt(StartHuntRequestSignal signal)
        {
            Debug.Log("GameStateManager->SwitchToHunt:");
            if (_gameState == GameState.MainScreen)
            {
                _uiManager.SwitchToHunt();
                _gameState = GameState.Game;
            }
        }

        private void SwitchToMainScreen(StopHuntRequestSignal signal)
        {
            Debug.Log("GameStateManager->SwitchToMainScreen:");
            if (_gameState == GameState.Game)
            {
                _uiManager.SwitchToMainScreen();
                _gameState = GameState.MainScreen;
            }
        }
        
    }
}