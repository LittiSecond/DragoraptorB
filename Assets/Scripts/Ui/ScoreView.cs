using UnityEngine.UIElements;

using Dragoraptor.Interfaces.Score;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class ScoreView
    {

        private string SCORE_TEXT_NAME = "score-value";
        
        private Label _scoreText;
        private readonly IScoreSource _scoreSource;
        private readonly IUiFactory _factory;


        public ScoreView(IScoreSource scoreSource, IUiFactory factory)
        {
            _scoreSource = scoreSource;
            _factory = factory;
        }
        
        public void Initialize()
        {
            VisualElement root = _factory.GetHuntScreen();
            _scoreText = root.Q<Label>(SCORE_TEXT_NAME);

            _scoreSource.OnScoreChanged += newValue => _scoreText.text = newValue.ToString();
        }

    }
}