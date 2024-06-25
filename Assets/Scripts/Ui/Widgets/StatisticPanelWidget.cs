using System;
using System.Collections.Generic;

using UnityEngine.UIElements;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Ui;
using Dragoraptor.Models;
using UnityEngine;


namespace Dragoraptor.Ui
{
    public class StatisticPanelWidget : ScreenWidgetBase
    {
        private const string CLOSE_BUTTON_NAME = "close-button";
        private const string LEVELS_COMPLITED_VALUE_NAME = "levels-complited-value";
        private const string SCORE_VALUE_NAME = "score-value";
        private const string HUNTS_TOTAL_VALUE_NAME = "hunts-count-value";
        private const string LAST_HUNT_SCORE_VALUE_NAME = "last-hunt-score-value";
        private const string SELECT_LEVEL_DROPDOWN_NAME = "select-level-dropdown";
        private const string BEST_SCORE_VALUE_NAME = "best-score-value";
        
        
        public Action OnCloseButtonClick;

        private IProgressDataHolder _progressDataHolder;
        private Button _closeButton;
        private Label _levelsCompletedValue;
        private Label _scoreValue;
        private Label _huntsTotalValue;
        private Label _lastHuntScoreValue;
        private Label _bestScoreOfHuntValue;
        private DropdownField _selectLevelDropdown;

        private List<string> _levelNumbers;

        public StatisticPanelWidget(IUiFactory uiFactory, IProgressDataHolder progressDataHolder) : base(uiFactory)
        {
            _progressDataHolder = progressDataHolder;
        }

        protected override void Initialise()
        {
            _root = _factory.GetStatisticPanel();
            _closeButton = _root.Q<Button>(CLOSE_BUTTON_NAME);
            _closeButton.RegisterCallback<ClickEvent>(evt => OnCloseButtonClick?.Invoke());
            _levelsCompletedValue = _root.Q<Label>(LEVELS_COMPLITED_VALUE_NAME);
            _scoreValue = _root.Q<Label>(SCORE_VALUE_NAME);
            _huntsTotalValue = _root.Q<Label>(HUNTS_TOTAL_VALUE_NAME);
            _lastHuntScoreValue = _root.Q<Label>(LAST_HUNT_SCORE_VALUE_NAME);
            _bestScoreOfHuntValue = _root.Q<Label>(BEST_SCORE_VALUE_NAME);
            _selectLevelDropdown = _root.Q<DropdownField>(SELECT_LEVEL_DROPDOWN_NAME);
            _selectLevelDropdown.RegisterValueChangedCallback( evt =>  OnLevelChanged());

            _levelNumbers = new List<string>();

            //OutputTestData();
        }


        private void OutputTestData()
        {
            _levelNumbers.Add("1");
            _levelNumbers.Add("2");
            _levelNumbers.Add("3");
            
            _selectLevelDropdown.choices.Clear();
            _selectLevelDropdown.choices.AddRange(_levelNumbers);
            _selectLevelDropdown.value = _selectLevelDropdown.choices[0];
            _selectLevelDropdown.index = 0;
        }

        public override void Show()
        {
            base.Show();
            ProgressData data = _progressDataHolder.GetProgressData();
            
            _selectLevelDropdown.choices.Clear();
            
            int currentLevel = data.CurrentLevelNumber;
            //_progresses = data.Levels.ToArray();
            for (int i = 0; i < data.Levels.Count; i++)
            {
                LevelStatus levelStatus = data.Levels[i].Status;
                if (levelStatus == LevelStatus.Available || levelStatus == LevelStatus.Finished)
                {
                    int levelNumber = data.Levels[i].LevelNumber;
                    _selectLevelDropdown.choices.Add(levelNumber.ToString());
                    //_levelSelectDropdown.options.Add(new Dropdown.OptionData(levelNumber.ToString()));
                    if (levelNumber == currentLevel)
                    {
                        _selectLevelDropdown.index = i;
                        _selectLevelDropdown.value = _selectLevelDropdown.choices[i];
                    }
                }
            }
            _levelsCompletedValue.text = data.CompletedLevels.ToString();
            _scoreValue.text = data.TotalScore.ToString();
            _huntsTotalValue.text = data.HuntsTotal.ToString();
            _lastHuntScoreValue.text = data.LastScore.ToString();

            int index = currentLevel - 1;
            if (index < 0)
            {
                index = 0;
            }
            _bestScoreOfHuntValue.text = data.Levels[index].BestScore.ToString();
        }

        private void OnLevelChanged()
        {
            int index = _selectLevelDropdown.index;
            ProgressData progressData = _progressDataHolder.GetProgressData();
            if (index >= 0 && index < progressData.Levels.Count)
            {
                _bestScoreOfHuntValue.text = progressData.Levels[index].BestScore.ToString();
            }
        }
    }
}