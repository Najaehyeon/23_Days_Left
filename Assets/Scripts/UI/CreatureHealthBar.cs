using System;
using _23DaysLeft.Managers;
using UnityEngine;
using UnityEngine.UI;

public class CreatureHealthBar : MonoBehaviour
{
    public Image fillImage;
    public Vector3 offset;
    
    private Transform targetTr;
    private Camera mainCamera;
    private float maxHealth;
    
    public void Init(Transform target, float maxHp)
    {
        transform.localScale = Vector3.one;
        maxHealth = maxHp;
        targetTr = target;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!targetTr) return;
        transform.position = targetTr.position + offset;
        transform.LookAt(mainCamera.transform);
    }

    public void ChangeHealth(float health)
    {
        fillImage.fillAmount = health / maxHealth;
    }
    
    public void Inactive()
    {
        targetTr = null;
        PoolManager.Instance.Despawn(gameObject);
    }
}
