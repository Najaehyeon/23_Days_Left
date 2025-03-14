using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace _23DaysLeft.Monsters
{
    public class StateMachine<T> : MonoBehaviour
    {
        protected NavMeshAgent navMeshAgent;
        protected Animator anim;
        public CreatureState creatureState = CreatureState.None;

        public virtual void Init(T actor) { }

        protected virtual void Update()
        {
            StateUpdate();
        }

        #region MACHINE

        public void StateChange(CreatureState changeCreatureState)
        {
            switch (creatureState)
            {
                case CreatureState.None:
                    break;
                case CreatureState.Idle:
                    Idle_Exit();
                    break;
                case CreatureState.Walk:
                    Walk_Exit();
                    break;
                case CreatureState.Run:
                    Run_Exit();
                    break;
                case CreatureState.Attack:
                    Attack_Exit();
                    break;
                case CreatureState.Hit:
                    Hit_Exit();
                    break;
                case CreatureState.Die:
                    Die_Exit();
                    break;
            }

            creatureState = changeCreatureState;

            switch (creatureState)
            {
                case CreatureState.None:
                    break;
                case CreatureState.Idle:
                    Idle_Enter();
                    break;
                case CreatureState.Walk:
                    Walk_Enter();
                    break;
                case CreatureState.Run:
                    Run_Enter();
                    break;
                case CreatureState.Attack:
                    Attack_Enter();
                    break;
                case CreatureState.Hit:
                    Hit_Enter();
                    break;
                case CreatureState.Die:
                    Die_Enter();
                    break;
            }
        }

        private void StateUpdate()
        {
            switch (creatureState)
            {
                case CreatureState.None:
                    break;
                case CreatureState.Idle:
                    Idle_Update();
                    break;
                case CreatureState.Walk:
                    Walk_Update();
                    break;
                case CreatureState.Run:
                    Run_Update();
                    break;
                case CreatureState.Attack:
                    Attack_Update();
                    break;
                case CreatureState.Hit:
                    Hit_Update();
                    break;
                case CreatureState.Die:
                    Die_Update();
                    break;
            }
        }

        #endregion

        #region STATE

        protected virtual void Idle_Enter() { }

        protected virtual void Idle_Update() { }

        protected virtual void Idle_Exit() { }

        protected virtual void Walk_Enter() { }

        protected virtual void Walk_Update() { }

        protected virtual void Walk_Exit() { }

        protected virtual void Run_Enter() { }

        protected virtual void Run_Update() { }

        protected virtual void Run_Exit() { }

        protected virtual void Attack_Enter() { }

        protected virtual void Attack_Update() { }

        protected virtual void Attack_Exit() { }

        protected virtual void Hit_Enter() { }

        protected virtual void Hit_Update() { }

        protected virtual void Hit_Exit() { }

        protected virtual void Die_Enter() { }

        protected virtual void Die_Update() { }

        protected virtual void Die_Exit() { }

        #endregion
    }
}