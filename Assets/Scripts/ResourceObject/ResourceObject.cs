using DaysLeft.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public float resourceHealth = 100f;
    public GameObject dropResource;
    public WeaponItemData weaponItemData;
    public DayNightCycle _dayNightCycle;

    private void Start()
    {

    }

    public virtual void RespawnResource()
    {
        if (_dayNightCycle.currentTime == 0.4f)
        {
            resourceHealth = 100;
        }
    }

    public virtual void DigAndDropResource()
    {

    }
}
