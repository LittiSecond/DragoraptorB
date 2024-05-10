using UnityEngine;


namespace Dragoraptor.Interfaces
{
    public interface IAreaChecker
    {
        ObjectType CheckPoint(Vector2 worldPosition);
    }
}