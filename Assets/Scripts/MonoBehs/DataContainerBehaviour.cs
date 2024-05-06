using Dragoraptor.ScriptableObjects;
using Interfaces;
using UnityEngine;


namespace Dragoraptor.MonoBehs
{
    public class DataContainerBehaviour : MonoBehaviour, IDataHolder
    {
        [SerializeField] private GamePlaySettings _gamePlaySettings;
        [SerializeField] private Campaign _mainCampaign;


        #region IDataHolder

        public GamePlaySettings GetGamePlaySettings()
        {
            return _gamePlaySettings;
        }

        public Campaign GetCampaign()
        {
            return _mainCampaign;
        }
        
        #endregion
        

    }
}