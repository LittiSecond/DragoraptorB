namespace Dragoraptor.Interfaces
{
    public interface INpcManager
    {
        void PrepareSpawn();
        void StopSpawn();
        void RestartSpawn();
        void ClearNps();
    }
}