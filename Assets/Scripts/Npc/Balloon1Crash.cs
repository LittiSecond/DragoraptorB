using System;
using UnityEngine;
using VContainer;
using ObjPool;
using Dragoraptor.Interfaces;
using TimersService;


namespace Dragoraptor.Npc
{
    public class Balloon1Crash : PooledObject, IActivatable
    {

        [SerializeField] private Transform[] _toSaveState;
        [SerializeField] private Rigidbody2D _top;
        [SerializeField] private Vector2 _topImpulse;
        [SerializeField] private float _topRotationInpulse;
        [SerializeField] private Rigidbody2D _left;
        [SerializeField] private Vector2 _leftImpulse;
        [SerializeField] private Rigidbody2D _right;
        [SerializeField] private Vector2 _rigthImpulse;
        [SerializeField] private Rigidbody2D _bottom;
        [SerializeField] private Rigidbody2D _basket;

        [SerializeField] private float _startFadingDelay = 3.0f;
        [SerializeField] private float _fadingDuration = 5.0f;
        [SerializeField] private float _destroyDelayFromActivate = 8.1f;

        private SpriteRenderer[] _renderersToFade;
        private IUpdateService _updateService;
        private ITimersService _timersService;
        private IMultiFading _multiFading;
        private Vector3[] _positions;
        private Quaternion[] _rotations;

        private int _startFadingTimerID;
        private int _selfDestroyTimerID;

        private void Awake()
        {
            _positions = new Vector3[_toSaveState.Length];
            _rotations = new Quaternion[_toSaveState.Length];

            for (int i = 0; i < _toSaveState.Length; i++)
            {
                _positions[i] = _toSaveState[i].localPosition;
                _rotations[i] = _toSaveState[i].localRotation;
            }

            _renderersToFade = GetComponentsInChildren<SpriteRenderer>();
        }

        [Inject]
        public void Construct(IUpdateService updateService, ITimersService timersService)
        {
            _updateService = updateService;
            _timersService = timersService;
        }

        // private void OnDisable()
        // {
        //     PrepareToReturnToPool();
        // }
        
        public override void PrepareToReturnToPool()
        {
            base.PrepareToReturnToPool();
            if (_startFadingTimerID > 0)
            {
                _timersService.RemoveTimer(_startFadingTimerID);
                _startFadingTimerID = 0;
            }

            if (_selfDestroyTimerID > 0)
            {
                _timersService.RemoveTimer(_startFadingTimerID);
                _selfDestroyTimerID = 0;
            }
            RestoreState();
            if (_multiFading != null)
            {
                _multiFading.StopFading();
                _multiFading.RestoreColors();
            }
        }


        #region IActivatable
        
        public void Activate()
        {
            _top.simulated = true;
            _top.AddForce(_topImpulse, ForceMode2D.Impulse);
            _top.angularVelocity = _topRotationInpulse;
            _left.simulated = true;
            _left.AddForce(_leftImpulse, ForceMode2D.Impulse);
            _right.simulated = true;
            _right.AddForce(_rigthImpulse, ForceMode2D.Impulse);
            _bottom.simulated = true;
            _basket.simulated = true;

            if (_startFadingTimerID == 0)
            {
                _startFadingTimerID = _timersService.AddTimer(StartFading, _startFadingDelay);
            }

            if (_selfDestroyTimerID == 0)
            {
                _selfDestroyTimerID = _timersService.AddTimer(OnDestroyTimer, _destroyDelayFromActivate);
            }
        }
        
        #endregion


        private void RestoreState()
        {
            _top.velocity = Vector2.zero;
            _top.simulated = false;
            _left.velocity = Vector2.zero;
            _left.simulated = false;
            _right.velocity = Vector2.zero;
            _right.simulated = false;
            _bottom.velocity = Vector2.zero;
            _bottom.simulated = false;
            _basket.velocity = Vector2.zero;
            _basket.simulated = false;
            
            for (int i = 0; i < _toSaveState.Length; i++)
            {
                _toSaveState[i].localPosition = _positions[i];
                _toSaveState[i].localRotation = _rotations[i];
            }
            
        }

        private void StartFading()
        {
            if (_multiFading == null)
            {
                InitializeFader();
            }
            
            _multiFading.StartFading();
            _startFadingTimerID = 0;
        }


        private void InitializeFader()
        {
            MultiFading fader = new MultiFading(_updateService);
            fader.FadingDuration = _fadingDuration;
            fader.SetRenderers(_renderersToFade);
            _multiFading = fader;
        }

        private void OnDestroyTimer()
        {
            _selfDestroyTimerID = 0;
            PrepareToReturnToPool();
            ReturnToPool();
        }
    }
}