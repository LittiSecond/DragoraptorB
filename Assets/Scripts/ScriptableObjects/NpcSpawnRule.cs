using Dragoraptor.Models.NpcSpawn;
using UnityEngine;

using Dragoraptor.Npc;


namespace Dragoraptor.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewRule", menuName = "Resources/NpcSpawnRule")]
    public class NpcSpawnRule : ScriptableObject
    {
        public SpawnSequence[] Sequences;
    }
}