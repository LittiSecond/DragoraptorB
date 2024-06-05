using UnityEngine;
using ObjPool;
using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.Models;
using VContainer;


namespace Dragoraptor.MonoBehs
{
    public class PickUpItem : PooledObject, IActivatable
    {
        
        [SerializeField] private float _activationDelay = 0.0f;


        private IItemCollector _itemCollector;
        private PickableResource _content;
        private float _startTime;

        [Inject]
        private void Construct(IItemCollector collector)
        {
            _itemCollector = collector;
        }
        
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Time.time - _startTime >= _activationDelay)
            {
                if (collision.gameObject.layer == (int)SceneLayer.Player)
                {
                    if (_itemCollector.PickUp(_content))
                    {
                        ReturnToPool();
                    }
                }
            }
        }
        
        #region IActivatable

        public virtual void Activate()
        {
            _startTime = Time.time;
        }
        
        #endregion
        
        public void SetContent(PickableResource newContent)
        {
            _content = newContent;
        }


    }
}