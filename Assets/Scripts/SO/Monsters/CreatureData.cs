using UnityEngine;

namespace _23DaysLeft.Monsters
{
    [CreateAssetMenu(fileName = "CreatureData", menuName = "Creatures/CreatureData", order = 2)]
    public class CreatureData : ScriptableObject
    {
        public string Name;
        public float MaxHp;
        public float AttackPower;
        public float Defense;
        public float Speed;
        public float AttackDelay;
        public CombatType CombatType;
    }

    public enum CombatType
    {
        Fleeing,
        Attacking,
    }
}