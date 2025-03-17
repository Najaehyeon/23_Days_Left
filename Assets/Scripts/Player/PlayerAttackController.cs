using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    // �����ϴ� ���⿡ ���� �ش� ���⸸ Ȱ��ȭ
    public GameObject equippedSword;
    public GameObject equippedAxe;
    public GameObject equippedPickaxe;
    public GameObject equippedBow;

    // �÷��̾� �� ���ݷ�, ���� ���ݷ�, ���� ���ݷ�, (������) ����
    public float attackDamage;
    public float digWoodDamage;
    public float mineOreDamage;
    public float durability;

    public float attackRate;
    public float afterLastAttackTime;

    public int randomPunchHand;

    [SerializeField] ResourceObject tree;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();

    }

    // ���� ���⿡ ����, �ɷ�ġ �Ҵ�.
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

    public void ApplyDamage()
    {
        tree.mineResource(digWoodDamage);
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && afterLastAttackTime >= attackRate)
        {
            afterLastAttackTime = 0f;
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
    }
    // ��Ŭ�� �� ����
    // ���ݽ� �ִϸ��̼� �ߵ�
    // �ִϸ��̼ǿ��� Ÿ�� ������ Hit �޼��� ����
    // Hit �ż��忡�� ũ�ν��� ����� �ִ� �� ���� ���� ���� �ٸ��� ����
    // ����� �ְ�, ���� �� ����� ����(����, ����)�� ���, ����(����, ����) ���ݷ� ����.
}
