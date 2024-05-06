using System;
using UnityEngine;


namespace Dragoraptor.Models
{
    [Serializable]
    public sealed class AttackAreasPack
    {
        public Rect LeftIdle;
        public Rect LeftWalk;
        public Rect LeftFliesUp;
        public Rect LeftFliesDown;
        public Rect RightIdle;
        public Rect RightWalk;
        public Rect RightFliesUp;
        public Rect RightFliesDown;
    }
}