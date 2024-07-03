using System;
using UnityEngine;


namespace Dragoraptor.Models.NpcSpawn
{
    [Serializable]
    public struct SpawnData
    {
        public string PrefabID;
        public float Time;
        public Vector2 SpawnPosition;
    }
}