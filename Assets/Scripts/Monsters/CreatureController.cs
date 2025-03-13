using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _23DaysLeft.Monsters
{
    public interface IDetectable
    {
        void OnPlayerDetected(Transform playerTr);
        void OnPlayerFaraway();
    }

    public class CreatureController : MonoBehaviour, IDetectable
    {
        protected CreatureStateMachine stateMachine;
        protected CreatureData creatureData;
        protected NavMeshAgent navMeshAgent;
        
        // state
        private WaitForSeconds idleWaitTime;
        protected Transform playerTr;
        private Vector3 lastDestination;
        private float lastAttackTime;
        
        // status
        protected float currentHp;
        protected bool isDead;

        public void Init(Creature creature)
        {
            stateMachine = creature.StateMachine;
            creatureData = creature.CreatureData;
            navMeshAgent = creature.NavMeshAgent;
            
            currentHp = creatureData.MaxHp;
            lastAttackTime = creatureData.AttackDelay;
            idleWaitTime = new WaitForSeconds(creatureData.IdleTime);
            StartCoroutine(Wandering());
        }

        private void Update()
        {
            if (!playerTr) return;
            PlayerDetected();
        }
        
        private IEnumerator Idle()
        {
            stateMachine.StateChange(CreatureState.Idle);
            yield return idleWaitTime;
            StartCoroutine(Wandering());
        }

        private IEnumerator Wandering()
        {
            stateMachine.StateChange(CreatureState.Walk);
            navMeshAgent.SetDestination(GetWanderPoint());

            var time = 0f;
            while (creatureData.WanderTime > time)
            {
                time += Time.deltaTime;

                // 아직 경로 계산 중이면 기다림
                if (!navMeshAgent.pathPending)
                {
                    // 경로가 없거나 경로가 끝나면 새로운 목적지 설정
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            navMeshAgent.SetDestination(GetWanderPoint());
                        }
                    }
                }

                yield return null;
            }

            StartCoroutine(Idle());
        }

        private Vector3 GetWanderPoint()
        {
            var minWanderDistance = creatureData.MinWanderDistance;
            var maxWanderDistance = creatureData.MaxWanderDistance;

            var randomPoint = Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance);
            NavMesh.SamplePosition(transform.position + randomPoint, out var hit, maxWanderDistance, NavMesh.AllAreas);
            return hit.position;
        }

        private void PlayerDetected()
        {
            navMeshAgent.speed = creatureData.CombatSpeed;
            navMeshAgent.isStopped = false;
            stateMachine.StateChange(CreatureState.Run);
            
            switch (creatureData.CombatType)
            {
                case CombatType.Fleeing:
                    Fleeing();
                    break;
                case CombatType.Attacking:
                    Chasing();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void Fleeing()
        {
            Vector3 fleeDir = (transform.position - playerTr.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDir * creatureData.FleeDistance;

            if (NavMesh.SamplePosition(fleeTarget, out var hit, creatureData.FleeDistance, NavMesh.AllAreas))
            {
                if ((lastDestination - hit.position).sqrMagnitude < 0.2f) return;
                lastDestination = hit.position;
                navMeshAgent.SetDestination(hit.position);
            }
        }
        
        private void Chasing()
        {
            if ((transform.position - playerTr.position).sqrMagnitude > creatureData.AttackDistance)
            {
                if ((lastDestination - playerTr.position).sqrMagnitude < 0.2f) return;
                lastDestination = playerTr.position;
                navMeshAgent.SetDestination(playerTr.position);
            }
            else
            {
                lastAttackTime += Time.deltaTime;
                if (lastAttackTime >= creatureData.AttackDelay && IsPlayerInFieldOfView())
                {
                    Attack();
                    lastAttackTime = 0f;
                }
            }
        }
        
        private bool IsPlayerInFieldOfView()
        {
            Vector3 directionToPlayer = playerTr.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            return angle < creatureData.FieldOfView * 0.5f;
        }

        private void Attack()
        {
            stateMachine.StateChange(CreatureState.Attack);
            // player.OnHit(creatureData.AttackPower);
        }
        
        private void Hit()
        {
            
        }

        private void Die()
        {
            isDead = true;
        }

        public void OnPlayerDetected(Transform player)
        {
            if (isDead || playerTr) return;
            playerTr = player;
            navMeshAgent.speed = creatureData.CombatSpeed;
            StopAllCoroutines();
        }

        public void OnPlayerFaraway()
        {
            if (isDead || !playerTr) return;
            playerTr = null;
            navMeshAgent.speed = creatureData.OriginSpeed;
            StopAllCoroutines();
            StartCoroutine(Wandering());
        }
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