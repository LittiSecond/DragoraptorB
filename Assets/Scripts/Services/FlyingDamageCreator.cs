using UnityEngine;

using ObjPool;

using Dragoraptor.Interfaces;
using Dragoraptor.MonoBehs;


namespace Dragoraptor
{
    public class FlyingDamageCreator : IDamageObserver
    {
        private const string TYPE = "FlyingText";

        private readonly Transform _startPoint;
        private IObjectPool _objectPool;


        public FlyingDamageCreator(Transform startPoint)
        {
            _startPoint = startPoint;
        }

        public void Construct(IObjectPool pool)
        {
            _objectPool = pool;
        }

        #region IDamageObserver

        public void OnDamaged(float amount)
        {
            if (_objectPool == null)
            {
                //Debug.LogError("FlyingDamageCreator->OnDamaged: _objectPool == null");
                return;
            }
            
            PooledObject obj = _objectPool.GetObjectOfType(TYPE);
            if (obj)
            {
                FlyingText text = obj as FlyingText;
                text.transform.position = _startPoint.position;
                text.StartFlying((-amount).ToString());
            }
        }

        #endregion
    }
}