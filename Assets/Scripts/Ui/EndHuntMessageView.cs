using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Ui;
using UnityEngine.UIElements;

namespace Dragoraptor.Ui
{
    public class EndHuntMessageView : IHuntUiInitializable
    {
        private const string MESSAGE_ELEMENT_NAME = "end-hunt-message";

        private IUiFactory _factory;
        private ISatietyObservable _satiety;
        private VisualElement _messageRoot;

        private bool _isVisible;


        public EndHuntMessageView(IUiFactory factory, ISatietyObservable satiety)
        {
            _factory = factory;
            _satiety = satiety;
        }

        #region IHuntUiInitializable
        
        public void InitializeUi()
        {
            VisualElement widgetRoot = _factory.GetHuntScreen();
            _messageRoot = widgetRoot.Q<VisualElement>(MESSAGE_ELEMENT_NAME);
            _satiety.OnValueChanged += SatietyChanged;
            Hide();
        }
        
        #endregion

        private void SatietyChanged(float value)
        {
            bool isMax = value >= _satiety.MaxValue;
            if (isMax && !_isVisible)
            {
                Show();
            }
            else if (!isMax && _isVisible)
            {
                Hide();
            }
        }

        private void Show()
        {
            _messageRoot.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
            _isVisible = true;
        }

        private void Hide()
        {
            _messageRoot.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
            _isVisible = false;
        }
        
        
        
    }
}