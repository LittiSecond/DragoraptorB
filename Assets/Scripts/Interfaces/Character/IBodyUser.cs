using Dragoraptor.MonoBehs;


namespace Dragoraptor.Interfaces.Character
{
    public interface IBodyUser
    {
        void SetBody(PlayerBody body);
        void ClearBody();
    }
}