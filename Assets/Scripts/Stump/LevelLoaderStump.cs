using UnityEngine;
using Dragoraptor.Interfaces;
using Dragoraptor.ScriptableObjects;


namespace Dragoraptor
{
    public class LevelLoaderStump : ICurrentLevelDescriptorHolder
    {

        private LevelDescriptor _levelDescriptor;
        
        public LevelDescriptor GetCurrentLevel()
        {
            if (_levelDescriptor == null)
            {
                _levelDescriptor = Resources.Load<LevelDescriptor>("Levels/Level1/Level1");
            }

            return _levelDescriptor;
        }
    }
}