using System.Collections;
using UnityEngine;

public class Tree : ResourceObject
{
    private bool isObjectAtPosition = false;

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
        if (_dayNightCycle.currentTime >= 0.4f && _dayNightCycle.currentTime < 0.41f && !isObjectAtPosition)
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
            // 나무 캐는 효과음
            if (resourceCurHealth <= 50 && gatherCount == 2)
            {
                Instantiate(dropResource, transform.position + Vector3.up * 5f + Vector3.forward, Quaternion.identity);
                gatherCount--;
            }
            if (resourceCurHealth <= 0 && gatherCount == 1)
            {
                Instantiate(dropResource, transform.position + Vector3.up * 5f + Vector3.forward, Quaternion.identity);
                gatherCount--;
                _animator.enabled = true;       // 넘어지는 애니메이션 실행
                _boxCollider.isTrigger = true;  // 부딪히지 않게 하기.
                StartCoroutine(TreeDisapear());
            }
        }
    }

    IEnumerator TreeDisapear()
    {
        yield return new WaitForSeconds(0.5f);
        _meshRenderer.enabled = false; // 메쉬 비활성화
        yield return new WaitForSeconds(0.5f);
        _animator.enabled = false;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("플레이어가 설치한 오브젝트"))
    //    {
    //        isObjectAtPosition = true;
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("플레이어가 설치한 오브젝트"))
    //    {
    //        isObjectAtPosition = false;
    //    }
    //}
}
