using System.Collections.Generic;
using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.MonoBehs;
using Dragoraptor.ScriptableObjects;
using VContainer;


namespace Dragoraptor.Character
{
    public class CharacterManager : ICharacterManager
    {
        
        private const string CHARACTER_PREFAB_ID = "PlayerCharacter";
        
        private GameObject _playerGO;
        private PlayerBody _playerBody;
        private readonly IPrefabLoader _prefabLoader;
        private IReadOnlyList<IBodyUser> _bodyUsers;
        private ICharStateHolder _stateHolder;

        private Vector2 _spawnPosition;
        private bool _haveCharacterBody;


        public CharacterManager(IPrefabLoader prefabLoader, 
            IDataHolder dataHolder,
            ICharStateHolder stateHolder,
            IReadOnlyList<IBodyUser> bodyUsers
            )
        {
            Debug.Log("CharacterManager->ctor:");
            _prefabLoader = prefabLoader;
            _spawnPosition = dataHolder.GetGamePlaySettings().CharacterSpawnPosition;
            _bodyUsers = bodyUsers;
            _stateHolder = stateHolder;
        }
        
        // [Inject]
        // public void Construct(ICharStateHolder stateHolder)
        // {
        //     _stateHolder = stateHolder;
        // }


        #region ICharacterManager

        public void CreateCharacter()
        {
            Debug.Log("CharacterManager->CreateCharacter:");
            if (!_haveCharacterBody)
            {
                InstantiateCharacter();
                _haveCharacterBody = true;
            }

            _playerGO.transform.position = _spawnPosition;
            _playerGO.SetActive(true);
            
            
            foreach (var user in _bodyUsers)
            {
                user.SetBody(_playerBody);
            }
        }

        public void RemoveCharacter()
        {
            Debug.Log("CharacterManager->RemoveCharacter: ");
            _playerGO.SetActive(false);
            
            foreach (var user in _bodyUsers)
            {
                user.ClearBody();
            }
            
            _stateHolder.SetState(CharacterState.None);
        }

        public void CharacterControlOn()
        {
            Debug.Log("CharacterManager->CharacterControlOn: ");
        }

        public void CharacterControlOff()
        {
            Debug.Log("CharacterManager->CharacterControlOff: ");
        }
        
        
        #endregion
        
        
        private void InstantiateCharacter()
        {
            GameObject prefab = _prefabLoader.GetPrefab(CHARACTER_PREFAB_ID);
            _playerGO = GameObject.Instantiate(prefab);
            _playerBody = _playerGO.GetComponent<PlayerBody>();
        }
        
    }
}