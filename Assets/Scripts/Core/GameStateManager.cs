using UnityEngine;
using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.Ui;
using EventBus;



namespace Dragoraptor.Core
{
    public class GameStateManager
    {


        private UiManager _uiManager;
        private IEventBus _eventBus;
        private ISceneController _sceneController;
        private ICharacterManager _characterManager;
        
        
        private GameState _gameState;

        
        public GameStateManager(UiManager uiManager, 
            IEventBus eventBus, 
            ISceneController sceneController, 
            ICharacterManager characterManager)
        {
            _uiManager = uiManager;
            _eventBus = eventBus;
            _sceneController = sceneController;
            _characterManager = characterManager;
        }
        
        public void StartProgram()
        {
            Debug.Log("GameStateManager->StartProgram:");
            if (_gameState == GameState.None)
            {
                _uiManager.SwitchToMainScreen();
                _eventBus.Subscribe<StartHuntRequestSignal>(SwitchToHunt);
                _eventBus.Subscribe<StopHuntRequestSignal>(SwitchToMainScreen);
                _gameState = GameState.MainScreen;
            }
        }


        private void SwitchToHunt(StartHuntRequestSignal signal)
        {
            Debug.Log("GameStateManager->SwitchToHunt:");
            if (_gameState == GameState.MainScreen)
            {
                _gameState = GameState.Game;
                _uiManager.SwitchToHunt();
                _sceneController.BuildLevel();
                
                _characterManager.CreateCharacter();
                _characterManager.CharacterControlOn();
            }
        }

        private void SwitchToMainScreen(StopHuntRequestSignal signal)
        {
            Debug.Log("GameStateManager->SwitchToMainScreen:");
            if (_gameState == GameState.Game)
            {
                _gameState = GameState.MainScreen;
                _characterManager.CharacterControlOff();
                _characterManager.RemoveCharacter();
                _uiManager.SwitchToMainScreen();
                _sceneController.SetMainScreenScene();
            }
        }
        
    }
}