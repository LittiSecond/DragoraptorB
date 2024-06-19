using System;
using UnityEngine;

namespace Dragoraptor.Models
{
    [Serializable]
    public class CampaignLevelData
    {
        public string LevelPath;
        public Vector2 PositionOnMap;
        public LevelStatus StartStatus;
    }
}