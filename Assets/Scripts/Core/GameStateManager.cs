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
        private ILevelTimer _levelTimer;
        
        
        private GameState _gameState;

        
        public GameStateManager(UiManager uiManager, 
            IEventBus eventBus, 
            ISceneController sceneController, 
            ICharacterManager characterManager,
            ILevelTimer timer)
        {
            _uiManager = uiManager;
            _eventBus = eventBus;
            _sceneController = sceneController;
            _characterManager = characterManager;
            _levelTimer = timer;
        }
        
        public void StartProgram()
        {
            Debug.Log("GameStateManager->StartProgram:");
            if (_gameState == GameState.None)
            {
                _uiManager.SwitchToMainScreen();
                _eventBus.Subscribe<StartHuntRequestSignal>(SwitchToHunt);
                _eventBus.Subscribe<StopHuntRequestSignal>(BreakHunt);
                _eventBus.Subscribe<LevelTimeUpSignal>(LevelTimeUp);
                _eventBus.Subscribe<CloseHuntRequestSignal>(CloseHunt);
                _eventBus.Subscribe<RestartHuntRequestSignal>(RestartHunt);
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
                _levelTimer.StartTimer();
                
                _characterManager.CreateCharacter();
                _characterManager.CharacterControlOn();
            }
        }

        private void SwitchToMainScreen()
        {
            Debug.Log("GameStateManager->SwitchToMainScreen:");
            if (_gameState == GameState.Game)
            {
                _gameState = GameState.MainScreen;
                //_characterManager.CharacterControlOff();
                _characterManager.RemoveCharacter();
                //_levelTimer.StopTimer();
                _uiManager.SwitchToMainScreen();
                _sceneController.SetMainScreenScene();
            }
        }


        private void BreakHunt(StopHuntRequestSignal signal)
        {
            Debug.Log("GameStateManager->BreakHunt:");
            if (_gameState == GameState.Game)
            {
                _characterManager.CharacterControlOff();
                _levelTimer.StopTimer();
                _uiManager.ShowEndHuntWindow();
            }
        }

        private void LevelTimeUp(LevelTimeUpSignal signal)
        {
            Debug.Log("GameStateManager->LevelTimeUp:");
            if (_gameState == GameState.Game)
            {
                _characterManager.CharacterControlOff();
                _levelTimer.StopTimer();
                _uiManager.ShowEndHuntWindow();
            }
        }

        private void CloseHunt(CloseHuntRequestSignal signal)
        {
            Debug.Log("GameStateManager->CloseHunt:");
            SwitchToMainScreen();
        }

        private void RestartHunt(RestartHuntRequestSignal signal)
        {
            Debug.Log("GameStateManager->RestartHunt:");
            if (_gameState == GameState.Game)
            {
                _characterManager.CharacterControlOff();
                _levelTimer.StopTimer();
                _characterManager.RemoveCharacter();
                
                _characterManager.CreateCharacter();
                _characterManager.CharacterControlOn();
                _levelTimer.StartTimer();
            }
            
            // if (_state == GameState.Hunt)
            // {
            //     _levelProgressControler.LevelEnd();
            //     _timeController.StopTimer();
            //     _npcManager.StopNpcSpawn();
            //     _npcManager.ClearNpc();
            //     DeactivateCharacterControl();
            //     _characterController.RemoveCharacter();
            //     _sceneController.ClearTemporaryObjects();
            //     
            //     _levelProgressControler.RegistrateHuntResults();
            //
            //     _characterController.CreateCharacter();
            //     ActivateCharacterControl();
            //     _npcManager.RestartNpcSpawn();
            //     _victoryController.LevelStart();
            //     _levelProgressControler.LevelStart();
            //     _timeController.StartTimer();
            //     SwitchPause(false);
            // }
        }
        
    }
}