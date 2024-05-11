using UnityEngine;


namespace Dragoraptor.Interfaces
{
    public interface IJumpCalculator
    {
        Vector2 CalculateJumpImpulse(Vector2 jumpDirection);
        float CalculateJumpCost();
    }
}