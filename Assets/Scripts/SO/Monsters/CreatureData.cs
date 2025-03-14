using UnityEngine;

namespace _23DaysLeft.Monsters
{
    [CreateAssetMenu(fileName = "CreatureData", menuName = "Creatures/CreatureData", order = 2)]
    public class CreatureData : ScriptableObject
    {
        [Header("Creature Info")]
        public string Name;
        public float MaxHp;
        public float AttackPower;
        public float AttackDelay;
        public float HitDelay;
        public float Defense;
        public float OriginSpeed;
        public float CombatSpeed;
        public CombatType CombatType;

        [Header("Creature AI")]
        public float MinWanderDistance = 10f;
        public float MaxWanderDistance = 15f;
        public float WanderTime = 10f;
        public float IdleTime = 5f;
        public float FleeDistance = 10f;
        public float FieldOfView = 120f;
        public float AttackDistance = 1f;
        
        // [Header("Creature Drops")]
        // public ItemData[] DropItems;
    }

    public enum CombatType
    {
        Fleeing,
        Attacking,
    }
}