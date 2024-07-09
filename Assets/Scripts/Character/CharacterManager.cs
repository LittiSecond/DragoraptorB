using System;
using System.Collections.Generic;
using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;


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
        private IPlayerHealth _health;
        private ISatietyController _satiety;
        private IPlayerMediator _playerMediator;
        private ICurrentLevelDescriptorHolder _levelDescriptorHolder;

        private Vector2 _spawnPosition;
        private float _charDeathDelay;
        private int _timerId;
        private bool _haveCharacterBody;
        private bool _isTiming;

        public CharacterManager(IPrefabLoader prefabLoader, 
            IDataHolder dataHolder,
            ICharStateHolder stateHolder,
            IReadOnlyList<IBodyUser> bodyUsers,
            IInput input,
            IEnergyLogic energyLogic,
            IPlayerHealth health,
            ISatietyController satiety,
            IPlayerMediator mediator,
            ICurrentLevelDescriptorHolder currentLevelDescriptorHolder
            )
        {
            //Debug.Log("CharacterManager->ctor:");
            _prefabLoader = prefabLoader;
            _spawnPosition = dataHolder.GetGamePlaySettings().CharacterSpawnPosition;
            _bodyUsers = bodyUsers;
            _stateHolder = stateHolder;
            _input = input;
            _energyController = energyLogic;
            _health = health;
            _health.OnHealthEnd += OnHealthEnd;
            _satiety = satiety;
            _playerMediator = mediator;
            _levelDescriptorHolder = currentLevelDescriptorHolder;
        }


        #region ICharacterManager

        public event Action OnCharacterKilled;
        
        public void CreateCharacter()
        {
            //Debug.Log("CharacterManager->CreateCharacter:");
            if (!_haveCharacterBody)
            {
                InstantiateCharacter();
                _haveCharacterBody = true;
            }

            _playerGO.transform.position = _spawnPosition;
            _playerMediator.SetCharacterTransform(_playerGO.transform);
            _playerGO.SetActive(true);
            
            for (int i = 0; i < _bodyUsers.Count; i++)
            {
                _bodyUsers[i].SetBody(_playerBody);
            }
            
            _health.ResetHealth();
            _stateHolder.SetState(CharacterState.Idle);
            _energyController.On();
            _energyController.Reset();
            _satiety.SetVictorySatiety(_levelDescriptorHolder.GetCurrentLevel().SatietyToSucces);
            _satiety.ResetSatiety();
        }

        public void RemoveCharacter()
        {
            //Debug.Log("CharacterManager->RemoveCharacter: ");
            _energyController.Off();
            _playerGO.SetActive(false);
            
            for (int i = 0; i < _bodyUsers.Count; i++)
            {
                _bodyUsers[i].ClearBody();
            }
            _playerMediator.SetCharacterTransform(null);
            _stateHolder.SetState(CharacterState.None);
        }

        public void CharacterControlOn()
        {
            //Debug.Log("CharacterManager->CharacterControlOn: ");
            _input.On();
        }

        public void CharacterControlOff()
        {
            //Debug.Log("CharacterManager->CharacterControlOff: ");
            _input.Off();
        }

        #endregion
        
        
        private void InstantiateCharacter()
        {
            GameObject prefab = _prefabLoader.GetPrefab(CHARACTER_PREFAB_ID);
            _playerGO = GameObject.Instantiate(prefab);
            _playerBody = _playerGO.GetComponent<PlayerBody>();
        }
        
        private void OnHealthEnd()
        {
            CharacterControlOff();
            _stateHolder.SetState(CharacterState.Death);
            _playerMediator.SetCharacterTransform(null);
            OnCharacterKilled?.Invoke();
        }
    }
}