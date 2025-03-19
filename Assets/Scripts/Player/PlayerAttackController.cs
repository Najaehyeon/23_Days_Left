using _23DaysLeft.Utils;
using _23DaysLeft.Monsters;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Diagnostics;

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

    [Header("EquippedArmor")]
    public GameObject HelmetPlate;
    public GameObject ChestPlate;
    public GameObject PantsPlate;
    public GameObject BootsPlate;
    public GameObject ShoulderPlate;
    public GameObject GlovesPlate;
    public GameObject ChestLeather;
    public GameObject PantsLeather;
    public GameObject BootsLeather;

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
            Global.Instance.AudioManager.PlaySFX(SoundTypeEnum.Punch1.GetClipName());
        }
        if (randomPunchHand == 1)
        {
            _animator.SetTrigger("DoPunchL");
            Global.Instance.AudioManager.PlaySFX(SoundTypeEnum.Punch2.GetClipName());
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
        Global.Instance.AudioManager.PlaySFX(SoundTypeEnum.SwordSoundEffectts1.GetClipName());
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
    public void Equip(ArmorInstance instance)
    {
        if (instance == null)
            Debug.LogError("Null weapon assignd to player");

        switch (instance.armorType)
        {
            case ArmorType.Helmet:
                if(instance.materialType == MaterialType.Plate)
                {
                    HelmetPlate.SetActive(true);
                }
                break;
            case ArmorType.Chest:
                if (instance.materialType == MaterialType.Plate)
                {
                    ChestPlate.SetActive(true);
                    ChestLeather.SetActive(false);
                }
                else if (instance.materialType == MaterialType.Leather)
                {
                    ChestPlate.SetActive(false);
                    ChestLeather.SetActive(true);
                }
                break;
            case ArmorType.Panth:
                if (instance.materialType == MaterialType.Plate)
                {
                    PantsPlate.SetActive(true);
                    PantsLeather.SetActive(false);
                }
                else if (instance.materialType == MaterialType.Leather)
                {
                    PantsPlate.SetActive(false);
                    PantsLeather.SetActive(true);
                }
                break;
            case ArmorType.Boots:
                if (instance.materialType == MaterialType.Plate)
                {
                    BootsPlate.SetActive(true);
                    BootsLeather.SetActive(false);
                }
                else if (instance.materialType == MaterialType.Leather)
                {
                    BootsPlate.SetActive(false);
                    BootsLeather.SetActive(true);
                }
                break;
            case ArmorType.Glove:
                if (instance.materialType == MaterialType.Plate)
                {
                    GlovesPlate.SetActive(true);
                }                
                break;
            case ArmorType.Shoulders:
                if (instance.materialType == MaterialType.Plate)
                {
                    ShoulderPlate.SetActive(true);
                }
                break;
        }
    }
}
