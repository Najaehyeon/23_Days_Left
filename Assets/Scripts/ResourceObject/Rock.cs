using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : ResourceObject
{
    private void Update()
    {
        RespawnResource();
    }

    public override void RespawnResource()
    {
        if (_dayNightCycle.currentTime == 0.4f)
        {
            resourceCurHealth = resourceMaxHealth;
            gatherCount = 2;
        }
    }

    public override void mineResource(float damage)
    {
        if (resourceCurHealth > 0)
        {
            resourceCurHealth -= damage;
            // 兵兵 家府 瓤苞澜
            if (resourceCurHealth <= 50 && gatherCount == 2)
            {
                Instantiate(dropResource, transform.position + Vector3.up + Vector3.forward, Quaternion.identity);
                gatherCount--;
            }
            if (resourceCurHealth <= 0 && gatherCount == 1)
            {
                Instantiate(dropResource, transform.position + Vector3.up + Vector3.forward, Quaternion.identity);
                gatherCount--;
            }
        }
        else
        {
            // 平平 家府 瓤苞澜
        }
    }
}
