using Dragoraptor.Models;

namespace Dragoraptor.Interfaces.Npc
{
    public interface INpcSpawner
    {
        void PrepareSpawn();
        void RestartSpawn();
        void StopSpawn();
        void SetCollector(INpcCollector collector);
    }
}