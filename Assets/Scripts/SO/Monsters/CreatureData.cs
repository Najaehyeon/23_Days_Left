using UnityEngine;

namespace _23DaysLeft.Monsters
{
    [CreateAssetMenu(fileName = "CreatureData", menuName = "Creatures/CreatureData", order = 2)]
    public class CreatureData : ScriptableObject
    {
        [Header("Creature Info")]
        public string Name;
        public CombatType CombatType;
        
        [Header("Creature Stats")]
        public float MaxHp;
        public float AttackPower;
        public float AttackDelay;
        public float Defense;
        public float Speed;
        public float detectionRange;
        
        // [Header("Creature Drops")]
        // public ItemData[] DropItems;
    }

    public enum CombatType
    {
        Fleeing,
        Attacking,
    }
}