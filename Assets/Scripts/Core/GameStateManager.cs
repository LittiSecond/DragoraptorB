using UnityEngine;

using EventBus;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Npc;
using Dragoraptor.Interfaces.Score;
using Dragoraptor.Ui;


namespace Dragoraptor.Core
{
    public class GameStateManager
    {


        private UiManager _uiManager;
        private IEventBus _eventBus;
        private ISceneController _sceneController;
        private ICharacterManager _characterManager;
        private ILevelTimer _levelTimer;
        private INpcManager _npcManager;
        private IObjectPoolManager _poolManager;
        private IScoreManager _score;
        
        
        private GameState _gameState;

        private bool _isPause;
        
        
        public GameStateManager(UiManager uiManager, 
            IEventBus eventBus, 
            ISceneController sceneController, 
            ICharacterManager characterManager,
            ILevelTimer timer,
            INpcManager npcManager,
            IObjectPoolManager poolManager,
            IScoreManager score)
        {
            _uiManager = uiManager;
            _eventBus = eventBus;
            _sceneController = sceneController;
            _characterManager = characterManager;
            _characterManager.OnCharacterKilled += CharacterKilled;
            _levelTimer = timer;
            _npcManager = npcManager;
            _poolManager = poolManager;
            _score = score;
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
                _eventBus.Subscribe<HuntMenuOnOffSignal>(OnHuntMenuSwitch);
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
                _poolManager.PreparePool();
                _npcManager.PrepareSpawn();
                _npcManager.RestartSpawn();
                _score.ClearScore();
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
                _npcManager.StopSpawn();
                _npcManager.ClearNps();
                _poolManager.ReturnAllToPool();
                _poolManager.ClearPool();
                //_characterManager.CharacterControlOff();
                _characterManager.RemoveCharacter();
                //_levelTimer.StopTimer();
                _uiManager.SwitchToMainScreen();
                _sceneController.SetMainScreenScene();
                SwitchPause(false);
            }
        }


        private void BreakHunt(StopHuntRequestSignal signal)
        {
            Debug.Log("GameStateManager->BreakHunt:");
            BreakHunt();
        }

        private void BreakHunt()
        {
            if (_gameState == GameState.Game)
            {
                _characterManager.CharacterControlOff();
                _levelTimer.StopTimer();
                _npcManager.StopSpawn();
                _uiManager.ShowEndHuntWindow();
                SwitchPause(true);
            }
        }

        private void CharacterKilled()
        {
            Debug.Log("GameStateManager->CharacterKilled:");
            BreakHunt();
        }

        private void LevelTimeUp(LevelTimeUpSignal signal)
        {
            Debug.Log("GameStateManager->LevelTimeUp:");
            if (_gameState == GameState.Game)
            {
                _characterManager.CharacterControlOff();
                _levelTimer.StopTimer();
                _npcManager.StopSpawn();
                _uiManager.ShowEndHuntWindow();
                SwitchPause(true);
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
                _npcManager.StopSpawn();
                _poolManager.ReturnAllToPool();
                _characterManager.RemoveCharacter();
                
                _characterManager.CreateCharacter();
                _characterManager.CharacterControlOn();
                _score.ClearScore();
                _levelTimer.StartTimer();
                _npcManager.RestartSpawn();
                SwitchPause(false);
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
        
        private void SwitchPause(bool isPauseOn)
        {
            if (_isPause && !isPauseOn)
            {
                _isPause = false;
                Time.timeScale = 1.0f;
            }
            else if ( !_isPause && isPauseOn)
            {
                _isPause = true;
                Time.timeScale = 0.0f;
            }
        }

        private void OnHuntMenuSwitch(HuntMenuOnOffSignal signal)
        {
            if (_gameState == GameState.Game)
            {
                SwitchPause(signal.IsOpened);
            }
        }

    }
}