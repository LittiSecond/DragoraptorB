using UnityEngine;

using VContainer.Unity;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Npc;
using Dragoraptor.Models;
using Dragoraptor.Npc;


namespace Dragoraptor.Core
{
    public class NpcSpawner : INpcSpawner, ITickable
    {

        private INpcCollector _collector;
        private IObjectPool _pool;

        private float _timeCounter;
        private float _delay;

        private bool _isTiming;


        public NpcSpawner(IObjectPool pool)
        {
            _pool = pool;
        }

        #region INpcSpawner

        public void SetSpawnRule(NpcSpawnRule rule)
        {
            Debug.Log("NpcSpawner->SetSpawnRule:");
        }

        public void RestartSpawn()
        {
            Debug.Log("NpcSpawner->RestartSpawn:");
            _timeCounter = 0.0f;
            _delay = 3.0f;
            _isTiming = true;
        }

        public void StopSpawn()
        {
            Debug.Log("NpcSpawner->StopSpawn:");
        }

        public void SetCollector(INpcCollector collector)
        {
            _collector = collector;
        }

        #endregion


        #region ITickable
        
        public void Tick()
        {
            if (_isTiming)
            {
                _timeCounter += Time.deltaTime;
                if (_timeCounter >= _delay)
                {
                    _isTiming = false;
                    TimerEnd();
                }
            }
        }
        
        #endregion

        private void SpawnNpc(string typeId, Vector2 position)
        {
            var mob = _pool.GetObjectOfType(typeId);
            if (mob != null)
            {
                NpcBaseLogic newMob = mob as NpcBaseLogic;
                newMob.transform.position = (Vector3)position;
                newMob.Activate();
                _collector.AddNpc(newMob);
            }
            else
            {
                Debug.Log("NpcSpawner->SpawnNpc: mob == null");
            }
        }

        private void TimerEnd()
        {
            SpawnNpc("Bird1", new Vector2(4.0f, 0.0f));
        }
        
    }
}