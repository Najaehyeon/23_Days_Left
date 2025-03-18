using DaysLeft.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceObject : MonoBehaviour
{
    public float resourceMaxHealth = 100f;
    public float resourceCurHealth;
    public float gatherCount = 2; // 채집할 수 있는 남은 자원
    public GameObject dropResource;
    public WeaponItemData weaponItemData;
    public DayNightCycle _dayNightCycle;

    private void Start()
    {
        resourceCurHealth = resourceMaxHealth;
    }

    public abstract void RespawnResource();

    public abstract void mineResource(float damage);
}
