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
    // 장착하는 무기에 따라서 해당 무기만 활성화
    public GameObject equippedSword;
    public GameObject equippedAxe;
    public GameObject equippedPickaxe;
    public GameObject equippedBow;

    DetectTargetByWeapon detectTargetByWeapon;

    public EquippedWeaponType equippedWeaponType = EquippedWeaponType.Hand;
    public AttackTargetType attackTargetType;



    // 플레이어 몹 공격력, 나무 공격력, 광석 공격력, (내구도) 선언
    public float attackDamage;
    public float digWoodDamage;
    public float mineOreDamage;
    public float durability;

    public float attackRate;
    public float afterLastAttackTime;

    public int randomPunchHand;

    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();

    }

    // 장착 무기에 따라서, 능력치 할당.
    private void Start()
    {
        attackDamage = 10f;
        digWoodDamage = 10f;
        mineOreDamage = 5f;
        durability = 0f;
    }
    private void Update()
    {
        afterLastAttackTime += Time.deltaTime;
    }


    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && afterLastAttackTime >= attackRate)
        {
            afterLastAttackTime = 0f;
            switch (equippedWeaponType)
            {
                case EquippedWeaponType.Hand:
                    randomPunchHand = Random.Range(0, 2);
                    if (randomPunchHand == 0)
                    {
                        _animator.SetTrigger("DoPunchR");
                    }
                    if (randomPunchHand == 1)
                    {
                        _animator.SetTrigger("DoPunchL");
                    }
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
                break;
            case AttackTargetType.Wood:
                break;
            case AttackTargetType.Ore:
                break;
        }
    }

    // 좌클릭 시 공격
    // 공격시 애니메이션 발동
    // 애니메이션에서 타격 지점에 Hit 메서드 실행
    // Hit 매서드에서 크로스헤어에 대상이 있는 지 없는 지에 따라서 다르게 실행
    // 대상이 있고, 만약 그 대상이 나무(광석, 몬스터)일 경우, 나무(광석, 몬스터) 공격력 전달.
}
