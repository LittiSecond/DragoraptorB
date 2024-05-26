using Dragoraptor.Interfaces.Npc;
using Dragoraptor.Models;
using VContainer.Unity;


namespace Dragoraptor.Core
{
    public class NpcSpawner : INpcSpawner, ITickable
    {

        private INpcCollector _collector;
        
        

        #region INpcSpawner

        public void SetSpawnRule(NpcSpawnRule rule)
        {
            
        }

        public void RestartSpawn()
        {
            
        }

        public void StopSpawn()
        {
            
        }

        public void SetCollector(INpcCollector collector)
        {
            _collector = collector;
        }

        #endregion


        #region ITickable
        
        public void Tick()
        {
            
        }
        
        #endregion
        
    }
}