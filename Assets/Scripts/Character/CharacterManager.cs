using System.Collections.Generic;
using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
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
        private IInput _input;
        private IEnergyLogic _energyController;

        private Vector2 _spawnPosition;
        private bool _haveCharacterBody;


        public CharacterManager(IPrefabLoader prefabLoader, 
            IDataHolder dataHolder,
            ICharStateHolder stateHolder,
            IReadOnlyList<IBodyUser> bodyUsers,
            IInput input,
            IEnergyLogic energyLogic
            )
        {
            Debug.Log("CharacterManager->ctor:");
            _prefabLoader = prefabLoader;
            _spawnPosition = dataHolder.GetGamePlaySettings().CharacterSpawnPosition;
            _bodyUsers = bodyUsers;
            _stateHolder = stateHolder;
            _input = input;
            _energyController = energyLogic;
        }


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
            
            for (int i = 0; i < _bodyUsers.Count; i++)
            {
                _bodyUsers[i].SetBody(_playerBody);
            }
            
            _stateHolder.SetState(CharacterState.Idle);
            _energyController.On();
            _energyController.Reset();
        }

        public void RemoveCharacter()
        {
            Debug.Log("CharacterManager->RemoveCharacter: ");
            _energyController.Off();
            _playerGO.SetActive(false);
            
            for (int i = 0; i < _bodyUsers.Count; i++)
            {
                _bodyUsers[i].ClearBody();
            }
            
            _stateHolder.SetState(CharacterState.None);
        }

        public void CharacterControlOn()
        {
            Debug.Log("CharacterManager->CharacterControlOn: ");
            _input.On();
        }

        public void CharacterControlOff()
        {
            Debug.Log("CharacterManager->CharacterControlOff: ");
            _input.Off();
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