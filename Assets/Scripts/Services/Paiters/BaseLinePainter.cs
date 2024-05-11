using UnityEngine;


namespace Dragoraptor.Painters
{
    abstract class BaseLinePainter
    {

        protected Transform _bodyTransform;
        protected LineRenderer _lineRenderer;

        protected Vector2 _bodyPosition;
        protected Vector2 _touchPosition;


        public abstract void Execute();

        public virtual void SetData(Transform bodyTransform, LineRenderer lineRenderer)
        {
            _bodyTransform = bodyTransform;
            _lineRenderer = lineRenderer;
            _lineRenderer.enabled = false;
        }

        public void ClearData()
        {
            _bodyTransform = null;
            _lineRenderer = null;
        }

        public void SetTouchPosition(Vector2 position)
        {
            _touchPosition = position;
        }

        public void DrawingOn()
        {
            _lineRenderer.SetPosition(1, Vector3.zero);
            _lineRenderer.enabled = true;
            _bodyPosition = _bodyTransform.position;
        }

        public void DrawingOff()
        {
            _lineRenderer.enabled = false;
        }

    }
}