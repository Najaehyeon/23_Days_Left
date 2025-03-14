using System;
using System.Collections;
using System.Collections.Generic;
using _23DaysLeft.Monsters;
using UnityEngine;
using UnityEngine.AI;

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
