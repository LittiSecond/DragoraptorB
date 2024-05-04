using UnityEngine;
using Dragoraptor.Enums;
using Dragoraptor.Interfaces;
using Dragoraptor.Ui;
using EventBus;



namespace Dragoraptor.Core
{
    public class GameStateManager
    {


        private UiManager _uiManager;
        private IEventBus _eventBus;
        private ISceneController _sceneController;
        
        private GameState _gameState;

        
        public GameStateManager(UiManager uiManager, IEventBus eventBus, ISceneController sceneController)
        {
            _uiManager = uiManager;
            _eventBus = eventBus;
            _sceneController = sceneController;
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
                _sceneController.BuildLevel();
            }
        }

        private void SwitchToMainScreen(StopHuntRequestSignal signal)
        {
            Debug.Log("GameStateManager->SwitchToMainScreen:");
            if (_gameState == GameState.Game)
            {
                _uiManager.SwitchToMainScreen();
                _gameState = GameState.MainScreen;
                _sceneController.SetMainScreenScene();
            }
        }
        
    }
}