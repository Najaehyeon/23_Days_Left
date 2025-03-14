using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : ResourceObject
{
    private void Update()
    {
        RespawnResource();
    }

    private void OnCollisionEnter(Collision collision)  // ÇÃ·¹ÀÌ¾î°¡ Ä¶ ¶§
    {
        if (collision.gameObject.CompareTag("ÇÃ·¹ÀÌ¾î ¹«±â"))
        {
            if (remainDigCount >= 0)
            {
                Instantiate(dropResource, transform.position + Vector3.up, Quaternion.identity);
                // ±ø±ø ¼Ò¸®
            }
            else 
            {
                // Æ½Æ½ ¼Ò¸®
            }
        }
    }
}
