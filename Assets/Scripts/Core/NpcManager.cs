using UnityEngine;

using VContainer.Unity;

using Dragoraptor.Interfaces;


namespace Dragoraptor.Core
{
    public class NpcManager : INpcManager, ITickable
    {


        #region INpcManager
        
        public void PrepareSpawn()
        {
            Debug.Log("NpcManager->PrepareSpawn: ");
        }

        public void StopSpawn()
        {
            Debug.Log("NpcManager->StopSpawn: ");
        }

        public void RestartSpawn()
        {
            Debug.Log("NpcManager->RestartSpawn: ");
        }

        public void ClearNps()
        {
            Debug.Log("NpcManager->ClearNps: ");
        }
        
        #endregion


        #region ITickable

        public void Tick()
        {
            
        }
        
        #endregion
        
    }
}