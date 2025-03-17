using UnityEngine;
using UnityEngine.AI;

namespace _23DaysLeft.Monsters
{
    [RequireComponent(typeof(CreatureController), typeof(CreatureStateMachine), typeof(NavMeshAgent))]
    public class Creature : MonoBehaviour
    {
        [SerializeField] private CreatureData creatureData;
        [SerializeField] private CreatureStateMachine stateMachine;
        [SerializeField] private CreatureController controller;
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent navMeshAgent;

        public CreatureData CreatureData => creatureData;
        public CreatureStateMachine StateMachine => stateMachine;
        public CreatureController Controller => controller;
        public Animator Animator => animator;
        public NavMeshAgent NavMeshAgent => navMeshAgent;

        public void Init(Vector3 spawnPos)
        {
            transform.position = spawnPos;
            navMeshAgent.enabled = true;
            stateMachine.Init(this);
            controller.Init(this);
        }
    }
}