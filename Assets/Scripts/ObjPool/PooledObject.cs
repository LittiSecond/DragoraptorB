using UnityEngine;


namespace ObjPool
{
    public class PooledObject : MonoBehaviour
    {

        [SerializeField] protected string _type = string.Empty;
        public ObjectPool ObjectPool { get; private set; }


        public bool IsUsed { get; set; }

        public string Type
        {
            get { return _type; }
        }


        public void SetObjectPool(ObjectPool op)
        {
            ObjectPool = op;
        }

        protected void ReturnToPool()
        {
            if (ObjectPool != null)
            {
                ObjectPool.ReturnToPool(this);
            }
        }

        public virtual void PrepareToReturnToPool()
        {

        }

    }
}