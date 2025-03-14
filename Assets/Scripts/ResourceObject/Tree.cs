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

    private void OnCollisionEnter(Collision collision)  // 플레이어가 캘 때
    {
        if (collision.gameObject.CompareTag("플레이어 무기"))
        {
            if (remainDigCount > 0)
            {
                Instantiate(dropResource, transform.position + Vector3.up, Quaternion.identity);
                remainDigCount--;
            }
            else if (remainDigCount == 0)
            {
                Instantiate(dropResource, transform.position + Vector3.up, Quaternion.identity);
                _animator.enabled = true;       // 넘어지는 애니메이션 실행
                _boxCollider.isTrigger = true;  // 
                StartCoroutine(TreeDown());
            }
        }
    }

    IEnumerator TreeDown()
    {
        yield return new WaitForSeconds(0.5f);
        // 메쉬 비활성화
        _meshRenderer.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("플레이어가 설치한 오브젝트"))
        {
            isObjectAtPosition = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("플레이어가 설치한 오브젝트"))
        {
            isObjectAtPosition = false;
        }
    }
}
