using UnityEngine;
using UnityEngine.AI;

namespace _23DaysLeft.Monsters
{
    [RequireComponent(typeof(BossController), typeof(BossStateMachine), typeof(NavMeshAgent))]
    public class BossMonster : Creature { }
}