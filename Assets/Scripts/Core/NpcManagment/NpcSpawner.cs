using System;
using System.Collections.Generic;
using UnityEngine;

using VContainer.Unity;

using TimersService;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Npc;
using Dragoraptor.Models.NpcSpawn;
using Dragoraptor.Npc;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor.Core.NpcManagment
{
    public class NpcSpawner : INpcSpawner, ITickable
    {

        private List<SequenceExecutionData> _sequenceData;
        private INpcCollector _collector;
        private readonly IObjectPool _pool;
        private readonly ICurrentLevelDescriptorHolder _descriptorHolder;
        private readonly ITimersService _timersService;

        private NpcSpawnRule _currentRule;
        
        private float _timeCounter;
        private float _delay;

        private bool _isSpawning;


        public NpcSpawner(IObjectPool pool, ICurrentLevelDescriptorHolder holder, ITimersService timersService)
        {
            _pool = pool;
            _descriptorHolder = holder;
            _timersService = timersService;
            _sequenceData = new List<SequenceExecutionData>();
        }

        #region INpcSpawner

        public void PrepareSpawn()
        {
            //Debug.Log("NpcSpawner->PrepareSpawn:");
            _isSpawning = false;
            
            var descriptor = _descriptorHolder.GetCurrentLevel();
            _currentRule = descriptor.SpawnRule;
            _sequenceData.Clear();

            if (_currentRule != null)
            {
                foreach (var sequence in _currentRule.Sequences)
                {
                    SequenceExecutionData newData = new SequenceExecutionData();
                    newData.CyclesQuantity = sequence.Quantity;
                    //newData.PrefabIds = sequence.PrefabIds;
                    newData.StartDelay = sequence.StartDelay;
                    newData.BetweenDelay = sequence.BetweenDelay;
                    newData.IsRandomized = sequence.IsRandomized;
                    if (newData.IsRandomized)
                    {
                        newData.Duration = sequence.Duration;
                    }
                    else
                    {
                        float lastSpawnTime = sequence.SpawnDatas[^1].Time; 
                        newData.Duration = Mathf.Max(lastSpawnTime, sequence.Duration);
                    }

                    newData.SpawnDatas = new SpawnData[sequence.SpawnDatas.Length];
                    sequence.SpawnDatas.CopyTo(newData.SpawnDatas, 0);

                    newData.IsFinished = true;

                    _sequenceData.Add(newData);
                }
            }
            else
            {
                Debug.LogError("NpcSpawner->PrepareSpawn: NpcSpawnRule == null");
            }
        }

        public void RestartSpawn()
        {
            //Debug.Log("NpcSpawner->RestartSpawn:");

            float currentTime = Time.time;

            foreach (var oneSequence in _sequenceData)
            {
                oneSequence.CyclesCounter = 1;
                if (oneSequence.IsRandomized)
                {
                    RandomizeSpawnData(oneSequence);
                }

                oneSequence.SequenceStartTime = currentTime + oneSequence.StartDelay;
                oneSequence.NextTime = oneSequence.SequenceStartTime + oneSequence.SpawnDatas[0].Time;
                oneSequence.CyclesCounter = 1;
                oneSequence.NextDataIndex = 0;
                oneSequence.IsFinished = false;
                //PrintDebugInfo(oneSequence);
            }

            _isSpawning = true;

        }

        public void StopSpawn()
        {
            //Debug.Log("NpcSpawner->StopSpawn:");
            _isSpawning = false;
            foreach (var oneSequence in _sequenceData)
            {
                oneSequence.IsFinished = true;
            }
        }

        public void SetCollector(INpcCollector collector)
        {
            _collector = collector;
        }

        #endregion


        #region ITickable
        
        public void Tick()
        {
            if (_isSpawning)
            {
                for (int i = 0; i < _sequenceData.Count; i++)
                {
                    ExecuteOneSequence(_sequenceData[i]);
                }
            }
        }
        
        #endregion

        private void SpawnNpc(SpawnData spawnData)
        {
            var mob = _pool.GetObjectOfType(spawnData.PrefabID);
            if (mob != null)
            {
                NpcBaseLogic newMob = mob as NpcBaseLogic;
                newMob.transform.position = (Vector3)spawnData.SpawnPosition;
                var addData = spawnData.Data;
                if (addData != null)
                {
                    newMob.SetAdditionalData(addData);
                }
                newMob.Activate();
                _collector.AddNpc(newMob);
                //Debug.Log($"NpcSpawner->SpawnNpc: newMob = {newMob.Type}; time = {Time.time}");
            }
            else
            {
                Debug.Log("NpcSpawner->SpawnNpc: mob == null");
            }
            
        }

        private void ExecuteOneSequence(SequenceExecutionData data)
        {
            if (!data.IsFinished)
            {
                if (data.NextTime <= Time.time)
                {
                    SpawnNpc(data.SpawnDatas[data.NextDataIndex]);

                    if (!SelectNextSpawnData(data))
                    {
                        SetNextSequenceCycle(data);
                    }
                    
                }
            }
            
        }

        private bool SelectNextSpawnData(SequenceExecutionData data)
        {
            bool isSelected = false;
            
            if (data.SpawnDatas.Length > data.NextDataIndex + 1)
            {
                data.NextDataIndex++;
                data.NextTime = data.SequenceStartTime + data.SpawnDatas[data.NextDataIndex].Time;
                isSelected = true;
            }
            
            return isSelected;
        }

        private void SetNextSequenceCycle(SequenceExecutionData data)
        {
            if ((data.CyclesQuantity == 0) || (data.CyclesCounter < data.CyclesQuantity))
            {
                data.CyclesCounter++;
                data.NextDataIndex = 0;
                //float endSequenceTime = Mathf.Max(data.SequenceStartTime + data.Duration, Time.time);
                data.SequenceStartTime = data.SequenceStartTime + data.Duration + data.BetweenDelay;
                if (data.IsRandomized)
                {
                    RandomizeSpawnData(data);
                }
                data.NextTime = data.SequenceStartTime + data.SpawnDatas[0].Time;
            }
            else
            {
                data.IsFinished = true;
            }
        }
        
        private void RandomizeSpawnData(SequenceExecutionData data)
        {
            RandomizeSequence(data);
            RandomizeSpawnTimes(data);
            //PrintDebugInfo(data);
        }

        private void RandomizeSequence(SequenceExecutionData data)
        {
            int quantity = data.SpawnDatas.Length;
            SpawnData[] tempData = new SpawnData[quantity];
            bool[] isMoved = new bool[quantity];
            for (int i = 0; i < quantity; i++)
            {
                int sourceIndex = UnityEngine.Random.Range(0, quantity);
                while (isMoved[sourceIndex])
                {
                    sourceIndex++;
                    if (sourceIndex >= quantity)
                    {
                        sourceIndex = 0;
                    }
                }
                tempData[i] = data.SpawnDatas[sourceIndex];
                isMoved[sourceIndex] = true;
            }

            for (int i = 0; i < quantity; i++)
            {
                data.SpawnDatas[i] = tempData[i];
            }
        }

        private void RandomizeSpawnTimes(SequenceExecutionData data)
        {
            int quantity = data.SpawnDatas.Length;
            float stepInterval = data.Duration / (quantity + 1);
            float previousTime = -1.0f;

            for (int i = 0; i < quantity; i++)
            {
                float nodalTime = stepInterval * (i + 1);
                float dt = UnityEngine.Random.Range(-stepInterval, stepInterval);
                float currentTime = nodalTime + dt;
                if ( i > 0 )
                {
                    if (currentTime < previousTime)
                    {
                        data.SpawnDatas[i - 1].Time = currentTime;
                        currentTime = previousTime;
                    }
                }
                data.SpawnDatas[i].Time = currentTime;
                previousTime = currentTime;
            }
        }

        private void PrintDebugInfo(SequenceExecutionData data)
        {
            string message = String.Empty;

            for (int i = 0; i < data.SpawnDatas.Length; i++)
            {
                SpawnData current = data.SpawnDatas[i];
                message += $"i = {i}, type = {current.PrefabID}, time = {current.Time};   ";
            }
            
            Debug.Log("NpcSpawner->PrintDebugInfo: " + message);
        }
        
    }
}