using UnityEngine;

using Dragoraptor.Interfaces;
using Dragoraptor.MonoBehs;
using Dragoraptor.ScriptableObjects;
using Interfaces;


namespace Dragoraptor.Character
{
    public class CharacterManager : ICharacterManager
    {
        
        private const string CHARACTER_PREFAB_ID = "PlayerCharacter";
        
        private GameObject _playerGO;
        private PlayerBody _playerBody;
        private readonly IPrefabLoader _prefabLoader;
        
        private Vector2 _spawnPosition;
        private bool _haveCharacterBody;


        public CharacterManager(IPrefabLoader prefabLoader, 
            IDataHolder dataHolder
            )
        {
            _prefabLoader = prefabLoader;
            _spawnPosition = dataHolder.GetGamePlaySettings().CharacterSpawnPosition;
        }
        

        #region ICharacterManager

        public void CreateCharacter()
        {
            Debug.Log("CharacterManager->CreateCharacter: ");
            if (!_haveCharacterBody)
            {
                InstantiateCharacter();
                _haveCharacterBody = true;
            }

            _playerGO.transform.position = _spawnPosition;
            _playerGO.SetActive(true);
        }

        public void RemoveCharacter()
        {
            Debug.Log("CharacterManager->RemoveCharacter: ");
            _playerGO.SetActive(false);
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