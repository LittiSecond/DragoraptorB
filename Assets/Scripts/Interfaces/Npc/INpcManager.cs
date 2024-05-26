namespace Dragoraptor.Interfaces.Npc
{
    public interface INpcManager
    {
        void PrepareSpawn();
        void StopSpawn();
        void RestartSpawn();
        void ClearNps();
    }
}