using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using TimersService;
using Dragoraptor.Interfaces;
using Dragoraptor.Npc;


namespace Dragoraptor.DebugTools
{
    public class DebugUpdater : MonoBehaviour, IUpdateService
    {

        [SerializeField] private Balloon1Crash _balloon1;

        private List<IExecutable> _executables = new List<IExecutable>();
        private ITimersService _timersService;
        private ITickable _timersServiceBeh;


        private void Awake()
        {
            TimersServiceBehaviour tsb = new TimersServiceBehaviour();
            _timersService = tsb;
            _timersServiceBeh = tsb;
        }

        private void Start()
        {
            if (_balloon1)
            {
                _balloon1.Construct(this, _timersService);
            }
        }


        private void Update()
        {
            for (int i = 0; i < _executables.Count; i++)
            {
                _executables[i].Execute();
            }
            
            _timersServiceBeh.Tick();
        }
        
        

        #region IUpdateService

        public void AddToUpdate(IExecutable executable)
        {
            _executables.Add(executable);
        }

        public void RemoveFromUpdate(IExecutable executable)
        {
            _executables.Remove(executable);
        }
        
        #endregion
        
        
        
    }
}