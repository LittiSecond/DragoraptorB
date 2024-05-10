using UnityEngine;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.MonoBehs;


namespace Dragoraptor.Character
{
    public class JumpController : IBodyUser, ICharStateListener
    {
        
        
        #region IBodyUser
        
        public void SetBody(PlayerBody body)
        {
            Debug.Log("JumpController->SetBody:");
        }

        public void ClearBody()
        {
            Debug.Log("JumpController->ClearBody:");
        }
        
        #endregion
        
        #region ICharStateListener

        public void StateChanged(CharacterState newState)
        {
            Debug.Log("JumpController->StateChanged: newState = " + newState.ToString());
        }
        
        #endregion
    }
}