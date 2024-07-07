using System;
using UnityEngine;

namespace Dragoraptor.Models.Npc
{
    [Serializable]
    public class ShipMovementStats
    {
        [field: SerializeField] public float HorizontalSpeed { get; private set; } = 0.5f;
        [field: SerializeField] public float VerticalSpeed { get; private set; } = 0.1f;
        [field: SerializeField] public float MinY { get; private set; } = -1.0f;
        [field: SerializeField] public float MaxXInaccuracy { get; private set; } = 0.1f;
    }
}