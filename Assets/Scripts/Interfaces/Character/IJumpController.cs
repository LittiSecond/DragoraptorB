using UnityEngine;


namespace Dragoraptor.Interfaces.Character
{
    public interface IJumpController
    {
        void TouchBegin();
        void TouchEnd(Vector2 worldPosition);
    }
}