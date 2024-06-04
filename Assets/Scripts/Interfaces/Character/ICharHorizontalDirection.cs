using UnityEngine;


namespace Dragoraptor.Interfaces.Character
{
    public interface ICharHorizontalDirection
    {
        Direction HorizontalDirection { get; set; }
        void TouchPrepareJump(Vector2 worldPosition);
    }
}