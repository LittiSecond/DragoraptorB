using UnityEngine;


namespace Dragoraptor.Interfaces
{
    public interface ISceneGeometry
    {
        Vector2 ConvertScreenPositionToWorld(Vector2 screenPositionInPx);
        Rect GetVisibleArea();
    }
}