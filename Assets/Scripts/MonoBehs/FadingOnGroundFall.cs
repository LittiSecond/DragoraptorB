using System;
using UnityEngine;

using VContainer;

using Dragoraptor.Interfaces;


namespace Dragoraptor.MonoBehs
{
    public class FadingOnGroundFall : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer[] _renderersToFade;
        [SerializeField] private float _fadeDuration = 5.0f;
        
        private IUpdateService _updateService;
        private IMultiFading _fader;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == (int)SceneLayer.Ground)
            {
                StartFading();
            }
        }

        [Inject]
        private void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        private void StartFading()
        {
            if (_fader == null)
            {
                InitializeFader();
            }
            
            _fader.StartFading();
            
        }

        private void InitializeFader()
        {
            _fader = new MultiFading(_updateService);
            _fader.SetRenderers(_renderersToFade);
            _fader.FadingDuration = _fadeDuration;
        }

        private void OnDisable()
        {
            if (_fader != null)
            {
                _fader.StopFading();
                _fader.RestoreColors();
            }
        }
    }
}