namespace Dragoraptor.Interfaces.Character
{
    public interface ICharStateHolder
    {
        CharacterState State { get; }
        void SetState(CharacterState newState);
    }
}