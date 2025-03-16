using _23DaysLeft.Monsters;
using UnityEngine;

public class BossStateMachine : CreatureStateMachine
{
    private BossController bossController;

    private readonly int hashNormalAttack = Animator.StringToHash("NormalAttack");
    private readonly int hashKickAttack = Animator.StringToHash("KickAttack");
    private readonly int hashJumpAttack = Animator.StringToHash("JumpAttack");

    public override void Init(Creature creature)
    {
        anim = creature.Animator;
        navMeshAgent = creature.NavMeshAgent;
        bossController = creature.Controller as BossController;
    }

    protected override void Idle_Enter()
    {
        navMeshAgent.isStopped = true;
        anim.SetBool(hashRun, false);
    }

    protected override void Idle_Update() { }

    protected override void Idle_Exit() { }

    protected override void Walk_Enter() { }

    protected override void Walk_Update() { }

    protected override void Walk_Exit() { }

    protected override void Attack_Enter()
    {
        var attackPattern = GetRandomAttackPattern(AttackPattern.pattern1);
        bossController.AttackMultiplierChange(attackPattern);
        anim.SetTrigger(attackPattern);
        navMeshAgent.enabled = false;
    }

    protected override void Attack_Update()
    {
    }

    protected override void Attack_Exit()
    {
        navMeshAgent.enabled = true;   
    }

    protected override void Hit_Enter() { }

    protected override void Hit_Update() { }

    protected override void Hit_Exit() { }

    private int GetRandomAttackPattern(AttackPattern pattern)
    {
        var rand = Random.Range(0, 3);
        return rand switch
        {
            0 => hashNormalAttack,
            1 => hashKickAttack,
            2 => hashJumpAttack,
            _ => hashNormalAttack
        };
    }
}

public enum AttackPattern
{
    pattern1,
    pattern2,
    pattern3,
}