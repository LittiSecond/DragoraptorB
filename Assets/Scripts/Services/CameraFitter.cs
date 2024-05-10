using UnityEngine;


namespace Dragoraptor
{
    public class CameraFitter
    {
        private const float GAME_AREA_WIDTH = 6.0f;

        public static void FitCamera()
        {
            float screenRatio = (float)Screen.height / Screen.width;
            float areaHeight = GAME_AREA_WIDTH * screenRatio;
            Camera.main.orthographicSize = areaHeight / 2;
        }
    }
}