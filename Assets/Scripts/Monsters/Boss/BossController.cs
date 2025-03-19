using _23DaysLeft.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace _23DaysLeft.Monsters
{
    public class BossController : CreatureController
    {
        private float attackMultipler = 1.0f;
        [SerializeField] private bool isAttackCoolTime = false;

        public override void Init(Creature bossMonster)
        {
            // 초기화
            stateMachine = bossMonster.StateMachine;
            creatureData = bossMonster.CreatureData;
            navMeshAgent = bossMonster.NavMeshAgent;
            currentHp = creatureData.MaxHp;
            lastAttackTime = creatureData.AttackDelay;
            idleWaitTime = new WaitForSeconds(creatureData.AttackDelay);
            isDead = false;

            // State 초기화
            var player = CharacterManager.Instance.Player;
            OnPlayerDetected(player.transform);
        }

        // 어택 쿨타임 중에는 return
        protected override void Chasing()
        {
            if (isAttackCoolTime) return;
            Vector3 direction = (playerTr.position - transform.position).normalized;
            Vector3 desiredPos = playerTr.position - direction * creatureData.AttackDistance;

            if (Vector3.Distance(playerTr.position, transform.position) > creatureData.AttackDistance + 0.5f)
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
                }
            }
        }

        protected override IEnumerator Idle()
        {
            stateMachine.StateChange(CreatureState.Idle);
            yield return idleWaitTime;
            isAttackCoolTime = false;
            stateMachine.StateChange(CreatureState.Run);
        }

        public override void OnHit(float damage)
        {
            if (isDead) return;
            currentHp = Mathf.Max(currentHp - damage, 0);
            if (currentHp <= 0)
            {
                Global.Instance.UIManager.InactiveBossInfoPanel();
                Die();
            }
        }

        protected override void OnHitEnd() { }

        // attack 패턴 중 랜덤으로 선택
        protected override void Attack()
        {
            if (isDead || isAttackCoolTime) return;
            isAttackCoolTime = true;
            stateMachine.StateChange(CreatureState.Attack);
        }

        // attack 후 쿨타임을 가지기 위해 Idle로 전환
        protected override void OnAttackEnd()
        {
            if (isDead) return;
            StartCoroutine(Idle());
        }

        public void AttackMultiplierChange(int pattern)
        {
            attackMultipler = pattern switch
            {
                0 => 1.0f,
                1 => 1.5f,
                2 => 2.0f,
                _ => 1.0f
            };
        }

        public override void OnPlayerDetected(Transform player)
        {
            if (isDead || playerTr) return;
            playerTr = player;
            navMeshAgent.speed = creatureData.CombatSpeed;
            stateMachine.StateChange(CreatureState.Run);
            stateMachine.OnPlayerDetected?.Invoke();
        }

        public override void OnPlayerFaraway() { }

        #region AttackAnimEvent

        public void AnimEvent_NormalAttack()
        {
            if (isDead) return;
            if (IsTargetInAttackRange())
            {
                Debug.Log("NormalAttack Hit");
                var damage = creatureData.AttackPower * attackMultipler;
                CharacterManager.Instance.Player.controller.GetHit(damage);
            }
        }

        public void AnimEvent_KickAttack()
        {
            if (isDead) return;
            if (IsTargetInAttackRange())
            {
                Debug.Log("KickAttack Hit");
                var damage = creatureData.AttackPower * attackMultipler;
                CharacterManager.Instance.Player.controller.GetHit(damage);
                Global.Instance.AudioManager.PlaySFX(SoundTypeEnum.BossKickAttack.GetClipName());
            }
        }

        public void AnimEvent_JumpAttack()
        {
            if (isDead) return;
            if (Vector3.Distance(playerTr.position, transform.position) < 5.0f)
            {
                Debug.Log("JumpAttack Hit");
                var damage = creatureData.AttackPower * attackMultipler;
                CharacterManager.Instance.Player.controller.GetHit(damage);
                Global.Instance.AudioManager.PlaySFX(SoundTypeEnum.BossKickAttack.GetClipName());
            }
        }

        public void AnimEvent_AttackEnd()
        {
            Debug.Log("AnimEvent_AttackEnd");
            OnAttackEnd();
        }

        #endregion
    }
}