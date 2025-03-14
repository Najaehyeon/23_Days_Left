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
            _meshRenderer.enabled = true;
            _boxCollider.isTrigger = false;
        }
    }

    public override void DigAndDropResource()
    {
        
    }

    private void OnCollisionEnter(Collision collision)  // �÷��̾ Ķ ��
    {
        if (collision.gameObject.CompareTag("�÷��̾� ����"))
        {
            if (remainDigCount > 0)
            {
                Instantiate(dropResource, transform.position + Vector3.up, Quaternion.identity);
                remainDigCount--;
            }
            else if (remainDigCount == 0)
            {
                Instantiate(dropResource, transform.position + Vector3.up, Quaternion.identity);
                _animator.enabled = true;       // �Ѿ����� �ִϸ��̼� ����
                _boxCollider.isTrigger = true;  // 
                StartCoroutine(TreeDown());
            }
        }
    }

    IEnumerator TreeDown()
    {
        yield return new WaitForSeconds(0.5f);
        // �޽� ��Ȱ��ȭ
        _meshRenderer.enabled = false;
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
