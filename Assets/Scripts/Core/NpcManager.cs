using System.Collections.Generic;
using UnityEngine;

using VContainer.Unity;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Npc;
using Dragoraptor.Npc;


namespace Dragoraptor.Core
{
    public class NpcManager : INpcManager, ITickable, INpcCollector
    {


        private List<NpcBaseLogic> _npcOnField;
        private ICurrentLevelDescriptorHolder _descriptorHolder;
        private INpcSpawner _spawner;


        public NpcManager(ICurrentLevelDescriptorHolder holder, INpcSpawner npcSpawner)
        {
            _descriptorHolder = holder;
            _spawner = npcSpawner;
            _spawner.SetCollector(this);
        }
        

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

        
        #region INpcCollector

        public void AddNpc(NpcBaseLogic newNpc)
        {
            
        }

        #endregion
    }
}