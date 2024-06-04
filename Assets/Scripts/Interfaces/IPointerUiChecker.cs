using UnityEngine;


namespace Dragoraptor.Interfaces
{
    public interface IPointerUiChecker
    {
        bool IsPointerUnderUiElement(Vector2 screenPosition);
    }
}