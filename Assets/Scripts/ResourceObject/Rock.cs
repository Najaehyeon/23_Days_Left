using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : ResourceObject
{
    private void Update()
    {
        RespawnResource();
    }

    private void OnCollisionEnter(Collision collision)  // �÷��̾ Ķ ��
    {
        if (collision.gameObject.CompareTag("�÷��̾� ����"))
        {
            if (remainDigCount >= 0)
            {
                Instantiate(dropResource, transform.position + Vector3.up, Quaternion.identity);
                // ���� �Ҹ�
            }
            else 
            {
                // ƽƽ �Ҹ�
            }
        }
    }
}
