using Dragoraptor.Interfaces;
using UnityEngine;


namespace Dragoraptor.Character
{
    public class CharacterMediator : IPlayerPosition, IPlayerMediator
    {

        private Transform _playerCharTransform;

        private bool _haveCharacter;
        
        
        public Vector3? GetPlayerPosition()
        {
            Vector3? position = null;
            if (_haveCharacter)
            {
                position = _playerCharTransform.position; 
            }

            return position;
        }

        public void SetCharacterTransform(Transform charTransform)
        {
            _playerCharTransform = charTransform;
            _haveCharacter = _playerCharTransform != null;
        }
        
        
        
    }
}