using UnityEngine;

namespace Dragoraptor.Interfaces
{
    public interface IMultiFading
    {
        public float FadingDuration { set; }
        void SetRenderers(SpriteRenderer[] renderers);
        void StartFading();
        void StopFading();
        void RestoreColors();
    }
}