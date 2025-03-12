using UnityEngine;
using UnityEngine.AI;

namespace _23DaysLeft.Monsters
{
    public class Creature : MonoBehaviour
    {
        [SerializeField] protected CreatureStateMachine stateMachine;
        [SerializeField] protected CreatureData creatureData;
        [SerializeField] protected NavMeshAgent navMeshAgent;

        protected float currentHp;
        
        // patrol
        // chase
        // attack
        // hit
        // die
    }

    public enum CreatureState
    {
        None,
        Idle,
        Walk,
        Run,
        Attack,
        Hit,
        Die
    }
}