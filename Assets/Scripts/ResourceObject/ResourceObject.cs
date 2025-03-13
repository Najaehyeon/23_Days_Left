using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public float digCount;
    public float remainDigCount;
    public GameObject dropResource;

    public DayNightCycle _dayNightCycle;

    private void Start()
    {
        remainDigCount = digCount;
    }

    public virtual void RespawnTree()
    {
        if (_dayNightCycle.currentTime == 0.4f)
        {
            remainDigCount = digCount;
        }
    }
}
