using UnityEngine;


namespace Dragoraptor.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCampaign", menuName = "Resources/Campaign")]
    public sealed class Campaign : ScriptableObject
    {

        public string[] LevelPaths;

    }
}