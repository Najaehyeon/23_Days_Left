using System;
using System.Collections;
using _23DaysLeft.Managers;
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
        // components
        private CreatureStateMachine stateMachine;
        private CreatureData creatureData;
        private NavMeshAgent navMeshAgent;

        // state
        private WaitForSeconds idleWaitTime;
        private Transform playerTr;
        private Vector3 lastDestination;
        private float lastAttackTime;
        private float lastHitTime;

        // status
        private float currentHp;
        private bool isSpawned;
        private bool isDead = true;

        public void Init(Creature creature)
        {
            // 초기화
            stateMachine = creature.StateMachine;
            creatureData = creature.CreatureData;
            navMeshAgent = creature.NavMeshAgent;
            currentHp = creatureData.MaxHp;
            lastAttackTime = creatureData.AttackDelay;
            idleWaitTime = new WaitForSeconds(creatureData.IdleTime);
            isDead = false;

            // 이벤트 등록
            stateMachine.OnHitAnimationEnd += OnHitEnd;
            stateMachine.OnAttackAnimationEnd += OnAttackEnd;

            // 상태 코루틴
            StartCoroutine(Wandering());
        }

        private void Update()
        {
            if (isDead) return;
            
            if (lastHitTime < creatureData.HitDelay)
            {
                lastHitTime += Time.deltaTime;
            }

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
            NavMesh.SamplePosition(transform.position + randomPoint, out var hit, 5f, NavMesh.AllAreas);
            return hit.position;
        }

        private void PlayerDetected()
        {
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

            if (NavMesh.SamplePosition(fleeTarget, out var hit, 2f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
            }
        }

        private void Chasing()
        {
            Vector3 direction = (playerTr.position - transform.position).normalized;
            Vector3 desiredPos = playerTr.position - direction * creatureData.AttackDistance;

            lastAttackTime += Time.deltaTime;
            if (Vector3.Distance(playerTr.position, transform.position) > creatureData.AttackDistance + 0.2f)
            {
                if (NavMesh.SamplePosition(desiredPos, out var hit, 1f, NavMesh.AllAreas))
                {
                    navMeshAgent.SetDestination(hit.position);
                }
            }
            else
            {
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
            if (IsTargetInAttackRange())
            {
                Debug.Log("Attack");
                // player.OnHit(creatureData.AttackPower);
            }
        }

        private bool IsTargetInAttackRange()
        {
            return Vector3.Distance(playerTr.position, transform.position) <= creatureData.AttackDistance;
        }

        private void OnAttackEnd()
        {
            if (isDead) return;
            stateMachine.StateChange(CreatureState.Run);
        }

        public void OnHit(float damage)
        {
            if (isDead) return;
            lastHitTime = 0f;
            if (lastHitTime < creatureData.HitDelay) return;

            StopAllCoroutines();
            stateMachine.StateChange(CreatureState.Hit);
            currentHp = Mathf.Max(currentHp - damage, 0);
            if (currentHp <= 0)
            {
                Die();
            }
        }

        private void OnHitEnd()
        {
            if (isDead) return;
            if (playerTr)
            {
                stateMachine.StateChange(CreatureState.Run);
            }
            else
            {
                StartCoroutine(Wandering());
            }
        }

        private void Die()
        {
            isDead = true;
            navMeshAgent.enabled = false;
            stateMachine.StateChange(CreatureState.Die);
            StartCoroutine(DieCoroutine());
        }

        private IEnumerator DieCoroutine()
        {
            yield return new WaitForSeconds(1.5f);
            // 아이템 드롭
            PoolManager.Instance.Despawn(gameObject);
        }
        
        public void OnPlayerDetected(Transform player)
        {
            if (isDead || playerTr) return;
            playerTr = player;
            navMeshAgent.speed = creatureData.CombatSpeed;
            stateMachine.StateChange(CreatureState.Run);
            stateMachine.OnPlayerDetected?.Invoke();
            StopAllCoroutines();
        }

        public void OnPlayerFaraway()
        {
            if (isDead || !playerTr) return;
            playerTr = null;
            navMeshAgent.speed = creatureData.OriginSpeed;
            stateMachine.OnPlayerFaraway?.Invoke();
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