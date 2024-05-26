using Dragoraptor.Models;

namespace Dragoraptor.Interfaces.Npc
{
    public interface INpcSpawner
    {
        void SetSpawnRule(NpcSpawnRule rule);
        void RestartSpawn();
        void StopSpawn();
        void SetCollector(INpcCollector collector);
    }
}