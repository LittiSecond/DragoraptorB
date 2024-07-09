using UnityEngine;

using EventBus;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Npc;
using Dragoraptor.Interfaces.Score;
using Dragoraptor.Ui;
using TimersService;


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
        private ILevelProgress _levelProgress;
        private ICurrentLevelDescriptorHolder _levelDescriptorHolder;
        private ITimersService _timersService;
        
        
        private GameState _gameState;

        private float _charDeathDelay;
        private int _timerId;

        private bool _isPause;
        
        
        public GameStateManager(UiManager uiManager, 
            IEventBus eventBus, 
            ISceneController sceneController, 
            ICharacterManager characterManager,
            ILevelTimer timer,
            INpcManager npcManager,
            IObjectPoolManager poolManager,
            IScoreManager score,
            ILevelProgress levelProgress, 
            ICurrentLevelDescriptorHolder levelDescriptorHolder,
            ITimersService timersService,
            IDataHolder dataHolder
            )
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
            _levelProgress = levelProgress;
            _levelDescriptorHolder = levelDescriptorHolder;
            _timersService = timersService;
            _charDeathDelay = dataHolder.GetGamePlaySettings().CharacterDeathDelay;
        }
        
        public void StartProgram()
        {
            //Debug.Log("GameStateManager->StartProgram:");
            if (_gameState == GameState.None)
            {
                _uiManager.SwitchToMainScreen();
                _eventBus.Subscribe<StartHuntRequestSignal>(SwitchToHunt);
                _eventBus.Subscribe<StopHuntRequestSignal>(BreakHunt);
                _eventBus.Subscribe<LevelTimeUpSignal>(LevelTimeUp);
                _eventBus.Subscribe<ExitFromHuntRequestSignal>(CloseHunt);
                _eventBus.Subscribe<RestartHuntRequestSignal>(RestartHunt);
                _eventBus.Subscribe<HuntMenuOnOffSignal>(OnHuntMenuSwitch);
                _gameState = GameState.MainScreen;
            }
        }


        private void SwitchToHunt(StartHuntRequestSignal signal)
        {
            //Debug.Log("GameStateManager->SwitchToHunt:");
            if (_gameState == GameState.MainScreen && _levelDescriptorHolder.IsLevelReady())
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
                _levelProgress.LevelStart();
            }
        }

        private void SwitchToMainScreen()
        {
            //Debug.Log("GameStateManager->SwitchToMainScreen:");
            if (_gameState == GameState.Game)
            {
                _gameState = GameState.MainScreen;
                _npcManager.StopSpawn();
                _npcManager.ClearNps();
                _poolManager.ReturnAllToPool();
                _poolManager.ClearPool();
                //_characterManager.CharacterControlOff();
                _characterManager.RemoveCharacter();
                _levelTimer.StopTimer();
                _levelProgress.LevelEnd();
                _uiManager.SwitchToMainScreen();
                _sceneController.SetMainScreenScene();
                SwitchPause(false);
            }
        }


        private void BreakHunt(StopHuntRequestSignal signal)
        {
            //Debug.Log("GameStateManager->BreakHunt:");
            BreakHunt();
        }

        private void BreakHunt()
        {
            if (_gameState == GameState.Game)
            {
                _characterManager.CharacterControlOff();
                _levelTimer.StopTimer();
                _npcManager.StopSpawn();
                //_levelProgress.LevelEnd();
                _uiManager.ShowEndHuntWindow();
                SwitchPause(true);
            }
        }

        private void CharacterKilled()
        {
            //Debug.Log("GameStateManager->CharacterKilled:");
            _levelTimer.StopTimer();
            _timerId = _timersService.AddTimer(CharacterKilledTimer, _charDeathDelay);
        }

        private void CharacterKilledTimer()
        {
            _timerId = 0;
            BreakHunt();
        }

        private void LevelTimeUp(LevelTimeUpSignal signal)
        {
            //Debug.Log("GameStateManager->LevelTimeUp:");
            BreakHunt();
        }

        private void CloseHunt(ExitFromHuntRequestSignal signal)
        {
            //Debug.Log("GameStateManager->CloseHunt:");
            SwitchToMainScreen();
        }

        private void RestartHunt(RestartHuntRequestSignal signal)
        {
            //Debug.Log("GameStateManager->RestartHunt:");
            if (_gameState == GameState.Game)
            {
                if (_timerId > 0)
                {
                    _timersService.RemoveTimer(_timerId);
                    _timerId = 0;
                }
                
                _levelProgress.LevelEnd();
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
                _levelProgress.LevelStart();
                SwitchPause(false);
            }
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