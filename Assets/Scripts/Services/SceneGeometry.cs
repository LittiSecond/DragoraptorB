using UnityEngine;

using Dragoraptor.Interfaces;


namespace Dragoraptor
{
    public class SceneGeometry : ISceneGeometry
    {

        private Rect _worldVisibleArea;
        private float _screenToWorldRatio;
        
        
        #region ISceneGeometry

        public Vector2 ConvertScreenPositionToWorld(Vector2 screenPositionInPx)
        {
            Vector2 worldPosition;

            worldPosition.x = _worldVisibleArea.xMin + screenPositionInPx.x * _screenToWorldRatio;
            worldPosition.y = _worldVisibleArea.yMin + screenPositionInPx.y * _screenToWorldRatio;
            return worldPosition;
        }

        public Rect GetVisibleArea()
        {
            return _worldVisibleArea;
        }
        
        #endregion

        public void Initialize()
        {
            Camera camera = Camera.main;

            float worldInCameraHeight = camera.orthographicSize * 2;
            _screenToWorldRatio = worldInCameraHeight / Screen.height;
            float worldInCameraWidth = Screen.width * _screenToWorldRatio;

            Vector2 cameraPos = camera.transform.position;

            float yMin = cameraPos.y - camera.orthographicSize;
            float xMin = cameraPos.x - worldInCameraWidth / 2;

            _worldVisibleArea = new Rect(xMin, yMin, worldInCameraWidth, worldInCameraHeight);
        }
        
    }
}