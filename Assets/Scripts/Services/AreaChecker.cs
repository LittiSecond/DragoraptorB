using UnityEngine;
using Dragoraptor.Interfaces;


namespace Dragoraptor
{
    public class AreaChecker : IAreaChecker
    {
        public ObjectType CheckPoint(Vector2 worldPosition)
        {
            ObjectType type = ObjectType.None;

            var collision = Physics2D.OverlapPoint(worldPosition);
            if (collision != null)
            {
                int layer = collision.gameObject.layer;
                switch (layer)
                {
                    case (int)SceneLayer.Ground:
                        type = ObjectType.Ground;
                        break;
                    case (int)SceneLayer.Player:
                        type = ObjectType.Player;
                        break;
                    case (int)SceneLayer.Npc:
                        type = ObjectType.Npc;
                        break;
                }
            }

            return type;
        }
    }
}