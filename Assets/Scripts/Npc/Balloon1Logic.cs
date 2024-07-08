using UnityEngine;

using VContainer;

using Dragoraptor.Interfaces;
using Dragoraptor.ScriptableObjects;
using ObjPool;


namespace Dragoraptor.Npc
{
    public class Balloon1Logic : NpcBaseLogic
    {
        private const string DESTRUCTION_PREFAB_ID = "BalloonCrash";
        
        [SerializeField] private float _speed = 1.0f;
        
        private NpcMovementUsingWayPoints _movement;
        private ISceneGeometry _sceneGeometry;
        
        
        protected override void Awake()
        {
            base.Awake();
            var tempTransform = transform;
            var direction = new NpcDirectionStump();
            _movement = new NpcMovementUsingWayPoints(tempTransform, _rigidbody, direction);
            AddExecutable(_movement);
            AddActivatable(_movement);
            AddCleanable(_movement);
            _movement.OnWayFinished += OnWayFinished;
            _movement.Speed = _speed;
        }

        [Inject]
        private void Construct2(ISceneGeometry sceneGeometry)
        {
            _sceneGeometry = sceneGeometry;
        }

        private void OnWayFinished()
        {
            Rect visibleArea = _sceneGeometry.GetVisibleArea();
            Vector2 position = (Vector2)transform.position;
            if (!visibleArea.Contains(position))
            {
                DestroyItSelf();
            }
        }
        
        protected override void OnHealthEnded()
        {
            _movement.StopMovementLogic();
            CreateDestructionObject();
            base.OnHealthEnded();
        }
        
        public override void SetAdditionalData(NpcData additionalData)
        {
            NpcDataWay data = additionalData as NpcDataWay;
            if (data != null)
            {
                _movement.SetWay(data);
            }
        }

        private void CreateDestructionObject()
        {
            PooledObject obj = _pool.GetObjectOfType(DESTRUCTION_PREFAB_ID);
            if (obj != null)
            {
                obj.transform.position = transform.position;
                (obj as IActivatable)?.Activate();
            }
        }
    }
}