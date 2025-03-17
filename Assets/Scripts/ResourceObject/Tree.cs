using System.Collections;
using UnityEngine;

public class Tree : ResourceObject
{
    private bool isObjectAtPosition;

    BoxCollider _boxCollider;
    MeshRenderer _meshRenderer;
    Animator _animator;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RespawnResource();
    }

    public override void RespawnResource()
    {
        if (_dayNightCycle.currentTime == 0.4f && !isObjectAtPosition)
        {
            resourceCurHealth = resourceMaxHealth;
            _meshRenderer.enabled = true;
            _boxCollider.isTrigger = false;
            gatherCount = 2;
        }
    }

    public override void mineResource(float damage)
    {
        if (resourceCurHealth > 0)
        {
            resourceCurHealth -= damage;
            // ���� ĳ�� ȿ����
            if (resourceCurHealth <= 50 && gatherCount == 2)
            {
                Instantiate(dropResource, transform.position + Vector3.up + Vector3.forward, Quaternion.identity);
                gatherCount--;
            }
            if (resourceCurHealth <= 0 && gatherCount == 1)
            {
                Instantiate(dropResource, transform.position + Vector3.up + Vector3.forward, Quaternion.identity);
                gatherCount--;
                _animator.enabled = true;       // �Ѿ����� �ִϸ��̼� ����
                _boxCollider.isTrigger = true;  // �ε����� �ʰ� �ϱ�.
                StartCoroutine(TreeDown());
            }
        }
    }

    IEnumerator TreeDown()
    {
        yield return new WaitForSeconds(0.5f);
        _meshRenderer.enabled = false; // �޽� ��Ȱ��ȭ
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("�÷��̾ ��ġ�� ������Ʈ"))
        {
            isObjectAtPosition = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("�÷��̾ ��ġ�� ������Ʈ"))
        {
            isObjectAtPosition = false;
        }
    }
}
