using UnityEngine;

using ObjPool;

using Dragoraptor.Interfaces;
using VContainer;


namespace Dragoraptor.Npc
{
    public class ShipType3Crash : PooledObject, IExecutable, IActivatable
    {

        private const float FULL_TURN = 360.0f;
        private const float BALLON_DEACTIVATE_OFFSET = 1.0f;

        [SerializeField] private Transform[] _toSaveState;
        [SerializeField] private Transform _bow;
        [SerializeField] private Transform _stern;
        [SerializeField] private Transform _ballon;
        [SerializeField] private HingeJoint2D _bowJoint;
        [SerializeField] private HingeJoint2D _sternJoint;
        [SerializeField] private Rigidbody2D _bowRigedbody;
        [SerializeField] private Rigidbody2D _sternRigedbody;
        [SerializeField] private float _selfDestroyDelay = 8.0f;

        private IUpdateService _updateService;
        private ISceneGeometry _sceneGeometry;
        private Vector3[] _positions;
        private Quaternion[] _rotations;

        private float _breakAngle = 45.0f;
        private float _balloonDeactivateYPosition;
        private float _destroyTimeCounter;

        private bool _isBowConnected;
        private bool _isSternConnected;
        private bool _isBalloonEnabled;


        private void Start()
        {
            _positions = new Vector3[_toSaveState.Length];
            _rotations = new Quaternion[_toSaveState.Length];

            for (int i = 0; i < _toSaveState.Length; i++)
            {
                _positions[i] = _toSaveState[i].localPosition;
                _rotations[i] = _toSaveState[i].localRotation;
            }
            _isBowConnected = true;
            _isSternConnected = true;
            _isBalloonEnabled = true;

            _balloonDeactivateYPosition = _sceneGeometry.GetVisibleArea().yMax + BALLON_DEACTIVATE_OFFSET;
        }

        [Inject]
        private void Construct(IUpdateService updateService, ISceneGeometry sceneGeometry)
        {
            _updateService = updateService;
            _sceneGeometry = sceneGeometry;
        }

        #region IExecutable
        
        public void Execute()
        {
            if (_isBowConnected)
            {
                float zAngle = _bow.transform.localRotation.eulerAngles.z;
                if (zAngle > _breakAngle && zAngle < FULL_TURN - _breakAngle)
                {
                    _bowJoint.enabled = false;
                    _isBowConnected = false;
                    Vector2 velosity = _bowRigedbody.velocity;
                    velosity.x = 0.0f;
                    _bowRigedbody.velocity = velosity;
                }
            }

            if (_isSternConnected)
            {
                float zAngle = _stern.localRotation.eulerAngles.z;
                if (zAngle > _breakAngle && zAngle < FULL_TURN - _breakAngle)
                {
                    _sternJoint.enabled = false;
                    _isSternConnected = false;
                    Vector2 velosity = _sternRigedbody.velocity;
                    velosity.x = 0.0f;
                    _sternRigedbody.velocity = velosity;
                }
            }

            if (_isBalloonEnabled)
            {
                if (_ballon.position.y > _balloonDeactivateYPosition)
                {
                    _ballon.gameObject.SetActive(false);
                    _isBalloonEnabled = false;
                }
            }

            _destroyTimeCounter += Time.deltaTime;
            if (_destroyTimeCounter >= _selfDestroyDelay)
            {
                DestroyItself();
            }
        }
        
        #endregion


        #region IActivatable

        public void Activate()
        {
            _destroyTimeCounter = 0.0f;
            _updateService.AddToUpdate(this);
        }
        
        #endregion
        
        private void RestoreStartingState()
        {
            for (int i = 0; i < _toSaveState.Length; i++)
            {
                _toSaveState[i].localPosition = _positions[i];
                _toSaveState[i].localRotation = _rotations[i];
            }
            _bowJoint.enabled = true;
            _sternJoint.enabled = true;
            _isBowConnected = true;
            _isSternConnected = true;
            if (!_isBalloonEnabled)
            {
                _ballon.gameObject.SetActive(true);
                _isBalloonEnabled = true;
            }
        }

        public override void PrepareToReturnToPool()
        {
            base.PrepareToReturnToPool();
            RestoreStartingState();
            _updateService.RemoveFromUpdate(this);
        }

        private void DestroyItself()
        {
            PrepareToReturnToPool();
            ReturnToPool();
        }
    }
}