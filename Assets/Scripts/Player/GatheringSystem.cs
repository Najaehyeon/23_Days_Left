using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSystem : MonoBehaviour
{
    SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            
        }
    }
}
