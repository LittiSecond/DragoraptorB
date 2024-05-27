using System;

namespace Dragoraptor.Interfaces.Character
{
    public interface ICharacterManager
    {
        void CreateCharacter();
        void RemoveCharacter();
        void CharacterControlOn();
        void CharacterControlOff();
        event Action OnCharacterKilled;
    }
}