using System;
using UnityEngine;

using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Models.NpcSpawn
{
    [Serializable]
    public struct SpawnData
    {
        public string PrefabID;
        public float Time;
        public Vector2 SpawnPosition;
        public NpcData Data;
    }
}