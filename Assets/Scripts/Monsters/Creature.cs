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
    [SerializeField] private NavMeshAgent savMeshAgent;
    
    public CreatureData CreatureData => creatureData;
    public CreatureStateMachine StateMachine => stateMachine;
    public CreatureController Controller => controller;
    public Animator Animator => animator;
    public NavMeshAgent NavMeshAgent => savMeshAgent;

    private void Start()
    {
        stateMachine.Init(this);
        controller.Init(this);
    }
}
