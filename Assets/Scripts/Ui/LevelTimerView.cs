using UnityEngine.UIElements;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class LevelTimerView : ITimerView
    {

        private const string TIMER_TEXT_NAME = "timer-value";
        private const string SEPARATOR = ":";
        private const string LETTER_NULL = "0";
        private const int SECONDS_IN_MINUTE = 60;
        private const int TWO_NUMERAL_MIN_NUMBER = 10;

        private IUiFactory _factory;
        private Label _text;

        private bool _isInitialized;


        public LevelTimerView(IUiFactory factory)
        {
            _factory = factory;
        }

        
        #region ITimerView

        public void SetTime(float timeSeconds)
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            int timeInt = (int)timeSeconds;
            int minuts = timeInt / SECONDS_IN_MINUTE;
            int seconds = timeInt % SECONDS_IN_MINUTE;

            string text = minuts.ToString() + SEPARATOR;

            if (seconds < TWO_NUMERAL_MIN_NUMBER)
            {
                text += LETTER_NULL;
            }
            text += seconds.ToString();
            _text.text = text;
        }
        
        #endregion


        private void Initialize()
        {
            VisualElement screen = _factory.GetHuntScreen();
            _text = screen.Q<Label>(TIMER_TEXT_NAME);
            _isInitialized = true;
        }
        
        
        
    }
}