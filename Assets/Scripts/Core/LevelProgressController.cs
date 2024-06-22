using UnityEngine;

using EventBus;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Score;
using Dragoraptor.Models;


namespace Dragoraptor.Core
{
    public class LevelProgressController : ILevelProgress, IHuntResultsSource, IVictoryPossibilityHolder
    {

        private readonly ISatietyObservable _playerSatiety;
        private readonly IScoreSource _scoreSource;
        private readonly IHealthObservable _health;
        private readonly IEventBus _eventBus;
        private readonly ICurrentLevelDescriptorHolder _levelDescriptorHolder;
        private readonly IGameProgressCollector _gameProgressCollector;
        private HuntResults _lastHuntResults;

        private float _satietyToSuccess;
        private float _victoryScoreMultipler;
        private float _defeatScoreMultipler;
        private float _nullSatietyScoreMultipler;
        private float _satietyConditionScoreMultipler;

        private float _scoreСoefficient;

        private bool _isCharacterAlive;
        private bool _isSatietyConditionMet;
        private bool _isSatietyFull;
        private bool _isTimeUp;


        #region IVictoryPossibilityHolder

        public bool IsVictory
        {
            get
            {
                return IsVictoryCheck();
            }
        }

        #endregion
        
        
        public LevelProgressController(IDataHolder dataHolder,
            ISatietyObservable satietyObservable,
            IScoreSource scoreSource, 
            IHealthObservable healthObservable,
            IEventBus eventBus,
            ICurrentLevelDescriptorHolder descriptorHolder,
            IGameProgressCollector gameProgressCollector
            )
        {
            var gps = dataHolder.GetGamePlaySettings();
            _victoryScoreMultipler = gps.VictoryScoreMultipler;
            _defeatScoreMultipler = gps.DefeatScoreMultipler;
            _nullSatietyScoreMultipler = gps.NullSatietyScoreMultipler;
            _satietyConditionScoreMultipler = gps.SatietySuccefScoreMultipler;
            
            _playerSatiety = satietyObservable;
            _scoreSource = scoreSource;
            _health = healthObservable;
            _eventBus = eventBus;
            _levelDescriptorHolder = descriptorHolder;
            _gameProgressCollector = gameProgressCollector;
        }
        
        
        #region ILevelProgress
        
        public void LevelStart()
        {
            _eventBus.Subscribe<LevelTimeUpSignal>(OnLevelTimeUp);
            _isTimeUp = false;
            _health.OnValueChanged += OnPlayerHealthChanged;
            _isCharacterAlive = true;
            _playerSatiety.OnValueChanged += OnPlayerSatietyChanged;
            _satietyToSuccess = _levelDescriptorHolder.GetCurrentLevel().SatietyToSucces * _playerSatiety.MaxValue;
            _isSatietyConditionMet = false;
            _isSatietyFull = false;
            _scoreСoefficient = (_satietyConditionScoreMultipler - _nullSatietyScoreMultipler) / _satietyToSuccess;
        }

        public void LevelEnd()
        {
            Debug.Log("LevelProgressController->LevelEnd: ");
            _eventBus.Unsubscribe<LevelTimeUpSignal>(OnLevelTimeUp);
            _health.OnValueChanged -= OnPlayerHealthChanged;
            _playerSatiety.OnValueChanged -= OnPlayerHealthChanged;
            RegistrateHuntResults();
        }
        
        #endregion


        #region IHuntResultsSource
        
        public IHuntResults GetHuntResults()
        {
            GenerateHuntResults();
            return _lastHuntResults;
        }
        
        #endregion
        
        private void GenerateHuntResults()
        {
            Debug.Log("LevelProgressController->GenerateHuntResults: ");
            _lastHuntResults ??= new HuntResults();
            _lastHuntResults.IsAlive = _isCharacterAlive;
            _lastHuntResults.IsSatietyCompleted = _isSatietyConditionMet;
            bool isVictory = IsVictoryCheck();
            _lastHuntResults.IsSucces = isVictory;
            _lastHuntResults.BaseScore = _scoreSource.GetScore();
            _lastHuntResults.CollectedSatiety = _playerSatiety.Value;
            _lastHuntResults.MaxSatiety = _playerSatiety.MaxValue;
            _lastHuntResults.SatietyCondition = _satietyToSuccess;
            _lastHuntResults.SatietyScoreMultipler = CalculateSatietyScoreMultipler();
            _lastHuntResults.TotalScore = CalculateTotalScore(isVictory);
            _lastHuntResults.VictoryScoreMultipler = isVictory ? _victoryScoreMultipler : _defeatScoreMultipler;
        }
        

        private void OnLevelTimeUp(LevelTimeUpSignal signal)
        {
            Debug.Log("LevelProgressController->OnLevelTimeUp: ");
            _isTimeUp = true;
        }

        private void OnPlayerHealthChanged(float health)
        {
            if (health <= 0.0f)
            {
                _isCharacterAlive = false;
            }
        }

        private void OnPlayerSatietyChanged(float satiety)
        {
            _isSatietyConditionMet = satiety >= _satietyToSuccess;
            _isSatietyFull = satiety >= _playerSatiety.MaxValue;
        }
        
        private void RegistrateHuntResults()
        {
            //Debug.Log("LevelProgressController->RegistrateHuntResults: ");
            GenerateHuntResults();
            _gameProgressCollector.RegistrateHuntResults(_lastHuntResults);
        }
        
        private float CalculateSatietyScoreMultipler()
        {
            float currentSatiety = _playerSatiety.Value;
            float satietyMultipler = _scoreСoefficient * currentSatiety + _nullSatietyScoreMultipler;
            return satietyMultipler;
        }

        private int CalculateTotalScore(bool isVictory)
        {
            int currentScore = _scoreSource.GetScore();
            float victoryMultipler = isVictory ? _victoryScoreMultipler : _defeatScoreMultipler;
            int totalScore = (int)(currentScore * victoryMultipler * CalculateSatietyScoreMultipler());
            return totalScore;
        }

        private bool IsVictoryCheck()
        {
            return _isCharacterAlive && ( 
                (_isSatietyConditionMet && _isTimeUp) || _isSatietyFull );
        }
    }
}