using _23DaysLeft.Monsters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTargetByWeapon : MonoBehaviour
{
    private PlayerAttackController attackController;

    private void Start()
    {
        attackController = CharacterManager.Instance.Player.attackController;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            attackController.attackTargetType = AttackTargetType.Wood;
            attackController.resourceObject = other.GetComponent<Tree>();
        }
        else if (other.CompareTag("Rock"))
        {
            attackController.attackTargetType = AttackTargetType.Ore;
            attackController.resourceObject = other.GetComponent<Rock>();
        }
        else if (other.CompareTag("Enemy"))
        {
            attackController.attackTargetType = AttackTargetType.Enemy;
            attackController.creatureController = other.GetComponent<CreatureController>();
        }
        else
        {
            attackController.attackTargetType = AttackTargetType.None;
        }
    }
}
