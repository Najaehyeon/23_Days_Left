using _23DaysLeft.Monsters;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BossController), typeof(BossStateMachine), typeof(NavMeshAgent))]
public class BossMonster : Creature { }