using System;


namespace Dragoraptor.Models
{
    [Serializable]
    public struct PickableResource
    {
        public ResourceType Type;
        public int Amount;
    }
}