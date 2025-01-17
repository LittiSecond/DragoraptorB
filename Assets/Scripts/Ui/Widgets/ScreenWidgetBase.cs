﻿
using UnityEngine.UIElements;

using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public abstract class ScreenWidgetBase : IScreenWidget
    {
        
        protected IUiFactory _factory;
        protected VisualElement _root;

        private bool _isInitialised;

        
        public ScreenWidgetBase(IUiFactory uiFactory)
        {
            _factory = uiFactory;
        }


        #region IScreenWidget

        public virtual void Show()
        {
            if (!_isInitialised)
            {
                Initialise();
                _isInitialised = true;
            }
            _root.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        public virtual void Hide()
        {
            _root.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        #endregion

        protected abstract void Initialise();

    }
}