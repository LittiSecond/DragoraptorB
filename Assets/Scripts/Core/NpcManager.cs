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


        private readonly List<NpcBaseLogic> _npcOnField = new();
        private readonly ICurrentLevelDescriptorHolder _descriptorHolder;
        private readonly INpcSpawner _spawner;

        private bool _isNpcLogicEnabled;


        public NpcManager(ICurrentLevelDescriptorHolder holder, INpcSpawner npcSpawner)
        {
            _descriptorHolder = holder;
            _spawner = npcSpawner;
            _spawner.SetCollector(this);
            _isNpcLogicEnabled = true;
        }
        

        #region INpcManager
        
        public void PrepareSpawn()
        {
            Debug.Log("NpcManager->PrepareSpawn: ");
        }

        public void StopSpawn()
        {
            Debug.Log("NpcManager->StopSpawn: ");
            _spawner.StopSpawn();
        }

        public void RestartSpawn()
        {
            Debug.Log("NpcManager->RestartSpawn: ");
            _spawner.RestartSpawn();
        }

        public void ClearNps()
        {
            Debug.Log("NpcManager->ClearNps: ");
            for (int i = _npcOnField.Count - 1; i >= 0; i--)
            {
                _npcOnField[i].OnDestroy -= OnDestroyNpc;
                _npcOnField[i].DestroyItSelf();
            }
            _npcOnField.Clear();
        }
        
        #endregion


        #region ITickable

        public void Tick()
        {
            if (_isNpcLogicEnabled)
            {
                ExecuteNpcLogic();
            }
        }
        
        #endregion

        
        #region INpcCollector

        public void AddNpc(NpcBaseLogic newNpc)
        {
            _npcOnField.Add(newNpc);
            newNpc.OnDestroy += OnDestroyNpc;
        }

        #endregion
        
        private void ExecuteNpcLogic()
        {
            for (int i = 0; i < _npcOnField.Count; i++)
            {
                _npcOnField[i].Execute();
            }
        }
        
        private void OnDestroyNpc(NpcBaseLogic npc)
        {
            _npcOnField.Remove(npc);
            npc.OnDestroy -= OnDestroyNpc;
        }
    }
}