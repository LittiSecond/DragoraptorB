using UnityEngine;
using UnityEngine.UIElements;

using Dragoraptor.Interfaces.Character;


namespace Dragoraptor.Ui
{
    public class ResourceView
    {

        protected ProgressBar _progressBar;
        protected IObservableResource _source;

        protected bool _isInitialized;
        

        public ResourceView( IObservableResource resource)
        {
            _source = resource;
        }


        public virtual void Initialize(ProgressBar bar)
        {
            _progressBar = bar;
            _progressBar.highValue = _source.MaxValue;
            _progressBar.value = _source.Value;
            _source.OnMaxValueChanged += MaxValueChanged;
            _source.OnValueChanged += ValueChanged;
            
            _isInitialized = true;
        }

        protected void MaxValueChanged(float newValue)
        {
            if (!_isInitialized) return;
            
            _progressBar.highValue = newValue;
        }

        protected void ValueChanged(float newValue)
        {
            if (!_isInitialized) return;
            
            _progressBar.value = newValue;
        }

    }
}