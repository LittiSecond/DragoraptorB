using UnityEngine;

using Dragoraptor.Models;


namespace Dragoraptor.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewSettings", menuName = "Resources/GamePlaySettings")]
    public sealed class GamePlaySettings : ScriptableObject
    {
        public float MinJumpForce;
        public float MaxJumpForce;
        public float NoJumpPowerIndicatorLength;
        public float MaxJumpPowerIndicatorLength;
        public float WalkSpeed;
        public Vector2 CharacterSpawnPosition;
        public int MaxHealth;
        public int Armor;
        public int AttackPower;
        public AttackAreasPack AttackAreas;
        public float AttackInterval = 0.5f;
        public int MaxSatiety;
        public float Energy;
        public float EnergyRegeneration;
        public float WalkEnergyRegeneration;
        public float RegenerationDelay;
        public float AttackEnergyCost;
        public float JumpEnergyCost;
        public float VictoryScoreMultipler = 2.0f;
        public float DefeatScoreMultipler = 1.0f;
        public float NullSatietyScoreMultipler = 0.5f;
        public float SatietySuccefScoreMultipler = 1.0f;
        public float CharacterDeathDelay = 5.0f;
    }
}