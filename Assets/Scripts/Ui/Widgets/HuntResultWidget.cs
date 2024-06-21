using System;
using Dragoraptor.Interfaces;
using UnityEngine.UIElements;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class HuntResultWidget : ScreenWidgetBase
    {

        private const string FINISH_BUTTON_NAME = "finish-button";
        private const string RESTART_BUTTON_NAME = "restart-button";
        private const string SURVIVED_MARKER_NAME = "survived-marker";
        private const string FED_MARKER_NAME = "fed-marker";
        private const string GAME_RESULT_TEXT_NAME = "game-result";
        private const string COLLECTED_SCORE_VALUE_NAME = "score-value";
        private const string COLLECTED_SATIETY_NAME = "collected-satiety-value";
        private const string REQUIRED_SATIETY_NAME = "required-satiety-value";
        private const string SATIETY_MULTIPLER_NAME = "satiety-multipler-value";
        private const string VICTORY_MULTIPLER_NAME = "victory-multipler";
        private const string SCORE_FULL_RESULT_NAME = "score-result-value";

        private const string YES_MARKER_STYLE_NAME = "yes-marker";
        private const string NO_MARKER_STYLE_NAME = "no-marker";
        
        private const string STRING_FORMAT = "F";

        // TODO: создать класс - генератор текста вместо этих строчек
        private const string VICTORY_TEXT_VALUE = "Победа";
        private const string DEFEAT_TEXT_VALUE = "Поражение";


        private Button _finishButton;
        private Button _restartButton;
        private VisualElement _survivedMarker;
        private VisualElement _fedMarker;
        private Label _victoryOrDefeatText;
        private Label _scoreValue;
        private Label _collectedSatietyValue;
        private Label _requiredSatietyValue;
        private Label _satietyScoreMultipler;
        private Label _victoryScoreMultipler;
        private Label _fullResultScore;

        private IHuntResultsSource _huntResultsSource;
        
        private Action _restartListener;
        private Action _finishListener;
        
        
        
        public HuntResultWidget(IUiFactory uiFactory, IHuntResultsSource source) : base(uiFactory)
        {
            _huntResultsSource = source;
        }

        protected override void Initialise()
        {
            _root = _factory.GetHuntResultWindow();
            _finishButton = _root.Q<Button>(FINISH_BUTTON_NAME);
            _finishButton.RegisterCallback<ClickEvent>(evt => _finishListener());
            _restartButton = _root.Q<Button>(RESTART_BUTTON_NAME);
            _restartButton.RegisterCallback<ClickEvent>(evt=>_restartListener());
            _survivedMarker = _root.Q<VisualElement>(SURVIVED_MARKER_NAME);
            _fedMarker = _root.Q<VisualElement>(FED_MARKER_NAME);
            _victoryOrDefeatText = _root.Q<Label>(GAME_RESULT_TEXT_NAME);
            _scoreValue = _root.Q<Label>(COLLECTED_SCORE_VALUE_NAME);
            _collectedSatietyValue = _root.Q<Label>(COLLECTED_SATIETY_NAME);
            _requiredSatietyValue = _root.Q<Label>(REQUIRED_SATIETY_NAME);
            _satietyScoreMultipler = _root.Q<Label>(SATIETY_MULTIPLER_NAME);
            _victoryScoreMultipler = _root.Q<Label>(VICTORY_MULTIPLER_NAME);
            _fullResultScore = _root.Q<Label>(SCORE_FULL_RESULT_NAME);
        }
        
        public void AddListeners(Action finishListener, Action restartListener)
        {
            _finishListener = finishListener;
            _restartListener = restartListener;
        }

        public override void Show()
        {
            base.Show();
            CreateTextHuntResults();
        }

        private void CreateTextHuntResults()
        {
            IHuntResults huntResults = _huntResultsSource.GetHuntResults();
            CreateCheckBoxValue(_survivedMarker, huntResults.IsAlive);
            CreateCheckBoxValue(_fedMarker, huntResults.IsSatietyCompleted);

            _victoryOrDefeatText.text = (huntResults.IsSucces) ? VICTORY_TEXT_VALUE : DEFEAT_TEXT_VALUE;
            _scoreValue.text = huntResults.BaseScore.ToString();
            _collectedSatietyValue.text =
                huntResults.CollectedSatiety.ToString() + "/" + huntResults.MaxSatiety.ToString();
            _requiredSatietyValue.text = huntResults.SatietyCondition.ToString();
            _satietyScoreMultipler.text = "x" + huntResults.SatietyScoreMultipler.ToString(STRING_FORMAT);
            _victoryScoreMultipler.text = "x" + huntResults.VictoryScoreMultipler.ToString();
            _fullResultScore.text = huntResults.TotalScore.ToString();
        }

        private void CreateCheckBoxValue(VisualElement checkBox, bool valueType)
        {
            if (valueType)
            {
                checkBox.RemoveFromClassList(NO_MARKER_STYLE_NAME);
                checkBox.AddToClassList(YES_MARKER_STYLE_NAME);
            }
            else
            {
                checkBox.RemoveFromClassList(YES_MARKER_STYLE_NAME);
                checkBox.AddToClassList(NO_MARKER_STYLE_NAME);
            }
        }
        
        
    }
}