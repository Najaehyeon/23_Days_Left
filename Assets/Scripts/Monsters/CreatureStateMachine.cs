using UnityEngine;

namespace _23DaysLeft.Monsters
{
    public class CreatureStateMachine : StateMachine<Creature>
    {
        private readonly int hashWalk = Animator.StringToHash("IsWalk");
        private readonly int hashRun = Animator.StringToHash("IsRun");
        private readonly int hashAttack = Animator.StringToHash("IsAttack");

        public override void Init(Creature creature)
        {
            anim = creature.Animator;
            navMeshAgent = creature.NavMeshAgent;
        }

        protected override void Idle_Enter()
        {
            navMeshAgent.isStopped = false;
            anim.SetBool(hashWalk, false);
        }
        
        protected override void Idle_Update()
        {
        }
        
        protected override void Idle_Exit()
        {
            navMeshAgent.isStopped = true;
        }
        
        protected override void Walk_Enter()
        {
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
        }
        
        protected override void Run_Exit()
        {
            anim.SetBool(hashRun, false);
        }
        
        protected override void Attack_Enter()
        {
            anim.SetTrigger(hashAttack);
        }
        
        protected override void Attack_Update()
        {
        }
        
        protected override void Attack_Exit()
        {
        }
        
        protected override void Hit_Enter()
        {
            base.Hit_Enter();
        }
        
        protected override void Hit_Update()
        {
            base.Hit_Update();
        }
        
        protected override void Hit_Exit()
        {
            base.Hit_Exit();
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