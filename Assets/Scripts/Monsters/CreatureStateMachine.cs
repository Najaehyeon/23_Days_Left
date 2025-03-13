using System;
using UnityEditor.Animations;
using UnityEngine;

namespace _23DaysLeft.Monsters
{
    public class CreatureStateMachine : StateMachine<Creature>
    {
        public Action OnPlayerDetected;
        public Action OnPlayerFaraway;
        public Action OnHitAnimationEnd;
        
        private readonly int hashWalk = Animator.StringToHash("IsWalk");
        private readonly int hashRun = Animator.StringToHash("IsRun");
        private readonly int hashCombat = Animator.StringToHash("IsCombat");
        private readonly int hashAttack = Animator.StringToHash("Attack");
        private readonly int hashHit = Animator.StringToHash("Hit");

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
            anim.SetBool(hashRun, true);
        }
        
        protected override void Run_Update()
        {
            currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.normalizedTime >= 1f)
            {
                OnHitAnimationEnd?.Invoke();
            }
        }
        
        protected override void Run_Exit()
        {
            anim.SetBool(hashRun, false);
        }
        
        protected override void Attack_Enter()
        {
            navMeshAgent.isStopped = true;
            anim.SetBool(hashAttack, true);
        }
        
        protected override void Attack_Update()
        {
        }
        
        protected override void Attack_Exit()
        {
            anim.SetBool(hashAttack, false);
        }
        
        protected override void Hit_Enter()
        {
            navMeshAgent.isStopped = true;
            anim.SetTrigger(hashHit);
        }
        
        protected override void Hit_Update()
        {
        }
        
        protected override void Hit_Exit()
        {
        }
        
        protected override void Die_Enter()
        {
            base.Die_Enter();
        }
        
        protected override void Die_Update()
        {
            base.Die_Update();
        }
        
        protected override void Die_Exit()
        {
            base.Die_Exit();
        }
    }
}