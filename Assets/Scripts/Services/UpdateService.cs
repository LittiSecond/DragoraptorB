using System.Collections.Generic;

using Dragoraptor.Interfaces;
using VContainer.Unity;


namespace Dragoraptor
{
    public class UpdateService : IUpdateService, ITickable
    {

        private readonly List<IExecutable> _executables = new(); 
        
        
        #region IUpdateService
        
        public void AddToUpdate(IExecutable executable)
        {
            if (!_executables.Contains(executable))
            {
                _executables.Add(executable);
            }
        }

        public void RemoveFromUpdate(IExecutable executable)
        {
            _executables.Remove(executable);
        }
        
        #endregion


        #region ITickable
        
        public void Tick()
        {
            for (int i = 0; i < _executables.Count; i++)
            {
                _executables[i].Execute();
            }
        }
        
        #endregion
    }
}