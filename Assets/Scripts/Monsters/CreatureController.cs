using System;
using System.Collections;
using _23DaysLeft.Managers;
using _23DaysLeft.Utils;
using Unity.VisualScripting;
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
        protected CreatureStateMachine stateMachine;
        protected CreatureData creatureData;
        protected NavMeshAgent navMeshAgent;
        protected CreatureSpawner creatureSpawner;

        // state
        protected WaitForSeconds idleWaitTime;
        protected Transform playerTr;
        protected float lastAttackTime;
        private Vector3 lastDestination;
        private float lastHitTime;

        // status
        protected float currentHp;
        protected bool isDead = true;

        public virtual void Init(Creature creature)
        {
            // 초기화
            stateMachine = creature.StateMachine;
            creatureData = creature.CreatureData;
            navMeshAgent = creature.NavMeshAgent;
            currentHp = creatureData.MaxHp;
            lastAttackTime = creatureData.AttackDelay;
            lastHitTime = creatureData.HitDelay;
            idleWaitTime = new WaitForSeconds(creatureData.IdleTime);
            isDead = false;

            // 이벤트 등록
            stateMachine.OnHitAnimationEnd += OnHitEnd;
            stateMachine.OnAttackAnimationEnd += OnAttackEnd;
            CharacterManager.Instance.Player.controller.OnPlayerDead += OnPlayerFaraway;

            // 상태 코루틴
            StartCoroutine(Wandering());
        }

        private void Update()
        {
            if (isDead) return;

            if (lastHitTime <= creatureData.HitDelay) lastHitTime += Time.deltaTime;
            if (lastAttackTime <= creatureData.AttackDelay) lastAttackTime += Time.deltaTime;

            if (!playerTr) return;
            PlayerDetected();
        }

        protected virtual IEnumerator Idle()
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
            NavMesh.SamplePosition(transform.position + randomPoint, out var hit, 10f, NavMesh.AllAreas);
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

            if (!navMeshAgent.enabled) return;
            if (NavMesh.SamplePosition(fleeTarget, out var hit, 2f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
            }
        }

        protected virtual void Chasing()
        {
            Vector3 direction = (playerTr.position - transform.position).normalized;
            Vector3 desiredPos = playerTr.position - direction * creatureData.AttackDistance;

            if (Vector3.Distance(playerTr.position, transform.position) > creatureData.AttackDistance + 0.3f)
            {
                if (!navMeshAgent.enabled) return;
                if (NavMesh.SamplePosition(desiredPos, out var hit, 1f, NavMesh.AllAreas))
                {
                    navMeshAgent.SetDestination(hit.position);
                }
            }
            else
            {
                if (lastHitTime < creatureData.HitDelay) return;
                if (lastAttackTime >= creatureData.AttackDelay && IsPlayerInFieldOfView())
                {
                    Attack();
                }
            }
        }

        private float GetRandomSpeed(float baseSpeed)
        {
            return Random.Range(baseSpeed - 0.5f, baseSpeed + 0.5f);
        }

        protected bool IsPlayerInFieldOfView()
        {
            Vector3 directionToPlayer = playerTr.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            return angle < creatureData.FieldOfView * 0.5f;
        }

        protected virtual void Attack()
        {
            lastAttackTime = 0f;
            stateMachine.StateChange(CreatureState.Attack);
            if (IsTargetInAttackRange())
            {
                var controller = CharacterManager.Instance.Player.controller;
                controller.GetHit(creatureData.AttackPower);
            }
        }

        protected bool IsTargetInAttackRange()
        {
            return Vector3.Distance(playerTr.position, transform.position) <= creatureData.AttackDistance + 0.3f;
        }

        protected virtual void OnAttackEnd()
        {
            if (isDead) return;
            stateMachine.StateChange(CreatureState.Run);
        }

        public virtual void OnHit(float damage)
        {
            if (isDead) return;
            if (lastHitTime < creatureData.HitDelay) return;
            lastHitTime = 0f;

            StopAllCoroutines();
            stateMachine.StateChange(CreatureState.Hit);
            currentHp = Mathf.Max(currentHp - damage, 0);
            Global.Instance.UIManager.UpdateCreatureHpBar(this, currentHp);
            Global.Instance.AudioManager.PlaySFX(Utils.SoundTypeEnum.MonsterHit.GetClipName());
            if (currentHp <= 0)
            {
                Global.Instance.UIManager.InactiveCreatureHpBar(this);
                Die();
            }
        }

        protected virtual void OnHitEnd()
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

        protected void Die()
        {
            isDead = true;
            stateMachine.StateChange(CreatureState.Die);
            creatureSpawner.CreatureDied();
            Global.Instance.AudioManager.PlaySFX(creatureData.DeadSound.GetClipName());
            StartCoroutine(DieCoroutine());
        }

        private IEnumerator DieCoroutine()
        {
            yield return new WaitForSeconds(5f);
            Global.Instance.PoolManager.Despawn(gameObject);
            for (int i = 0; i < creatureData.DropItems.Length; i++)
            {
                var dropItem = creatureData.DropItems[i];
                Global.Instance.PoolManager.Spawn(dropItem.Name, transform.position + Vector3.up);
            }
        }

        public void CreatureSpawned(CreatureSpawner spawner)
        {
            creatureSpawner = spawner;
        }

        public virtual void OnPlayerDetected(Transform player)
        {
            if (isDead || playerTr) return;
            playerTr = player;
            navMeshAgent.speed = GetRandomSpeed(creatureData.CombatSpeed);
            stateMachine.StateChange(CreatureState.Run);
            stateMachine.OnPlayerDetected?.Invoke();
            Global.Instance.UIManager.ActiveCreatureHpBar(this, transform, creatureData.MaxHp);
            Global.Instance.UIManager.UpdateCreatureHpBar(this, currentHp);
            StopAllCoroutines();
        }

        public virtual void OnPlayerFaraway()
        {
            if (isDead || !playerTr) return;
            playerTr = null;
            navMeshAgent.speed = GetRandomSpeed(creatureData.OriginSpeed);
            stateMachine.OnPlayerFaraway?.Invoke();
            Global.Instance.UIManager.InactiveCreatureHpBar(this);
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