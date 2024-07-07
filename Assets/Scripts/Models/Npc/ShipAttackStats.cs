using System;
using UnityEngine;

namespace Dragoraptor.Models.Npc
{
    [Serializable]
    public class ShipAttackStats
    {
        [field: SerializeField] public float Power { get; private set; }
        [field: SerializeField] public float ReloadTime { get; private set; }
        [field: SerializeField] public float MaxXInaccuracy { get; private set; }
    }
}