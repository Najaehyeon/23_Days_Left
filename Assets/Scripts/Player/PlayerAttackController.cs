using _23DaysLeft.Monsters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EquippedWeaponType
{
    Hand,
    Sword,
    Axe,
    Pickaxe,
    Bow
}

public enum AttackTargetType
{
    None,
    Enemy,
    Wood,
    Ore
}

public class PlayerAttackController : MonoBehaviour
{
    PlayerController playerController;

    [Header("EquippedWeapon")]
    public EquippedWeaponType equippedWeaponType = EquippedWeaponType.Hand;
    public GameObject equippedSword;
    public GameObject equippedAxe;
    public GameObject equippedPickaxe;
    public GameObject equippedBow;

    [Header("Target")]
    public AttackTargetType attackTargetType;
    public ResourceObject resourceObject;
    public CreatureController creatureController;

    [Header("Stat")]
    public float attackDamage;
    public float digWoodDamage;
    public float mineOreDamage;
    public float durability;

    [Header("Attack System")]
    private Animator _animator;
    public float attackRate;
    private float afterLastAttackTime;
    private int randomPunchHand;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;

        attackDamage = 10f;
        digWoodDamage = 10f;
        mineOreDamage = 5f;
        durability = 0f;
    }

    private void Update()
    {
        afterLastAttackTime += Time.deltaTime;
        if (afterLastAttackTime > 1)
        {
            playerController.isAttack = false;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && afterLastAttackTime >= attackRate && !playerController.IsDead)
        {
            afterLastAttackTime = 0f;
            playerController.isAttack = true;
            switch (equippedWeaponType)
            {
                // 각 공격 애니메이션에서 이벤트로 ApplyDamage() 메서드 실행
                case EquippedWeaponType.Hand:
                    RandomHandPunchAnimation();
                    break;
                case EquippedWeaponType.Sword:
                    _animator.SetTrigger("DoOneHandAttack"); 
                    break;
                case EquippedWeaponType.Axe:
                    _animator.SetTrigger("DoOneHandAttack");
                    break;
                case EquippedWeaponType.Pickaxe:
                    _animator.SetTrigger("DoTwoHandAttack");
                    break;
                case EquippedWeaponType.Bow:
                    _animator.SetTrigger("DoBowShot");
                    break;
            }
        }
    }

    void RandomHandPunchAnimation()
    {
        randomPunchHand = Random.Range(0, 2);
        if (randomPunchHand == 0)
        {
            _animator.SetTrigger("DoPunchR");
        }
        if (randomPunchHand == 1)
        {
            _animator.SetTrigger("DoPunchL");
        }
    }

    // 각 공격 애니메이션에서 이벤트로 실행되는 메서드
    public void ApplyDamage()
    {
        switch (equippedWeaponType)
        {
            case EquippedWeaponType.Hand:
                AttackByTarget();
                break;
            case EquippedWeaponType.Sword:
                AttackByTarget();
                break;
            case EquippedWeaponType.Axe:
                AttackByTarget();
                break;
            case EquippedWeaponType.Pickaxe:
                AttackByTarget();
                break;
            case EquippedWeaponType.Bow:
                AttackByTarget();
                break;
        }
    }

    void AttackByTarget()
    {
        switch (attackTargetType)
        {
            case AttackTargetType.None:
                break;
            case AttackTargetType.Enemy:
                creatureController.OnHit(attackDamage);
                break;
            case AttackTargetType.Wood:
                resourceObject.mineResource(digWoodDamage);
                break;
            case AttackTargetType.Ore:
                resourceObject.mineResource(mineOreDamage);
                break;
        }
    }

    public void Equip(WeaponInstance instance)
    {
        if (instance == null)
            Debug.LogError("Null weapon assignd to player");

        equippedWeaponType = instance.WeaponType;

        switch (equippedWeaponType)
        {
            case EquippedWeaponType.Hand:
                equippedBow.SetActive(false);
                equippedAxe.SetActive(false);
                equippedSword.SetActive(false);
                equippedPickaxe.SetActive(false);
                break;
            case EquippedWeaponType.Sword:
                equippedBow.SetActive(false);
                equippedAxe.SetActive(false);
                equippedSword.SetActive(true);
                equippedPickaxe.SetActive(false);
                break;
            case EquippedWeaponType.Axe:
                equippedBow.SetActive(false);
                equippedAxe.SetActive(true);
                equippedSword.SetActive(false);
                equippedPickaxe.SetActive(false);
                break;
            case EquippedWeaponType.Pickaxe:
                equippedBow.SetActive(false);
                equippedAxe.SetActive(false);
                equippedSword.SetActive(false);
                equippedPickaxe.SetActive(true);
                break;
            case EquippedWeaponType.Bow:
                equippedBow.SetActive(true);
                equippedAxe.SetActive(false);
                equippedSword.SetActive(false);
                equippedPickaxe.SetActive(false);
                break;
        }

        attackDamage = instance.AttackDamage;
        mineOreDamage = instance.MineOreDamage;
        digWoodDamage = instance.DigWoodDamage;
    }
}
