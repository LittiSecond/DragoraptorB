using Dragoraptor.ScriptableObjects;
using Dragoraptor.Interfaces;
using UnityEngine;


namespace Dragoraptor.MonoBehs
{
    public class DataContainerBehaviour : MonoBehaviour, IDataHolder
    {
        [SerializeField] private GamePlaySettings _gamePlaySettings;
        [SerializeField] private Campaign _mainCampaign;
        [SerializeField] private CharDamagedVisualEffectSettings _charDamagedVisualEffectSettings;


        #region IDataHolder

        public GamePlaySettings GetGamePlaySettings()
        {
            return _gamePlaySettings;
        }

        public Campaign GetCampaign()
        {
            return _mainCampaign;
        }

        public CharDamagedVisualEffectSettings GetCharDmgVisualSettings()
        {
            return _charDamagedVisualEffectSettings;
        }
        
        #endregion
        

    }
}