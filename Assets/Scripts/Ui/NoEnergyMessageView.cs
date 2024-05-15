using UnityEngine;
using UnityEngine.UIElements;

using Dragoraptor.Interfaces.Ui;
using TimersService;


namespace Dragoraptor.Ui
{
    public class NoEnergyMessageView : ScreenWidgetBase, INoEnergyMessageView
    {

        private const string MESSAGE_ELEMENT_NAME = "no-energy-message";

        private ITimersService _timersService;
        
        private readonly float _animationDuration = 2.0f;
        private readonly float _interval = 0.3f;

        private float _startTime;
        private int _timerId;

        private bool _isVisible;


        public NoEnergyMessageView(IUiFactory uiFactory, ITimersService timersService) : base(uiFactory)
        {
            _timersService = timersService;
        }


        protected override void Initialise()
        {
            VisualElement widgetRoot = _factory.GetHuntScreen();
            _root = widgetRoot.Q<VisualElement>(MESSAGE_ELEMENT_NAME);
        }


        #region INoEnergyMessageView, ScreenWidgetBase

        public override void Show()
        {
            base.Show();
            StartBlinking();
        }

        public override void Hide()
        {
            base.Hide();
            if (_timerId > 0)
            {
                StopBlinking();
            }
        }

        #endregion

        private void StartBlinking()
        {
            if (_timerId == 0)
            {
                _timerId = _timersService.AddRepeatedTimer(BlinkingTick, _interval);
            }
            _startTime = Time.time;
        }

        private void BlinkingTick()
        {
            if (_isVisible)
            {
                _root.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
                _isVisible = false;
            }
            else
            {
                _root.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
                _isVisible = true;
            }

            if (Time.time - _startTime >= _animationDuration)
            {
                StopBlinking();
                base.Hide();
            }
        }

        private void StopBlinking()
        {
            if (_timerId > 0)
            {
                _timersService.RemoveTimer(_timerId);
                _timerId = 0;
            }
        }
        
    }
}