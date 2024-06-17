using UnityEngine;
using UnityEngine.UIElements;


namespace Dragoraptor.Ui
{
    public class UiMissionIndicator
    {
        
        private const string SELECT_MARKER_NAME = "select-indicator";
        private const string BACKGROUND_NAME = "marker-bg";
        private const string ROOT_STYLE_NAME = "marker-root";
        
        private const float TO_PERCENT_MULTIPLER = 100.0f;

        //TODO: remove the color setting to ... something other  
        private readonly Color _notAvialableColor = Color.grey;
        private readonly Color _avialableColor = Color.yellow;
        private readonly Color _finishedColor = Color.green;
        
        private VisualElement _root;
        private VisualElement _backGround;
        private VisualElement _selectMarker;
        private VisualElement _parent;
        
        private Vector2 _position;
        private LevelStatus _status;

        private bool _haveVisual;
        private bool _isSelected;
        
        
        public int LevelNumber { get; private set; }
        
        public Vector2 Position 
        {
            set
            {
                _position = value;
                UpdatePosition();
            } 
        }

        public LevelStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                UpdateStatus();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                UpdateSelectedStatus();
            }
        }


        public UiMissionIndicator(int number, Vector2 position, LevelStatus status)
        {
            LevelNumber = number;
            Position = position;
            Status = status;
            _isSelected = false;
            _haveVisual = false;
        }

        public void SetParent(VisualElement parent)
        {
            if (!_haveVisual)
            {
                _parent = parent;
                _root = CreateVisual();
                _haveVisual = true;
                UpdateStatus();
                UpdatePosition();
                UpdateSelectedStatus();
            }
        }

        private VisualElement CreateVisual()
        {
            //TODO: remove to the factory
            VisualTreeAsset prefab = Resources.Load<VisualTreeAsset>("LevelMarker");
            VisualElement root = prefab.Instantiate();
            root.AddToClassList(ROOT_STYLE_NAME);
            _parent.Add(root);
            _selectMarker = root.Q<VisualElement>(SELECT_MARKER_NAME);
            _backGround = root.Q<VisualElement>(BACKGROUND_NAME);

            return root;
        }

        private void UpdateStatus()
        {
            if (_haveVisual)
            {
                if (_status == LevelStatus.Hidden)
                {
                    _backGround.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
                }
                else
                {
                    _backGround.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
                }
                
                switch (_status)
                {
                    case LevelStatus.NotAvailable:
                        _backGround.style.unityBackgroundImageTintColor = new StyleColor(_notAvialableColor);
                        break;
                    case LevelStatus.Available:
                        _backGround.style.unityBackgroundImageTintColor = new StyleColor(_avialableColor);
                        break;
                    case LevelStatus.Finished:
                        _backGround.style.unityBackgroundImageTintColor = new StyleColor(_finishedColor);
                        break;
                }
            }
        }

        private void UpdatePosition()
        {
            if (_haveVisual)
            {
                float x = _position.x;
                if (x >= 0.0f && x <= 1.0f)
                {
                    _root.style.left = new StyleLength(Length.Percent(x * TO_PERCENT_MULTIPLER));
                }

                float y = _position.y;
                if (y >= 0.0f && y <= 1.0f)
                {
                    _root.style.top = new StyleLength(Length.Percent(y * TO_PERCENT_MULTIPLER));
                }

            }
        }

        private void UpdateSelectedStatus()
        {
            if (_haveVisual)
            {
                if (_isSelected)
                {
                    _selectMarker.style.visibility = new StyleEnum<Visibility>(Visibility.Visible);
                }
                else
                {
                    _selectMarker.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
                }
            }
        }
        
        
    }
}