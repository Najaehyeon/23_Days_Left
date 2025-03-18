using System;
using UnityEngine;

namespace _23DaysLeft.Monsters
{
    public class CreatureStateMachine : StateMachine<Creature>
    {
        public Action OnPlayerDetected;
        public Action OnPlayerFaraway;
        public Action OnHitAnimationEnd;
        public Action OnAttackAnimationEnd;
        
        protected readonly int hashWalk = Animator.StringToHash("IsWalk");
        protected readonly int hashRun = Animator.StringToHash("IsRun");
        protected readonly int hashCombat = Animator.StringToHash("IsCombat");
        protected readonly int hashAttack = Animator.StringToHash("Attack");
        protected readonly int hashHit = Animator.StringToHash("Hit");
        protected readonly int hashDie = Animator.StringToHash("Die");

        private float enterTime;
        private AnimatorStateInfo currentState;
        
        public override void Init(Creature creature)
        {
            anim = creature.Animator;
            navMeshAgent = creature.NavMeshAgent;

            OnPlayerDetected += () => { anim.SetBool(hashCombat, true); };
            OnPlayerFaraway += () => { anim.SetBool(hashCombat, false); };
        }

        protected override void Idle_Enter()
        {
            navMeshAgent.isStopped = true;
            anim.SetBool(hashWalk, false);
        }
        
        protected override void Idle_Update()
        {
        }
        
        protected override void Idle_Exit()
        {
        }
        
        protected override void Walk_Enter()
        {
            navMeshAgent.isStopped = false;
            anim.SetBool(hashWalk, true);
        }
        
        protected override void Walk_Update()
        {
        }
        
        protected override void Walk_Exit()
        {
            anim.SetBool(hashWalk, false);
        }

        protected override void Run_Enter()
        {
            navMeshAgent.isStopped = false;
            anim.SetBool(hashRun, true);
        }
        
        protected override void Run_Update()
        {
            
        }
        
        protected override void Run_Exit()
        {
            anim.SetBool(hashRun, false);
        }
        
        protected override void Attack_Enter()
        {
            anim.SetBool(hashAttack, true);
            currentState = anim.GetCurrentAnimatorStateInfo(0);
            enterTime = 0;
        }
        
        protected override void Attack_Update()
        {
            navMeshAgent.velocity = Vector3.zero;
            enterTime += Time.deltaTime * currentState.normalizedTime;
            if (enterTime >= 1f)
            {
                OnAttackAnimationEnd?.Invoke();
            }
        }
        
        protected override void Attack_Exit()
        {
            anim.SetBool(hashAttack, false);
        }
        
        protected override void Hit_Enter()
        {
            anim.SetTrigger(hashHit);
            currentState = anim.GetCurrentAnimatorStateInfo(0);
            enterTime = 0;
        }
        
        protected override void Hit_Update()
        {
            navMeshAgent.velocity = Vector3.zero;
            enterTime += Time.deltaTime * currentState.normalizedTime;
            if (enterTime >= 1f)
            {
                OnHitAnimationEnd?.Invoke();
            }
        }
        
        protected override void Hit_Exit()
        {
        }
        
        protected override void Die_Enter()
        {
            anim.SetTrigger(hashDie);
        }
        
        protected override void Die_Update()
        {
        }
        
        protected override void Die_Exit()
        {
        }
    }
}